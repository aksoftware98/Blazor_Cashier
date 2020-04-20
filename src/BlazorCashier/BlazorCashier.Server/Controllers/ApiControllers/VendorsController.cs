using BlazorCashier.Models.Identity;
using BlazorCashier.Services.Vendors;
using BlazorCashier.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    public class VendorsController : BaseController
    {
        #region Private Members

        private readonly IVendorService _vendorService;

        #endregion

        #region Constructors 

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="userManager">Manager for the user operations</param>
        public VendorsController(
            UserManager<ApplicationUser> userManager,
            IVendorService vendorService)
                : base(userManager)
        {
            _vendorService = vendorService;
        }

        #endregion

        #region Endpoints

        // GET: api/vendors
        /// <summary>
        /// Retrieves all vendors for the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = await GetCurrentUser();

            var vendorsResponse = await _vendorService.GetVendorsForOrganization(currentUser.OrganizationId);

            return Ok(vendorsResponse);
        }

        // GET: api/vendors/fu8d9asu89
        /// <summary>
        /// Retrieves a vendor's details by id
        /// </summary>
        /// <param name="vendorId">Vendor's id</param>
        /// <returns></returns>
        [HttpGet("{vendorId}")]
        public async Task<IActionResult> Get(string vendorId)
        {
            var vendorResponse = await _vendorService.GetVendorDetails(vendorId);

            if (!vendorResponse.IsSuccess)
                return NotFound(vendorResponse);

            return Ok(vendorResponse);
        }

        // POST: api/vendors
        /// <summary>
        /// Creates a new vendor
        /// </summary>
        /// <param name="vendorDetail">Details to create the vendor with</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(VendorDetail vendorDetail)
        {
            if (!ModelState.IsValid) return BadRequest("Model has some errors");

            var user = await GetCurrentUser();
            vendorDetail.OrganizationId = user.OrganizationId;

            var createResponse = await _vendorService.CreateVendorAsync(vendorDetail, user.Id);

            if (!createResponse.IsSuccess)
                return BadRequest(createResponse);

            return Ok(createResponse);
        }

        // PUT: api/vendors
        /// <summary>
        /// Updates a vendor details
        /// </summary>
        /// <param name="vendorDetail">Details to update the vendor with</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(VendorDetail vendorDetail)
        {
            if (!ModelState.IsValid) return BadRequest("Model has some errors");

            var user = await GetCurrentUser();

            vendorDetail.OrganizationId = user.OrganizationId;

            var updateResponse = await _vendorService.UpdateVendorAsync(vendorDetail, user.Id);

            if (!updateResponse.IsSuccess)
                return BadRequest(updateResponse);

            return Ok(updateResponse);
        }

        // DELETE: api/vendors/fu80jasu80
        /// <summary>
        /// Delete a specific vendor
        /// </summary>
        /// <param name="vendorId">Id of the vendor to delete</param>
        /// <returns></returns>
        [HttpDelete("{vendorId}")]
        public async Task<IActionResult> Delete(string vendorId)
        {
            var deleteResponse = await _vendorService.DeleteVendor(vendorId);

            if (!deleteResponse.IsSuccess)
                return NotFound(deleteResponse);

            return Ok(deleteResponse);
        }

        #endregion
    }
}
