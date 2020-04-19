using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Models.Extensions;
using BlazorCashier.Models.Identity;
using BlazorCashier.Services.Responses;
using BlazorCashier.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Organizations
{
    public class OrganizationService : IOrganizationService
    {
        #region Private Members

        private readonly IRepository<Organization> _orgRepository;
        private readonly IRepository<Country> _countryRespository;
        private readonly IRepository<Currency> _currencyRespository;
        private readonly IWebHostEnvironmentProvider _hostProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrganizationService(
            IRepository<Organization> orgRepository,
            IRepository<Country> countryRepository,
            IRepository<Currency> currencyRepository,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironmentProvider webHostEnvironmentProvider)
            => (_orgRepository,_countryRespository, _currencyRespository, 
                _hostProvider, _userManager)
             = (orgRepository, countryRepository, currencyRepository, 
                webHostEnvironmentProvider, userManager);

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new organization and a user for the organization
        /// </summary>
        /// <param name="orgDetail">Details to use for adding the organization</param>
        /// <returns>Response containing the organization added and success flag along with an error if there is any</returns>
        public async Task<SingleEntityResponse<Organization>> AddOrganizationAsync(OrganizationDetail orgDetail)
        {
            // Check the country
            var country = await _countryRespository.GetByIdAsync(orgDetail.CountryId);

            if (country == null) return Error("Country does not exist");

            // Check the currency
            var currency = await _currencyRespository.GetByIdAsync(orgDetail.CurrencyId);

            if (currency == null) return Error("Currency does not exist");

            var userWithSameEmail = await _userManager.FindByEmailAsync(orgDetail.Email);

            if (userWithSameEmail != null) return Error("Email already taken");

            // Add the organization
            var org = new Organization
            {
                Address = orgDetail.Address,
                Name = orgDetail.FullName,
                Phone = orgDetail.Phone,
                TelePhone = orgDetail.Telephone,
                OwnerName = orgDetail.OwnerName,
                FinancialNumber = orgDetail.FinancialNumber,
                RegistrationDate = DateTime.UtcNow,
                Website = orgDetail.Website,
                Email = orgDetail.Email,
                Country = orgDetail.CountryId,
                City = orgDetail.City,
                CurrencyId = orgDetail.CurrencyId
            };

            await _orgRepository.InsertAsync(org);

            // Add a user with role "Owner"
            var user = new ApplicationUser
            {
                FirstName = orgDetail.FullName,
                LastName = "Admin",
                Email = orgDetail.Email,
                UserName = orgDetail.Email,
                ProfilePicture = $"{_hostProvider.WebRootPath.Replace("\\\\", "/")}/Images/Users/default.png"
            };

            var createUserResult = await _userManager.CreateAsync(user);
            
            if (!createUserResult.Succeeded)
            {
                // Delete the organization
                await _orgRepository.DeleteAsync(org);

                // Return all errors into one error
                return Error(createUserResult.Errors.Select(e => e.Description).AllInOne());
            }

            await _userManager.AddToRoleAsync(user, "Owner");

            return new SingleEntityResponse<Organization>(entity: org);
        }

        #endregion

        #region Helper Methods

        private SingleEntityResponse<Organization> Error(string error)
            => new SingleEntityResponse<Organization>(error: error);

        #endregion
    }
}
