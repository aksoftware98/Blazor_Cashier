using BlazorCashier.Models.Identity;
using BlazorCashier.Services.Invoices;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    public class InvoicesController : BaseController
    {
        #region Private Members
        
        private readonly IInvoiceService _invoiceService;

        #endregion

        #region Constructors

        public InvoicesController(
            UserManager<ApplicationUser> userManager,
            IInvoiceService invoiceService) : base(userManager)
        {
            _invoiceService = invoiceService;
        }

        #endregion

        #region Endpoints

        // GET: api/invoices
        /// <summary>
        /// Retrieves the invoices related to the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(EntitiesApiResponse<InvoiceDetail>))]
        public async Task<IActionResult> Get()
        {
            var user = await GetCurrentUser();

            var invoicesResponse = await _invoiceService.GetInvoicesForOrganizationAsync(user.OrganizationId);

            return Ok(invoicesResponse);
        }

        // GET: api/invoices/fyd78as9
        /// <summary>
        /// Retrieves an invoice's details by id
        /// </summary>
        /// <param name="invoiceId">Invoice id to retrieve the details for</param>
        /// <returns></returns>
        [HttpGet("{invoiceId}")]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<InvoiceDetail>))]
        [ProducesResponseType(404, Type = typeof(EntityApiResponse<InvoiceDetail>))]
        public async Task<IActionResult> Get(string invoiceId)
        {
            var invoiceResponse = await _invoiceService.GetInvoiceDetailsAsync(invoiceId);

            if (!invoiceResponse.IsSuccess)
                return NotFound(invoiceResponse);

            return Ok(invoiceResponse);
        }

        // POST: api/invoices
        /// <summary>
        /// Creates a new invoice using the passed details
        /// </summary>
        /// <param name="invoiceDetail">Invoice details to be used</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<InvoiceDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<InvoiceDetail>))]
        public async Task<IActionResult> Post([FromBody]InvoiceDetail invoiceDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();

            invoiceDetail.OrganizationId = user.OrganizationId;

            var createResponse = await _invoiceService.AddInvoiceAsync(invoiceDetail, user.Id);

            if (!createResponse.IsSuccess)
                return BadRequest(createResponse);

            return Ok(createResponse);
        }

        // PUT: api/invoices
        /// <summary>
        /// Updates the invoice details using the passed new details
        /// </summary>
        /// <param name="invoiceDetail">New invoice details</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<InvoiceDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<InvoiceDetail>))]
        public async Task<IActionResult> Put([FromBody]InvoiceDetail invoiceDetail)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();

            invoiceDetail.OrganizationId = user.OrganizationId;

            var updateResponse = await _invoiceService.UpdateInvoiceAsync(invoiceDetail, user.Id);

            if (!updateResponse.IsSuccess)
                return BadRequest(updateResponse);

            return Ok(updateResponse);
        }

        // DELETE: api/invoicesFJD78AH89
        /// <summary>
        /// Deletes a invoice by id
        /// </summary>
        /// <param name="invoiceId">Id of the invoice to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{invoiceId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Delete(string invoiceId)
        {
            var user = await GetCurrentUser();

            var deleteResponse = await _invoiceService.DeleteInvoiceAsync(invoiceId, user.Id);

            if (!deleteResponse.IsSuccess)
                return NotFound(deleteResponse);

            return Ok(deleteResponse);
        }

        #endregion
    }
}
