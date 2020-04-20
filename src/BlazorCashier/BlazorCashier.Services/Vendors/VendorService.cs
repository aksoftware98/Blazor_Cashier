using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Vendors
{
    public class VendorService : IVendorService
    {
        #region Private Members

        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<Organization> _orgRepository;
        private readonly IRepository<Country> _countryRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public VendorService(
            IRepository<Vendor> vendorRepository,
            IRepository<Organization> orgRepository,
            IRepository<Country> countryRepository)
        {
            _vendorRepository = vendorRepository;
            _orgRepository = orgRepository;
            _countryRepository = countryRepository;
        }

        #endregion
        
        #region Public Methods

        public async Task<EntityApiResponse<VendorDetail>> CreateVendorAsync(VendorDetail vendorDetail, string currentUserId)
        {
            if (vendorDetail is null)
                throw new ArgumentNullException(nameof(vendorDetail));

            var country = await _countryRepository.GetByIdAsync(vendorDetail.Country.Id);

            if (country is null)
                return new EntityApiResponse<VendorDetail>(error: "Country does not exist");

            var newVendor = new Vendor
            {
                FirstName = vendorDetail.FirstName,
                LastName = vendorDetail.LastName,
                Email = vendorDetail.Email,
                Address1 = vendorDetail.Address1,
                Address2 = vendorDetail.Address2,
                Note = vendorDetail.Note,
                City = vendorDetail.City,
                CountryId = country.Id,
                CreatedById = currentUserId,
                ModifiedById = currentUserId,
                Website = vendorDetail.Website,
                Telephone = vendorDetail.Telephone,
                Phone = vendorDetail.Phone,
                OrganizationId = vendorDetail.OrganizationId
            };

            await _vendorRepository.InsertAsync(newVendor);

            return new EntityApiResponse<VendorDetail>(entity: new VendorDetail(newVendor));
        }

        public async Task<ApiResponse> DeleteVendor(string vendorId)
        {
            var vendor = await _vendorRepository.GetByIdAsync(vendorId);

            if (vendor is null)
                return new ApiResponse("Vendor does not exist");

            await _vendorRepository.DeleteAsync(vendor);

            return new ApiResponse();
        }

        public async Task<EntityApiResponse<VendorDetail>> GetVendorDetails(string vendorId)
        {
            var vendor = await _vendorRepository.GetByIdAsync(vendorId);

            if (vendor is null)
                return new EntityApiResponse<VendorDetail>(error: "Vendor does not exist");

            return new EntityApiResponse<VendorDetail>(new VendorDetail(vendor));
        }

        public async Task<EntitiesApiResponse<VendorDetail>> GetVendorsForOrganization(string organizationId)
        {
            var org = await _orgRepository.GetByIdAsync(organizationId);

            if (org is null)
                return new EntitiesApiResponse<VendorDetail>(error: "Organization does not exist");

            var vendors = await _vendorRepository.Table.Where
                (v => v.OrganizationId == org.Id).ToListAsync();

            var vendorsDetails = vendors.Select(v => new VendorDetail(v));

            return new EntitiesApiResponse<VendorDetail>(entities: vendorsDetails);
        }

        public async Task<EntityApiResponse<VendorDetail>> UpdateVendorAsync(VendorDetail vendorDetail, string currentUserId)
        {
            if (vendorDetail is null)
                throw new ArgumentNullException(nameof(vendorDetail));

            var vendor = await _vendorRepository.GetByIdAsync(vendorDetail.Id);

            if (vendor is null)
                return new EntityApiResponse<VendorDetail>(error: "Vendor does not exist");

            var country = await _countryRepository.GetByIdAsync(vendorDetail.Country.Id);

            if (country is null)
                return new EntityApiResponse<VendorDetail>(error: "Country does not exist");

            vendor.FirstName = vendorDetail.FirstName.Trim();
            vendor.LastName = vendorDetail.LastName.Trim();
            vendor.Note = vendorDetail.Note.Trim();
            vendor.Phone = vendorDetail.Phone.Trim();
            vendor.Telephone = vendorDetail.Telephone.Trim();
            vendor.Website = vendorDetail.Website.Trim();
            vendor.Address1 = vendorDetail.Address1.Trim();
            vendor.Address2 = vendorDetail.Address2.Trim();
            vendor.City = vendorDetail.City.Trim();
            vendor.Email = vendorDetail.Email.Trim();
            vendor.ModifiedById = currentUserId;
            vendor.LastModifiedDate = DateTime.UtcNow;
            vendor.CountryId = country.Id;

            await _vendorRepository.UpdateAsync(vendor);

            return new EntityApiResponse<VendorDetail>(entity: new VendorDetail(vendor));
        }

        #endregion

        #region Helper Methods

        #endregion
    }
}
