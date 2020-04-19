using BlazorCashier.Models;
using BlazorCashier.Services.Responses;
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
        Task<SingleEntityResponse<Organization>> AddOrganizationAsync(OrganizationDetail orgDetail);
    }
}
