using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Services.Common;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Invoices
{
    public class InvoiceService : IInvoiceService
    {
        #region Private Members

        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly IRepository<InvoiceItem> _invoiceItemRepository;
        private readonly IRepository<Discount> _discountRepository;
        private readonly IRepository<DiscountItem> _discountItemRepository;
        private readonly IRepository<Organization> _orgRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<CashierPayment> _paymentRepository;
        private readonly IRepository<Stock> _stockRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ISettings _settings;

        #endregion

        #region Constructors

        public InvoiceService(
            IRepository<Invoice> invoiceRepository,
            IRepository<InvoiceItem> invoiceItemRepository,
            IRepository<Discount> discountRepository,
            IRepository<DiscountItem> discountItemRepository,
            IRepository<Organization> orgRepository,
            IRepository<Session> sessionRepository,
            IRepository<CashierPayment> paymentRepository,
            IRepository<Stock> stockRepository,
            IRepository<Customer> customerRepository,
            ISettings settings)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceItemRepository = invoiceItemRepository;
            _discountRepository = discountRepository;
            _discountItemRepository = discountItemRepository;
            _orgRepository = orgRepository;
            _sessionRepository = sessionRepository;
            _paymentRepository = paymentRepository;
            _stockRepository = stockRepository;
            _customerRepository = customerRepository;
            _settings = settings;
        }

        #endregion

        #region Public Methods

        public async Task<EntityApiResponse<InvoiceDetail>> AddInvoiceAsync(InvoiceDetail invoiceDetail, string currentUserId)
        {
            if (invoiceDetail is null)
                throw new ArgumentNullException(nameof(invoiceDetail));

            if (invoiceDetail.InvoiceItems is null || invoiceDetail.InvoiceItems.Count < 1)
                return new EntityApiResponse<InvoiceDetail>(error: "Invoice has no items");

            if (invoiceDetail.InvoiceItems.Any(item => item.Quantity < 1))
                return new EntityApiResponse<InvoiceDetail>(error: "Invoice can't have an item with quantity zero");

            var org = await _orgRepository.GetByIdAsync(invoiceDetail.OrganizationId);

            if (org is null)
                return new EntityApiResponse<InvoiceDetail>(error: "Organization does not exist");

            Customer customer = null;

            var belongToCustomer = !(invoiceDetail.Customer is null);
            var paidWithPoints = invoiceDetail.PaidWithPoints > 0;

            if (belongToCustomer)
            {
                customer = await _customerRepository.GetByIdAsync(invoiceDetail.Customer.Id);
                
                if (customer is null)
                    return new EntityApiResponse<InvoiceDetail>(error: "Customer does not exist");

                if (paidWithPoints && customer.Points < invoiceDetail.PaidWithPoints)
                    return new EntityApiResponse<InvoiceDetail>(error: "Customer doesn't have enough points");

                if (paidWithPoints && !_settings.PointsValues.TryGetValue(invoiceDetail.PaidWithPoints, out var amount))
                    return new EntityApiResponse<InvoiceDetail>(error: $"{invoiceDetail.PaidWithPoints} points are not a standard exchangement");
            }

            // Check if the current user is in his specified session
            var now = DateTime.UtcNow;

            Session session = null;
            if (_settings.RestrictPaymentOnSession)
            {
                session = await _sessionRepository.Table
                    .FirstOrDefaultAsync(s => now >= s.StartDate && now <= s.EndDate && s.UserId == currentUserId);

                if (session is null)
                    return new EntityApiResponse<InvoiceDetail>(error: "Can't add invoice, current session belongs to another user");
            }

            var newInvoiceNumber = org.LastInvoiceNumber + 1;
            var invoicePoints = 0;

            var invoice = new Invoice
            {
                Number = newInvoiceNumber,
                Discount = invoiceDetail.Discount,
                OrganizationId = org.Id,
                CreatedById = currentUserId,
                ModifiedById = currentUserId,
                OriginalPrice = invoiceDetail.OriginalPrice,
                FinalPrice = invoiceDetail.FinalPrice,
                Note = invoiceDetail.Note?.Trim(),
                CustomerId = customer?.Id
            };

            await _invoiceRepository.InsertAsync(invoice);

            foreach (var item in invoiceDetail.InvoiceItems)
            {
                var stock = await _stockRepository.GetByIdAsync(item.Stock?.Id);

                if (stock is null)
                    continue;

                var newInvoiceItem = new InvoiceItem
                {
                    Description = item.Description?.Trim(),
                    Quantity = item.Quantity,
                    Price = item.Price,
                    FinalPrice = item.FinalPrice,
                    Discount = item.Discount,
                    CreatedById = currentUserId,
                    ModifiedById = currentUserId,
                    OrganizationId = org.Id,
                    StockId = stock.Id,
                    InvoiceId = invoice.Id
                };

                await _invoiceItemRepository.InsertAsync(newInvoiceItem);

                stock.Quantity -= newInvoiceItem.Quantity;
                stock.LastModifiedDate = DateTime.UtcNow;
                stock.ModifiedById = currentUserId;

                await _stockRepository.UpdateAsync(stock);

                invoicePoints += stock.Points * newInvoiceItem.Quantity;
            }

            // Check points for updating customer's balance
            if (belongToCustomer)
            {
                customer.Points += invoicePoints;
                if (paidWithPoints) customer.Points -= invoiceDetail.PaidWithPoints;

                await _customerRepository.UpdateAsync(customer);
            }

            var payment = new CashierPayment
            {
                CashEntered = invoiceDetail.CashierPayment?.CashEntered ?? 0m,
                Change = invoiceDetail.CashierPayment?.Change ?? 0m,
                SessionId = session?.Id,
                OrganizationId = org.Id,
                InvoiceId = invoice.Id,
                CreatedById = currentUserId,
                ModifiedById = currentUserId
            };

            await _paymentRepository.InsertAsync(payment);

            invoice.Points = invoicePoints;
            await _invoiceRepository.UpdateAsync(invoice);

            org.LastInvoiceNumber += 1;
            await _orgRepository.UpdateAsync(org);

            return new EntityApiResponse<InvoiceDetail>(entity: new InvoiceDetail(invoice, payment));
        }

        public async Task<ApiResponse> DeleteInvoiceAsync(string invoiceId, string currentUserId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

            if (invoice is null)
                return new ApiResponse("Invoice does not exist");

            // Add all stock quantities
            foreach (var item in invoice.InvoiceItems)
            {
                var stock = await _stockRepository.GetByIdAsync(item.StockId);

                if (stock is null)
                    continue;

                stock.Quantity += item.Quantity;
                stock.LastModifiedDate = DateTime.UtcNow;
                stock.ModifiedById = currentUserId;

                await _stockRepository.UpdateAsync(stock);
            }

            await _invoiceItemRepository.DeleteAsync(invoice.InvoiceItems);

            // Add customer's points if there was a customer and the invoice was paid by points
            if (!string.IsNullOrEmpty(invoice.CustomerId))
            {
                var customer = await _customerRepository.GetByIdAsync(invoice.CustomerId);

                if (invoice.PaidWithPoints != 0) customer.Points += invoice.PaidWithPoints;

                customer.LastModifiedDate = DateTime.UtcNow;
                customer.ModifiedById = currentUserId;

                // Remove the points of the invoice from the customer's points balance
                customer.Points -= invoice.Points;

                await _customerRepository.UpdateAsync(customer);
            }

            // Get the related payment
            var payment = await _paymentRepository.Table.FirstOrDefaultAsync(p => p.InvoiceId == invoice.Id);

            // Delete the payment
            await _paymentRepository.DeleteAsync(payment);

            // Delete the invoice
            await _invoiceRepository.DeleteAsync(invoice);

            return new ApiResponse();
        }

        public async Task<EntityApiResponse<InvoiceDetail>> GetInvoiceDetailsAsync(string invoiceId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

            if (invoice is null)
                return new EntityApiResponse<InvoiceDetail>(error: "Invoice does not exist");

            var invoicePayment = _paymentRepository.Table.FirstOrDefault(p => p.InvoiceId == invoice.Id);

            return new EntityApiResponse<InvoiceDetail>(entity: new InvoiceDetail(invoice, invoicePayment));
        }

        public async Task<EntitiesApiResponse<InvoiceDetail>> GetInvoicesForOrganizationAsync(string organizationId)
        {
            var org = await _orgRepository.GetByIdAsync(organizationId);

            if (org is null)
                return new EntitiesApiResponse<InvoiceDetail>(error: "Organization does not exist");

            var invoices = await _invoiceRepository.Table
                .Where(i => i.OrganizationId == org.Id).ToListAsync();

            var invoiceDetails = invoices.Select(i => new InvoiceDetail(i));

            return new EntitiesApiResponse<InvoiceDetail>(entities: invoiceDetails);
        }

        public async Task<EntityApiResponse<InvoiceDetail>> UpdateInvoiceAsync(InvoiceDetail invoiceDetail, string currentUserId)
        {
            // Do the checks
            if (invoiceDetail is null)
                throw new ArgumentNullException(nameof(invoiceDetail));

            if (invoiceDetail.InvoiceItems is null || invoiceDetail.InvoiceItems.Count < 1)
                return new EntityApiResponse<InvoiceDetail>(error: "Invoice has no items");

            if (invoiceDetail.InvoiceItems.Any(item => item.Quantity < 1))
                return new EntityApiResponse<InvoiceDetail>(error: "Invoice can't have an item with quantity zero");

            var invoice = await _invoiceRepository.GetByIdAsync(invoiceDetail.Id);

            if (invoice is null)
                return new EntityApiResponse<InvoiceDetail>(error: "Invoice does not exist");

            // Customer which points to the newly updated customer 
            // or the same customer before updating the invoice
            Customer customer = null;

            // Indicates if a customer has been selected with the new invoice details
            var belongsToCustomer = !(invoiceDetail.Customer is null);

            // Indicates if the invoice is paid/sub-paid by points
            var paidWithPoints = invoiceDetail.PaidWithPoints > 0;

            // Indicates if invoice before updated did belong to a customer
            var oldInvoiceBelongToCustomer = !string.IsNullOrEmpty(invoice.CustomerId);

            if (belongsToCustomer)
            {
                customer = await _customerRepository.GetByIdAsync(invoiceDetail.Customer?.Id);

                if (customer is null)
                    return new EntityApiResponse<InvoiceDetail>(error: "Customer does not exist");

                if (paidWithPoints)
                {
                    if (!_settings.PointsValues.TryGetValue(invoiceDetail.PaidWithPoints, out var amount))
                        return new EntityApiResponse<InvoiceDetail>(error: $"{invoiceDetail.PaidWithPoints} points are not a standard exchangement");

                    if (oldInvoiceBelongToCustomer && customer.Id != invoice.CustomerId && customer.Points < invoiceDetail.PaidWithPoints)
                        return new EntityApiResponse<InvoiceDetail>(error: "Customer doesn't have enough points");

                    // Basically this check is for when the same customer just gets back and takes a new item
                    // his old points that he paid with should be considered too
                    else if (oldInvoiceBelongToCustomer && customer.Id == invoice.Id && customer.Points + invoice.PaidWithPoints < invoiceDetail.PaidWithPoints)
                        return new EntityApiResponse<InvoiceDetail>(error: "Customer doesn't have enough points");
                }
            }

            // Check if the current user is in his specified session
            var now = DateTime.UtcNow;

            Session session = null;
            if (_settings.RestrictPaymentOnSession)
            {
                session = await _sessionRepository.Table
                    .FirstOrDefaultAsync(s => now >= s.StartDate && now <= s.EndDate && s.UserId == currentUserId);

                if (session is null)
                    return new EntityApiResponse<InvoiceDetail>(error: "Can't add invoice, current session belongs to another user");
            }

            // Get info needed about the old invoice items
            var invoiceNewPoints = 0;
            var invoiceOldPoints = invoice.Points;

            // Get the ones that should be deleted, add their quantities, delete them
            var oldItems = invoice.InvoiceItems.ToList();

            var itemsToBeDeleted = oldItems
                .Where(i => !invoiceDetail.InvoiceItems.Any(itemDetail => itemDetail.Id == i.Id));

            foreach (var item in itemsToBeDeleted)
            {
                var stock = await _stockRepository.GetByIdAsync(item.Stock?.Id);

                stock.Quantity += item.Quantity;
                stock.LastModifiedDate = DateTime.UtcNow;
                stock.ModifiedById = currentUserId;

                await _stockRepository.UpdateAsync(stock);
            }

            await _invoiceItemRepository.DeleteAsync(itemsToBeDeleted);

            // Go over new and updated items in the invoice details
            foreach (var invoiceItem in invoiceDetail.InvoiceItems)
            {
                var stock = await _stockRepository.GetByIdAsync(invoiceItem.Stock?.Id);

                if (stock is null)
                    continue;

                InvoiceItem item = null;
                if (string.IsNullOrEmpty(item.Id))
                {
                    item = new InvoiceItem
                    {
                        Description = invoiceItem.Description?.Trim(),
                        Discount = invoiceItem.Discount,
                        Price = invoiceItem.Price,
                        FinalPrice = invoiceItem.FinalPrice,
                        Quantity = invoiceItem.Quantity,
                        InvoiceId = invoice.Id,
                        StockId = stock.Id,
                        OrganizationId = invoice.OrganizationId,
                        CreatedById = currentUserId,
                        ModifiedById = currentUserId
                    };

                    await _invoiceItemRepository.InsertAsync(item);

                    stock.Quantity -= item.Quantity;
                }
                // Update the already existing ones
                else
                {
                    item = oldItems.FirstOrDefault(i => i.Id == invoiceItem.Id);

                    if (item is null)
                        continue;

                    stock.Quantity += item.Quantity - invoiceItem.Quantity;

                    item.Quantity = invoiceItem.Quantity;
                    item.Discount = invoice.Discount;
                    item.Description = invoiceItem.Description?.Trim();
                    item.Price = invoiceItem.Price;
                    item.FinalPrice = invoiceItem.FinalPrice;
                    item.ModifiedById = currentUserId;
                    item.LastModifiedDate = DateTime.UtcNow;

                    await _invoiceItemRepository.UpdateAsync(item);
                }

                stock.LastModifiedDate = DateTime.UtcNow;
                stock.ModifiedById = currentUserId;

                await _stockRepository.UpdateAsync(stock);

                invoiceNewPoints += stock.Points * item.Quantity;
            }

            if (belongsToCustomer)
            {
                // if the current customer is different than the old customer
                if (oldInvoiceBelongToCustomer && customer.Id != invoice.CustomerId)
                {
                    // Get the old customer
                    var oldCustomer = await _customerRepository.GetByIdAsync(invoice.CustomerId);

                    // Add to the old customer the old points that he paid with
                    oldCustomer.Points += invoice.PaidWithPoints;

                    // Remove from the old customer the old points that he got from the invoice
                    oldCustomer.Points -= invoiceOldPoints;

                    await _customerRepository.UpdateAsync(oldCustomer);

                    customer.Points -= invoiceDetail.PaidWithPoints;
                }
                // Same customer
                else if (oldInvoiceBelongToCustomer && customer.Id == invoice.CustomerId)
                {
                    // customer's points were initially 650k
                    // bought stuff with $65 --> 60$ with points so take away 600k and $5 cash as payment
                    // now he got back stuff and his current invoice is 20$
                    // he bought them with points (200k) soo he should now have 450k points and $5 returned to him
                    // or he could buy stuff with $25.. $20 with points and $5 cash
                    // so points added to him should be the difference between 
                    if (paidWithPoints)
                        customer.Points += invoice.PaidWithPoints - invoiceDetail.PaidWithPoints;

                    customer.Points -= invoiceOldPoints;
                }
                // There wasn't any customer selected when the invoice was added
                // but when the invoice was updated a customer has been added
                else if (!oldInvoiceBelongToCustomer && paidWithPoints)
                {
                    customer.Points -= invoiceDetail.PaidWithPoints;
                }

                // Add the invoice points to the current customer anyways
                customer.Points += invoiceNewPoints;
                await _customerRepository.UpdateAsync(customer);
            }
            // Invoice did belong to a customer but customer was unselected
            else if (!belongsToCustomer && oldInvoiceBelongToCustomer)
            {
                customer.Points -= invoiceOldPoints;
                customer.Points += invoice.PaidWithPoints;

                await _customerRepository.UpdateAsync(customer);
            }

            // Update the related payment
            var payment = await _paymentRepository.Table
                .FirstOrDefaultAsync(p => p.InvoiceId == invoice.Id);

            payment.CashEntered = invoiceDetail.CashierPayment?.CashEntered ?? 0m;
            payment.Change = invoiceDetail.CashierPayment?.Change ?? 0m;
            payment.LastModifiedDate = DateTime.UtcNow;
            payment.ModifiedById = currentUserId;
            payment.SessionId = session?.Id;

            await _paymentRepository.UpdateAsync(payment);

            invoice.OriginalPrice = invoiceDetail.OriginalPrice;
            invoice.FinalPrice = invoiceDetail.FinalPrice;
            invoice.Discount = invoiceDetail.Discount;
            invoice.Note = invoiceDetail.Note?.Trim();
            invoice.Points = invoiceNewPoints;
            invoice.PaidWithPoints = invoiceDetail.PaidWithPoints;
            invoice.ModifiedById = currentUserId;
            invoice.LastModifiedDate = DateTime.UtcNow;
            invoice.CustomerId = customer.Id;

            await _invoiceRepository.UpdateAsync(invoice);

            return new EntityApiResponse<InvoiceDetail>(entity: new InvoiceDetail(invoice, payment));
        }

        #endregion
    }
}
