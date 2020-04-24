using BlazorCashier.Models;
using BlazorCashier.Models.Identity;
using BlazorCashier.Services.Stocks;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    public class StocksController : BaseController
    {
        #region Private Members

        private readonly IStockService _stockService;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public StocksController(
            UserManager<ApplicationUser> userManager,
            IStockService stockService) : base(userManager)
        {
            _stockService = stockService;
        }

        #endregion

        #region Endpoints

        // GET: api/stocks
        /// <summary>
        /// Retrieves all stocks for the current user
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(EntitiesApiResponse<StockDetail>))]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await GetCurrentUser();

            var stocksResponse = await _stockService.GetStocksForOrganizationAsync(user.OrganizationId);

            return Ok(stocksResponse);
        }

        // GET: api/stocks/g7f8ds78
        /// <summary>
        /// Retrieves a stock's details by id
        /// </summary>
        /// <param name="stockId">Stock id to retrieve the details for</param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<StockDetail>))]
        [ProducesResponseType(404, Type = typeof(EntityApiResponse<StockDetail>))]
        [HttpGet("{stockId}")]
        public async Task<IActionResult> Get(string stockId) 
        {
            var stockResponse = await _stockService.GetStockDetailsAsync(stockId);

            if (!stockResponse.IsSuccess)
                return NotFound(stockResponse);

            return Ok(stockResponse);
        }

        // POST: api/stocks
        /// <summary>
        /// Creates a new stock using the passed stock details
        /// </summary>
        /// <param name="stockDetail">Details to use for creating the stock</param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<StockDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<StockDetail>))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StockDetail stockDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();

            stockDetail.OrganiationId = user.OrganizationId;

            var createResponse = await _stockService.CreateStockAsync(stockDetail, user.Id);

            if (!createResponse.IsSuccess)
                return BadRequest(createResponse);

            return Ok(createResponse);
        }

        // PUT: api/stocks
        /// <summary>
        /// Updates a stock details using the passed new details
        /// </summary>
        /// <param name="stockDetail">New stock details to be saved</param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<StockDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<StockDetail>))]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]StockDetail stockDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();

            stockDetail.OrganiationId = user.OrganizationId;

            var updateResponse = await _stockService.UpdateStockAsync(stockDetail, user.Id);

            if (!updateResponse.IsSuccess)
                return BadRequest(updateResponse);

            return Ok(updateResponse);
        }

        // DELETE: api/stocks
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        [HttpDelete("{stockId}")]
        public async Task<IActionResult> Delete(string stockId) 
        {
            var deleteResponse = await _stockService.DeleteStockAsync(stockId);

            if (!deleteResponse.IsSuccess)
                return NotFound(deleteResponse);

            return Ok(deleteResponse);
        }

        #endregion
    }
}
