using BlazorCashier.Models;

namespace BlazorCashier.Shared.Domain
{
    public class DiscountItemDetail
    {
        public string Id { get; set; }
        public StockDetail Stock { get; set; }
        public DiscountDetail Discount { get; set; }

        public DiscountItemDetail()
        {

        }

        public DiscountItemDetail(DiscountItem discountItem)
        {
            Id = discountItem.Id;
            Stock = new StockDetail(discountItem.Stock);
            Discount = new DiscountDetail
            {
                Id = discountItem.Discount.Id
            };
        }
    }
}
