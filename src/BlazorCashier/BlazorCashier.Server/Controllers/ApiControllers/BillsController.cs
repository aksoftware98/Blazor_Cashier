using BlazorCashier.Models.Identity;
using BlazorCashier.Services.Bills;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    public class BillsController : BaseController
    {
        #region Private Members

        private readonly IBillService _billService;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public BillsController(
            UserManager<ApplicationUser> userManager,
            IBillService billService) : base(userManager)
        {
            _billService = billService;
        }

        #endregion

        #region Endpoints

        // GET: api/bills
        /// <summary>
        /// Retrieves the bills related to the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(EntitiesApiResponse<BillDetail>))]
        public async Task<IActionResult> Get()
        {
            var user = await GetCurrentUser();

            var billsResponse = await _billService.GetBillsForOrganizationAsync(user.OrganizationId);

            return Ok(billsResponse);
        }

        // GET: api/bills/fyd78as9
        /// <summary>
        /// Retrieves a bill's details by id
        /// </summary>
        /// <param name="billId">Bill id to retrieve the details for</param>
        /// <returns></returns>
        [HttpGet("{billId}")]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<BillDetail>))]
        [ProducesResponseType(404, Type = typeof(EntityApiResponse<BillDetail>))]
        public async Task<IActionResult> Get(string billId)
        {
            var billResponse = await _billService.GetBillDetailsAsync(billId);

            if (!billResponse.IsSuccess)
                return NotFound(billResponse);

            return Ok(billResponse);
        }

        // POST: api/bills
        /// <summary>
        /// Creates a new bill with the items sent with it for the current user
        /// </summary>
        /// <param name="billDetail">Bill details containing the items of the bill</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<BillDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<BillDetail>))]
        public async Task<IActionResult> Post([FromBody]BillDetail billDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();

            billDetail.organizationId = user.OrganizationId;

            var createResponse = await _billService.CreateBillAsync(billDetail, user.Id);

            if (!createResponse.IsSuccess)
                return BadRequest(createResponse);

            return Ok(createResponse);
        }

        // PUT: api/bills
        /// <summary>
        /// Updates the bill details using the passed new details
        /// </summary>
        /// <param name="billDetail">New bill details</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<BillDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<BillDetail>))]
        public async Task<IActionResult> Put([FromBody]BillDetail billDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();

            billDetail.organizationId = user.OrganizationId;

            var updateResponse = await _billService.UpdateBillAsync(billDetail, user.Id);

            if (!updateResponse.IsSuccess)
                return BadRequest(updateResponse);

            return Ok(updateResponse);
        }

        // DELETE: api/billsFJD78AH89
        /// <summary>
        /// Deletes a bill by id
        /// </summary>
        /// <param name="billId">Bill id to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{billId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Delete(string billId)
        {
            var deleteResponse = await _billService.DeleteBillAsync(billId);

            if (!deleteResponse.IsSuccess)
                return NotFound(deleteResponse);

            return Ok(deleteResponse);
        }

        #endregion
    }
}
