using BlazorCashier.Models;

namespace BlazorCashier.Shared.Domain
{
    public class CurrencyDetail
    {
        public string Id { get; set; }
        public string Code { get; set; }
        
        public CurrencyDetail(Currency currency)
        {
            Id = currency.Id;
            Code = currency.Code;
        }

        public CurrencyDetail()
        {

        }
    }
}
