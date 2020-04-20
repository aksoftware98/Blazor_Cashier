﻿using BlazorCashier.Models;

namespace BlazorCashier.Shared.Domain
{
    public class OrganizationDetail
    {
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public string Address { get; set; }
        public CurrencyDetail Currency { get; set; }
        public CountryDetail Country { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FinancialNumber { get; set; }
        public string CountryId { get; set; }
        public string CurrencyId { get; set; }

        public OrganizationDetail()
        {

        }

        public OrganizationDetail(Organization org)
        {
            Name = org.Name;
            OwnerName = org.OwnerName;
            Address = org.Address;
            City = org.City;
            Phone = org.Phone;
            Telephone = org.TelePhone;
            Email = org.Email;
            Website = org.Website;
            FinancialNumber = org.FinancialNumber;
            Country = new CountryDetail(org.Country.Id, org.Country.Code, org.Country.Name);
            Currency = new CurrencyDetail(org.Currency.Id, org.Currency.Code);
        }
    }
}
