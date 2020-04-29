using BlazorCashier.Models;

namespace BlazorCashier.Shared.Domain
{
    public class InvoiceItemDetail
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
        public float Discount { get; set; }
        public StockDetail Stock { get; set; }


        public InvoiceItemDetail()
        {

        }

        public InvoiceItemDetail(InvoiceItem invoiceItem)
        {
            Id = invoiceItem.Id;
            Description = invoiceItem.Description;
            Quantity = invoiceItem.Quantity;
            Price = invoiceItem.Price;
            FinalPrice = invoiceItem.FinalPrice;
            Discount = invoiceItem.Discount;
            Stock = new StockDetail(invoiceItem.Stock);
        }
    }
}
