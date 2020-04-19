namespace BlazorCashier.Shared.Domain
{
    public class OrganizationDetail
    {
        public string FullName { get; set; }
        public string OwnerName { get; set; }
        public string Address { get; set; }
        public CurrencyDetail Currency { get; set; }
        public CountryDetail Country { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Website { get; set; }
        public string ConfirmPassword { get; set; }
        public string FinancialNumber { get; set; }

    }
}
