using BlazorCashier.Models;

namespace BlazorCashier.Shared.Domain
{
    public class CountryDetail
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public CountryDetail(Country country)
        {
            Id = country.Id;
            Code = country.Code;
            Name = country.Name;
        }

        public CountryDetail()
        {

        }
    }
}
