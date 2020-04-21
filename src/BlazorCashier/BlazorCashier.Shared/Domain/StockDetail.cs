using BlazorCashier.Models;

namespace BlazorCashier.Shared.Domain
{
    public class StockDetail
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SellingPrice { get; set; }
        public int Points { get; set; }
        public ItemDetail Item { get; set; }
        public string OrganiationId { get; set; }

        public StockDetail()
        {

        }

        public StockDetail(Stock stock)
        {
            Id = stock.Id;
            Quantity = stock.Quantity;
            Price = stock.Price;
            SellingPrice = stock.SellingPrice;
            Points = stock.Points;
            Item = new ItemDetail(stock.Item);
        }
    }
}
