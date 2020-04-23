using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        #region Private Members

        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Organization> _orgRepository;
        private readonly IRepository<Country> _countryRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerService(
            IRepository<Customer> customerRepository,
            IRepository<Organization> orgRepository,
            IRepository<Country> countryRepository)
        {
            _customerRepository = customerRepository;
            _orgRepository = orgRepository;
            _countryRepository = countryRepository;
        }

        #endregion

        #region Public Methods

        public async Task<EntityApiResponse<CustomerDetail>> CreateCustomerAsync(CustomerDetail customerDetail, string currentUserId)
        {
            if (customerDetail is null)
                throw new ArgumentNullException(nameof(customerDetail));

            var org = await _orgRepository.GetByIdAsync(customerDetail.OrganizationId);

            if (org is null)
                return new EntityApiResponse<CustomerDetail>(error: "Organization does not exist");

            var country = await _countryRepository.GetByIdAsync(customerDetail.Country?.Id);

            if (country is null)
                return new EntityApiResponse<CustomerDetail>(error: "Country does not exist");

            var customerWithSameInfo = await _customerRepository.TableNoTracking
                .FirstOrDefaultAsync(c => c.FirstName == customerDetail.FirstName && c.LastName == customerDetail.LastName);

            if (!(customerWithSameInfo is null))
                return new EntityApiResponse<CustomerDetail>(error: "A customer already exist with the same name");

            var customer = new Customer
            {
                FirstName = customerDetail.FirstName?.Trim(),
                LastName = customerDetail.LastName?.Trim(),
                Email = customerDetail.Email?.Trim(),
                Phone = customerDetail.Phone?.Trim(),
                Address = customerDetail.Address?.Trim(),
                StreetAddress = customerDetail.StreetAddress?.Trim(),
                City = customerDetail.City?.Trim(),
                Barcode = customerDetail.Barcode?.Trim(),
                CountryId = country.Id,
                CreatedById = currentUserId,
                ModifiedById = currentUserId,
                OrganizationId = org.Id
            };

            await _customerRepository.InsertAsync(customer);

            return new EntityApiResponse<CustomerDetail>(entity: new CustomerDetail(customer));
        }

        public async Task<ApiResponse> DeleteCustomerAsync(string customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer is null)
                return new ApiResponse("Customer does not exist");

            await _customerRepository.DeleteAsync(customer);

            return new ApiResponse();
        }

        public async Task<EntityApiResponse<CustomerDetail>> AlterPointsOfCustomerAsync(string customerId, int points)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer is null)
                return new EntityApiResponse<CustomerDetail>(error: "Customer does not exist");

            if (points == default)
                return new EntityApiResponse<CustomerDetail>(entity: new CustomerDetail(customer));

            customer.Points += points;
            await _customerRepository.UpdateAsync(customer);

            return new EntityApiResponse<CustomerDetail>(entity: new CustomerDetail(customer));

        }

        public async Task<EntityApiResponse<CustomerDetail>> GetCustomerDetailsAsync(string customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer is null)
                return new EntityApiResponse<CustomerDetail>(error: "Customer does not exist");

            return new EntityApiResponse<CustomerDetail>(entity: new CustomerDetail(customer));
        }

        public async Task<EntitiesApiResponse<CustomerDetail>> GetCustomersForOrganizationAsync(string organizationId)
        {
            var org = await _orgRepository.GetByIdAsync(organizationId);

            if (org is null)
                return new EntitiesApiResponse<CustomerDetail>(error: "Organization does not exist");

            var customers = await _customerRepository.Table.
                Where(c => c.OrganizationId == org.Id).ToListAsync();

            var customerDetails = customers?.Select(c => new CustomerDetail(c));

            return new EntitiesApiResponse<CustomerDetail>(entities: customerDetails ?? new List<CustomerDetail>());
        }

        public async Task<EntityApiResponse<CustomerDetail>> UpdateCustomerAsync(CustomerDetail customerDetail, string currentUserId)
        {
            if (customerDetail is null)
                throw new ArgumentNullException(nameof(customerDetail));

            var customer = await _customerRepository.GetByIdAsync(customerDetail.Id);

            if (customer is null)
                return new EntityApiResponse<CustomerDetail>(error: "Customer does not exist");

            var org = await _orgRepository.GetByIdAsync(customerDetail.OrganizationId);

            if (org is null)
                return new EntityApiResponse<CustomerDetail>(error: "Organization does not exist");

            var country = await _countryRepository.GetByIdAsync(customerDetail.Country?.Id);

            if (country is null)
                return new EntityApiResponse<CustomerDetail>(error: "Country does not exist");

            var customerWithSameInfo = await _customerRepository.TableNoTracking
                .FirstOrDefaultAsync(c => c.FirstName == customerDetail.FirstName && c.LastName == customerDetail.LastName && c.Id != customerDetail.Id);

            if (!(customerWithSameInfo is null))
                return new EntityApiResponse<CustomerDetail>(error: "A customer already exist with the same name");

            customer.FirstName = customerDetail.FirstName?.Trim();
            customer.LastName = customerDetail.LastName?.Trim();
            customer.Email = customerDetail.Email?.Trim();
            customer.Phone = customerDetail.Phone?.Trim();
            customer.Address = customerDetail.Address?.Trim();
            customer.StreetAddress = customerDetail.StreetAddress?.Trim();
            customer.City = customerDetail.City?.Trim();
            customer.Barcode = customerDetail.Barcode?.Trim();
            customer.CountryId = country.Id;
            customer.LastModifiedDate = DateTime.UtcNow;
            customer.ModifiedById = currentUserId;

            await _customerRepository.UpdateAsync(customer);

            return new EntityApiResponse<CustomerDetail>(entity: new CustomerDetail(customer));
        }

        #endregion
    }
}
