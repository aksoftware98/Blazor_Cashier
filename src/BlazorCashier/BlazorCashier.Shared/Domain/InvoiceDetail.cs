using BlazorCashier.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlazorCashier.Shared.Domain
{
    public class InvoiceDetail
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public float Discount { get; set; }
        public int PaidWithPoints { get; set; }
        public int Points { get; set; }
        public string Note { get; set; }
        public CustomerDetail Customer { get; set; }
        public ICollection<InvoiceItemDetail> InvoiceItems { get; set; }
        public CashierPaymentDetail CashierPayment { get; set; }
        public string OrganizationId { get; set; }

        public InvoiceDetail()
        {

        }

        public InvoiceDetail(Invoice invoice, CashierPayment payment = null)
        {
            Id = invoice.Id;
            Number = invoice.Number;
            OriginalPrice = invoice.OriginalPrice;
            FinalPrice = invoice.FinalPrice;
            Discount = invoice.Discount;
            PaidWithPoints = invoice.PaidWithPoints;
            Points = invoice.Points;
            Note = invoice.Note;
            Customer = new CustomerDetail(invoice.Customer);
            InvoiceItems = new List<InvoiceItemDetail>
                (invoice.InvoiceItems.Select(i => new InvoiceItemDetail(i)));
            if (payment != null) CashierPayment = new CashierPaymentDetail(payment);
        }
    }
}
