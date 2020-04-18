namespace BlazorCashier.Models
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Flag { get; set; }
        public string CultureCode { get; set; }
    }
}
