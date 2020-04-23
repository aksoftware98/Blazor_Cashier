using BlazorCashier.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCashier.Shared.Domain
{
    public class CustomerDetail
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Barcode { get; set; }
        public int Points { get; set; }
        public CountryDetail Country { get; set; }

        [JsonIgnore]
        public string OrganizationId { get; set; }

        public CustomerDetail()
        {

        }

        public CustomerDetail(Customer customer)
        {
            Id = customer.Id;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Email = customer.Email;
            Phone = customer.Phone;
            Address = customer.Address;
            StreetAddress = customer.StreetAddress;
            City = customer.City;
            Barcode = customer.Barcode;
            Points = customer.Points;
            Country = new CountryDetail(customer.Country);
        }
    }
}
