using BlazorCashier.Services.Organizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/organizations")]
    public class OrganizationsController : ControllerBase
    {
        #region Private Members

        private readonly IOrganizationService _orgService;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrganizationsController(
            IOrganizationService orgService)
        {
            _orgService = orgService;
        }

        #endregion

        #region Endpoints

        [HttpGet("{organizationId}")]
        public async Task<IActionResult> Get(string organizationId)
        {
            var orgResponse = await _orgService.GetOrganizationDetailsAsync(organizationId);

            return orgResponse.IsSuccess switch
            {
                false => NotFound(orgResponse.Error),
                true => Ok(orgResponse.Entity)
            };
        }

        #endregion
    }
}
