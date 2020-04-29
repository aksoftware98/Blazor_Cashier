using BlazorCashier.Models;

namespace BlazorCashier.Shared.Domain
{
    public class CashierPaymentDetail
    {
        public string Id { get; set; }
        public decimal CashEntered { get; set; }
        public decimal Change { get; set; }
        public SessionDetail Session { get; set; }
        public InvoiceDetail Invoice { get; set; }
        public string OrganizationId { get; set; }

        public CashierPaymentDetail()
        {

        }

        public CashierPaymentDetail(CashierPayment payment)
        {
            Id = payment.Id;
            CashEntered = payment.CashEntered;
            Change = payment.Change;
            Session = new SessionDetail { Id = payment.Session.Id };
            Invoice = new InvoiceDetail { Id = payment.Invoice.Id, Number = payment.Invoice.Number };
        }
    }
}
