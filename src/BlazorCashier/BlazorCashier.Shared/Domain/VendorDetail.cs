using BlazorCashier.Models;

namespace BlazorCashier.Shared.Domain
{
    public class VendorDetail
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string City { get; set; }
        public CountryDetail Country { get; set; }
        public string Note { get; set; }
        public string OrganizationId { get; set; }

        public VendorDetail()
        {

        }

        public VendorDetail(Vendor vendor)
        {
            Id = vendor.Id;
            FirstName = vendor.FirstName;
            LastName = vendor.LastName;
            Address1 = vendor.Address1;
            Address2 = vendor.Address2;
            Phone = vendor.Phone;
            Telephone = vendor.Telephone;
            Email = vendor.Email;
            Website = vendor.Website;
            City = vendor.City;
            Note = vendor.Note;
            Country = new CountryDetail { Id = vendor.Country.Id, Name = vendor.Country.Name, Code = vendor .Country.Code};
        }
    }
}
