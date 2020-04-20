namespace BlazorCashier.Shared.Domain
{
    public class CountryDetail
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public CountryDetail(string id, string code, string name)
        {
            Id = id;
            Code = code;
            Name = name;
        }

        public CountryDetail()
        {

        }
    }
}
