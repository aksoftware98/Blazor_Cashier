using BlazorCashier.Models;

namespace BlazorCashier.Shared.Domain
{
    public class BillItemDetail
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public StockDetail Stock { get; set; }
        public BillDetail Bill { get; set; }

        public BillItemDetail()
        {

        }

        public BillItemDetail(BillItem billItem)
        {
            Id = billItem.Id;
            Description = billItem.Description;
            Quantity = billItem.Quantity;
            Price = billItem.Price;
            Stock = new StockDetail(billItem.Stock);
            Bill = new BillDetail
            {
                Id = billItem.Id,
                Number = billItem.Bill.Number
            };
        }
    }
}
