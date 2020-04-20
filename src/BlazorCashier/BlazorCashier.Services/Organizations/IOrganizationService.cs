using BlazorCashier.Models;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Organizations
{
    public interface IOrganizationService
    {
        /// <summary>
        /// Adds a new organization and a user for the organization
        /// </summary>
        /// <param name="orgDetail">Details to use for adding the organization</param>
        /// <returns>Response containing the organization added and success flag along with an error if there is any</returns>
        Task<EntityApiResponse<Organization>> AddOrganizationAsync(OrganizationDetail orgDetail);

        /// <summary>
        /// Retrieves the organization details
        /// </summary>
        /// <param name="organizationId">Organization id to get the data for</param>
        /// <returns>Response containing the organization details</returns>
        Task<EntityApiResponse<OrganizationDetail>> GetOrganizationDetailsAsync(string organizationId);
    }
}
