using BlazorCashier.Models.Identity;
using BlazorCashier.Services.Organizations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    public class OrganizationsController : BaseController
    {
        #region Private Members

        private readonly IOrganizationService _orgService;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrganizationsController(
            UserManager<ApplicationUser> userManager,
            IOrganizationService orgService) : base(userManager)
        {
            _orgService = orgService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Retrieves an organization by id
        /// </summary>
        /// <param name="organizationId">Id of the organization to retrieve</param>
        /// <returns></returns>
        [HttpGet("{organizationId}")]
        public async Task<IActionResult> Get(string organizationId)
        {
            var orgResponse = await _orgService.GetOrganizationDetailsAsync(organizationId);

            if (!orgResponse.IsSuccess) return NotFound(orgResponse);

            return Ok(orgResponse);
        }

        #endregion
    }
}
