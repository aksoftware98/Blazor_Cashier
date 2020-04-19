using BlazorCashier.Models.Data;
using System.Threading.Tasks;
using BlazorCashier.Models;
using Microsoft.AspNetCore.Identity;
using BlazorCashier.Models.Identity;

namespace BlazorCashier.Services.Organizations
{
    public class OrganizationService : IOrganizationService
    {
        #region Private Members

        private readonly IRepository<Organization> _orgRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrganizationService(
            IRepository<Organization> orgRepository,
            UserManager<ApplicationUser> userManager)
            => (_orgRepository, _userManager) = (orgRepository, userManager);

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new organization and a user for the organization
        /// </summary>
        /// <param name="viewModel">View model to use for adding the organization (should be replaced with the actual type once created)</param>
        /// <returns>True if operation succeeded, false otherwise.. will be replaced with a proper communication type once created</returns>
        public async Task<bool> AddOrganizationAsync(object data)
        {
            // For warning removal
            await Task.Delay(100);

            return true;
        }

        #endregion

        #region Helper Methods

        #endregion
    }
}
