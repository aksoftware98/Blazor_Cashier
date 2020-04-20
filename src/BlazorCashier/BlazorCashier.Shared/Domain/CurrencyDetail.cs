namespace BlazorCashier.Shared.Domain
{
    public class CurrencyDetail
    {
        public string Id { get; set; }
        public string Code { get; set; }
        
        public CurrencyDetail(string id, string code)
        {
            Id = id;
            Code = code;
        }

        public CurrencyDetail()
        {

        }
    }
}
