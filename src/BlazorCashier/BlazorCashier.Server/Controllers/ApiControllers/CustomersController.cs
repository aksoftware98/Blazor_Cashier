using BlazorCashier.Models.Identity;
using BlazorCashier.Services.Customers;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    public class CustomersController : BaseController
    {
        #region Private Members
        
        private readonly ICustomerService _customerService;

        #endregion

        #region Constructors

        public CustomersController(
            UserManager<ApplicationUser> userManager,
            ICustomerService customerService) : base(userManager)
        {
            _customerService = customerService;
        }

        #endregion

        #region Endpoints

        // GET: api/customers
        /// <summary>
        /// Retrieves the customers related to the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(EntitiesApiResponse<CustomerDetail>))]
        public async Task<IActionResult> Get()
        {
            var user = await GetCurrentUser();

            var customersResponse = await _customerService.GetCustomersForOrganizationAsync(user.OrganizationId);

            return Ok(customersResponse);
        }

        // GET: api/customers/fyd78as9
        /// <summary>
        /// Retrieves a customer's details by id
        /// </summary>
        /// <param name="customerId">Customer id to retrieve the details for</param>
        /// <returns></returns>
        [HttpGet("{customerId}")]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<CustomerDetail>))]
        [ProducesResponseType(404, Type = typeof(EntityApiResponse<CustomerDetail>))]
        public async Task<IActionResult> Get(string customerId)
        {
            var customerResponse = await _customerService.GetCustomerDetailsAsync(customerId);

            if (!customerResponse.IsSuccess)
                return NotFound(customerResponse);

            return Ok(customerResponse);
        }

        // POST: api/customers
        /// <summary>
        /// Creates a new customer using the passed details
        /// </summary>
        /// <param name="customerDetail">Customer details to be used</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<CustomerDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<CustomerDetail>))]
        public async Task<IActionResult> Post([FromBody]CustomerDetail customerDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();

            customerDetail.OrganizationId = user.OrganizationId;

            var createResponse = await _customerService.CreateCustomerAsync(customerDetail, user.Id);

            if (!createResponse.IsSuccess)
                return BadRequest(createResponse);

            return Ok(createResponse);
        }

        // PUT: api/customers
        /// <summary>
        /// Updates the customer details using the passed new details
        /// </summary>
        /// <param name="customerDetail">New customer details</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<CustomerDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<CustomerDetail>))]
        public async Task<IActionResult> Put([FromBody]CustomerDetail customerDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();

            customerDetail.OrganizationId = user.OrganizationId;

            var updateResponse = await _customerService.UpdateCustomerAsync(customerDetail, user.Id);

            if (!updateResponse.IsSuccess)
                return BadRequest(updateResponse);

            return Ok(updateResponse);
        }

        // DELETE: api/customersFJD78AH89
        /// <summary>
        /// Deletes a customer by id
        /// </summary>
        /// <param name="customerId">Id of the customer to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{customerId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Delete(string customerId)
        {
            var deleteResponse = await _customerService.DeleteCustomerAsync(customerId);

            if (!deleteResponse.IsSuccess)
                return NotFound(deleteResponse);

            return Ok(deleteResponse);
        }

        #endregion
    }
}
