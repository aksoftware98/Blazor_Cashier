using BlazorCashier.Models.Identity;
using BlazorCashier.Services.Items;
using BlazorCashier.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    public class ItemsController : BaseController
    {
        #region Private Members

        private readonly IItemService _itemService;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ItemsController(
            UserManager<ApplicationUser> userManager,
            IItemService itemService) : base(userManager)
        {
            _itemService = itemService;
        }

        #endregion

        #region Endpoints

        // GET: api/items
        /// <summary>
        /// Retrieves all items for the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = await GetCurrentUser();

            var itemsResponse = await _itemService.GetItemsForOrganization(currentUser.OrganizationId);

            return Ok(itemsResponse);
        }

        // GET: api/items?searchText=someText&pageNumber=5&pageSize=20
        //public async Task<IActionResult> Get(string searchText, int pageNumber = 0, int pageSize = 10)
        //{
        //    if (string.IsNullOrWhiteSpace(searchText))
        //        searchText = string.Empty;
        //    var user = await GetCurrentUser();

        //    var searchResponse = await _itemService.SearchForItemsByTextAsync(searchText, user.OrganizationId, pageNumber, pageSize);

        //    if (!searchResponse.IsSuccess)
        //        return NotFound(searchResponse);

        //    return Ok(searchResponse);
        //}

        // GET: api/items/fu8d9asu89

        /// <summary>
        /// Retrieves a item's details by id
        /// </summary>
        /// <param name="itemId">item's id</param>
        /// <returns></returns>
        [HttpGet("{itemId}")]
        public async Task<IActionResult> Get(string itemId)
        {
            var itemResponse = await _itemService.GetItemDetails(itemId);

            if (!itemResponse.IsSuccess)
                return NotFound(itemResponse);

            return Ok(itemResponse);
        }

        // POST: api/items
        /// <summary>
        /// Creates a new item
        /// </summary>
        /// <param name="itemDetail">Details to create the item with</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(ItemDetail itemDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();
            itemDetail.OrganizationId = user.OrganizationId;

            var createResponse = await _itemService.CreateItemAsync(itemDetail, user.Id);

            if (!createResponse.IsSuccess)
                return BadRequest(createResponse);

            return Ok(createResponse);
        }

        // PUT: api/items
        /// <summary>
        /// Updates a item details
        /// </summary>
        /// <param name="itemDetail">Details to update the item with</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(ItemDetail itemDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();
            itemDetail.OrganizationId = user.OrganizationId;

            var updateResponse = await _itemService.UpdateItemAsync(itemDetail, user.Id);

            if (!updateResponse.IsSuccess)
                return BadRequest(updateResponse);

            return Ok(updateResponse);
        }

        // DELETE: api/items/fu80jasu80
        /// <summary>
        /// Delete a specific item
        /// </summary>
        /// <param name="itemId">Id of the item to delete</param>
        /// <returns></returns>
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> Delete(string itemId)
        {
            var deleteResponse = await _itemService.DeleteItem(itemId);

            if (!deleteResponse.IsSuccess)
                return NotFound(deleteResponse);

            return Ok(deleteResponse);
        }

        #endregion
    }
}
