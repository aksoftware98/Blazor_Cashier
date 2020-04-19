using System.Threading.Tasks;

namespace BlazorCashier.Services.Organizations
{
    public interface IOrganizationService
    {
        /// <summary>
        /// Adds a new organization and a user for the organization
        /// </summary>
        /// <param name="viewModel">View model to use for adding the organization (should be replaced with the actual type once created)</param>
        /// <returns>True if operation succeeded, false otherwise.. will be replaced with a proper communication type once created</returns>
        Task<bool> AddOrganizationAsync(object viewModel);
    }
}
