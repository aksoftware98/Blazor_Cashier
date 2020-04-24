using BlazorCashier.Models.Identity;
using BlazorCashier.Services.Discounts;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    public class DiscountsController : BaseController
    {
        #region Private Members

        private readonly IDiscountService _discountService;

        #endregion

        #region Constructors

        public DiscountsController(
            UserManager<ApplicationUser> userManager,
            IDiscountService discountService) : base(userManager)
        {
            _discountService = discountService;
        }

        #endregion

        #region Endpoints

        // GET: api/discounts
        /// <summary>
        /// Retrieves the discounts related to the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(EntitiesApiResponse<DiscountDetail>))]
        public async Task<IActionResult> Get()
        {
            var user = await GetCurrentUser();

            var discountsResponse = await _discountService.GetDiscountsForOrganizationAsync(user.OrganizationId);

            return Ok(discountsResponse);
        }

        // GET: api/discounts/fyd78as9
        /// <summary>
        /// Retrieves a discount's details by id
        /// </summary>
        /// <param name="discountId">Discount id to retrieve the details for</param>
        /// <returns></returns>
        [HttpGet("{discountId}")]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<DiscountDetail>))]
        [ProducesResponseType(404, Type = typeof(EntityApiResponse<DiscountDetail>))]
        public async Task<IActionResult> Get(string discountId)
        {
            var discountResponse = await _discountService.GetDiscountDetailsAsync(discountId);

            if (!discountResponse.IsSuccess)
                return NotFound(discountResponse);

            return Ok(discountResponse);
        }

        // POST: api/discounts
        /// <summary>
        /// Creates a new discount using the passed details
        /// </summary>
        /// <param name="discountDetail">Discount details to be used</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<DiscountDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<DiscountDetail>))]
        public async Task<IActionResult> Post([FromBody]DiscountDetail discountDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();

            discountDetail.OrganizationId = user.OrganizationId;

            var createResponse = await _discountService.CreateDiscountAsync(discountDetail, user.Id);

            if (!createResponse.IsSuccess)
                return BadRequest(createResponse);

            return Ok(createResponse);
        }

        // PUT: api/discounts
        /// <summary>
        /// Updates the discount details using the passed new details
        /// </summary>
        /// <param name="discountDetail">New discount details</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<DiscountDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<DiscountDetail>))]
        public async Task<IActionResult> Put([FromBody]DiscountDetail discountDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();

            discountDetail.OrganizationId = user.OrganizationId;

            var updateResponse = await _discountService.UpdateDiscountAsync(discountDetail, user.Id);

            if (!updateResponse.IsSuccess)
                return BadRequest(updateResponse);

            return Ok(updateResponse);
        }

        // DELETE: api/discountsFJD78AH89
        /// <summary>
        /// Deletes a discount by id
        /// </summary>
        /// <param name="discountId">Id of the discount to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{discountId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Delete(string discountId)
        {
            var deleteResponse = await _discountService.DeleteDiscountAsync(discountId);

            if (!deleteResponse.IsSuccess)
                return NotFound(deleteResponse);

            return Ok(deleteResponse);
        }

        #endregion
    }
}
