using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Bills
{
    public class BillService : IBillService
    {
        #region Private Members

        private readonly IRepository<Bill> _billRepository;
        private readonly IRepository<BillItem> _billItemRepository;
        private readonly IRepository<Organization> _orgRepository;
        private readonly IRepository<Stock> _stockRepository;
        private readonly IRepository<Vendor> _vendorRepository;

        #endregion

        #region Constructors

        public BillService(
            IRepository<Bill> billRepository,
            IRepository<BillItem> billItemRepository,
            IRepository<Organization> orgRepository,
            IRepository<Stock> stockRepository,
            IRepository<Vendor> vendorRepository)
        {
            _billRepository = billRepository;
            _billItemRepository = billItemRepository;
            _orgRepository = orgRepository;
            _stockRepository = stockRepository;
            _vendorRepository = vendorRepository;
        }

        #endregion

        #region Public Methods

        public async Task<EntityApiResponse<BillDetail>> CreateBillAsync(BillDetail billDetail, string currentUseId)
        {
            if (billDetail is null)
                throw new ArgumentNullException(nameof(billDetail));

            if (billDetail.BillItems is null || billDetail.BillItems.Count < 1)
            {
                return new EntityApiResponse<BillDetail>(error: "Bill has no items");
            }

            if (billDetail.Vendor is null)
                return new EntityApiResponse<BillDetail>(error: "A vendor is required");

            var org = await _orgRepository.GetByIdAsync(billDetail.organizationId);

            if (org is null)
                return new EntityApiResponse<BillDetail>(error: "Organization does not exist");

            var vendor = await _vendorRepository.GetByIdAsync(billDetail.Vendor.Id);

            if (vendor is null)
                return new EntityApiResponse<BillDetail>(error: "Vendor does not exist");

            var newBillNumber = org.LastBillNumber + 1;

            var bill = new Bill
            {
                Total = billDetail.BillItems.Sum(i => i.Price * i.Quantity),
                Number = newBillNumber,
                Note = billDetail.Note?.Trim(),
                CreatedById = currentUseId,
                ModifiedById = currentUseId,
                VendorId = vendor.Id,
                OrganizationId = org.Id
            };

            await _billRepository.InsertAsync(bill);

            foreach (var item in billDetail.BillItems)
            {
                if (item.Stock is null || item.Quantity < 1)
                    continue;

                var stock = await _stockRepository.GetByIdAsync(item.Stock.Id);

                if (stock is null)
                    continue;

                var billItem = new BillItem
                {
                    Description = item.Description?.Trim(),
                    Quantity = item.Quantity,
                    Price = item.Price,
                    StockId = stock.Id,
                    OrganizationId = org.Id,
                    BillId = bill.Id,
                    CreatedById = currentUseId,
                    ModifiedById = currentUseId
                };

                await _billItemRepository.InsertAsync(billItem);

                stock.Quantity -= billItem.Quantity;

                await _stockRepository.UpdateAsync(stock);
            }


            org.LastBillNumber += 1;

            await _orgRepository.UpdateAsync(org);

            return new EntityApiResponse<BillDetail>(entity: new BillDetail(bill));

        }

        public async Task<ApiResponse> DeleteBillAsync(string billId)
        {
            var bill = await _billRepository.GetByIdAsync(billId);

            if (bill is null)
                return new ApiResponse("Bill does not exist");

            // Add the stock quantity used back to the stock
            foreach (var billItem in bill.BillItems)
            {
                var stock = await _stockRepository.GetByIdAsync(billItem.Stock.Id);

                if (stock is null)
                    continue;

                stock.Quantity += billItem.Quantity;

                await _stockRepository.UpdateAsync(stock);
            }

            await _billItemRepository.DeleteAsync(bill.BillItems);
            await _billRepository.DeleteAsync(bill);

            return new ApiResponse();
        }

        public async Task<EntityApiResponse<BillDetail>> GetBillDetailsAsync(string billId)
        {
            var bill = await _billRepository.GetByIdAsync(billId);

            if (bill is null)
                return new EntityApiResponse<BillDetail>(error: "Bill does not exist");

            return new EntityApiResponse<BillDetail>(entity: new BillDetail(bill));
        }

        public async Task<EntitiesApiResponse<BillDetail>> GetBillsForOrganizationAsync(string organizationId)
        {
            var org = await _orgRepository.GetByIdAsync(organizationId);

            if (org is null)
                return new EntitiesApiResponse<BillDetail>(error: "Organization does not exist");

            var bills = await _billRepository.Table
                .Where(b => b.OrganizationId == org.Id).ToListAsync();

            var billsDetails = bills.Select(b => new BillDetail(b));

            return new EntitiesApiResponse<BillDetail>(entities: billsDetails);
        }

        public async Task<EntityApiResponse<BillDetail>> UpdateBillAsync(BillDetail billDetail, string currentUseId)
        {
            if (billDetail is null)
                throw new ArgumentNullException(nameof(billDetail));

            var bill = await _billRepository.GetByIdAsync(billDetail.Id);

            if (bill == null)
                return new EntityApiResponse<BillDetail>(error: "Bill does not exist");

            if (billDetail.BillItems is null || billDetail.BillItems.Count < 1)
                return new EntityApiResponse<BillDetail>(error: "Bill has not items");

            if (billDetail.Vendor is null)
                return new EntityApiResponse<BillDetail>(error: "A vendor is required");

            var vendor = await _vendorRepository.GetByIdAsync(billDetail.Vendor.Id);

            if (vendor is null)
                return new EntityApiResponse<BillDetail>(error: "Vendor does not exist");

            // Add the quantities of the current bill items to the related stocks

            foreach (var item in bill.BillItems)
            {
                var stock = await _stockRepository.GetByIdAsync(item.Stock.Id);

                stock.Quantity += item.Quantity;

                await _stockRepository.UpdateAsync(stock);
            }

            // Delete all current billItems
            await _billItemRepository.DeleteAsync(bill.BillItems);
            
            var totalPrice = 0m;

            // Re-add new items to the bill
            foreach (var item in billDetail.BillItems)
            {
                if (item.Stock is null || item.Quantity < 1)
                    continue;

                var stock = await _stockRepository.GetByIdAsync(item.Stock.Id);

                if (stock is null)
                    continue;

                var billItem = new BillItem
                {
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Description = item.Description?.Trim(),
                    StockId = stock.Id,
                    CreatedById = currentUseId,
                    ModifiedById = currentUseId,
                    BillId = bill.Id
                };

                await _billItemRepository.InsertAsync(billItem);

                stock.Quantity -= billItem.Quantity;

                await _stockRepository.UpdateAsync(stock);

                totalPrice += billItem.Price * billItem.Quantity;
            }

            bill.LastModifiedDate = DateTime.UtcNow;
            bill.ModifiedById = currentUseId;
            bill.Total = totalPrice;
            bill.Note = billDetail.Note?.Trim();
            bill.VendorId = vendor.Id;
            bill.Total = totalPrice;

            await _billRepository.UpdateAsync(bill);

            return new EntityApiResponse<BillDetail>(entity: new BillDetail(bill));
        }

        #endregion
    }
}
