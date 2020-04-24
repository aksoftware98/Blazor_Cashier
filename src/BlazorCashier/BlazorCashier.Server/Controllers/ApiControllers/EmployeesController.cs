using BlazorCashier.Models.Identity;
using BlazorCashier.Services.Employees;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    public class EmployeesController : BaseController
    {
        #region Private Members

        private readonly IEmployeeService _employeeService;

        #endregion

        #region Constructors

        public EmployeesController(
            UserManager<ApplicationUser> userManager,
            IEmployeeService employeeService) : base(userManager)
        {
            _employeeService = employeeService;
        }

        #endregion

        #region Endpoints

        // GET: api/employees
        /// <summary>
        /// Retrieves the employees related to the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(EntitiesApiResponse<EmployeeDetail>))]
        public async Task<IActionResult> Get()
        {
            var user = await GetCurrentUser();

            var employeesResponse = await _employeeService.GetEmployeesForOrganizationAsync(user.OrganizationId);

            return Ok(employeesResponse);
        }

        // GET: api/employees/fyd78as9
        /// <summary>
        /// Retrieves a employee's details by id
        /// </summary>
        /// <param name="employeeId">Employee id to retrieve the details for</param>
        /// <returns></returns>
        [HttpGet("{employeeId}")]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<EmployeeDetail>))]
        [ProducesResponseType(404, Type = typeof(EntityApiResponse<EmployeeDetail>))]
        public async Task<IActionResult> Get(string employeeId)
        {
            var employeeResponse = await _employeeService.GetEmployeeDetailsAsync(employeeId);

            if (!employeeResponse.IsSuccess)
                return NotFound(employeeResponse);

            return Ok(employeeResponse);
        }

        // POST: api/employees
        /// <summary>
        /// Creates a new employee using the passed details
        /// </summary>
        /// <param name="employeeDetail">Employee details to be used</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<EmployeeDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<EmployeeDetail>))]
        public async Task<IActionResult> Post([FromForm]EmployeeDetail employeeDetail, IFormFile file)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();

            employeeDetail.OrganizationId = user.OrganizationId;
            employeeDetail.ProfileImage = file;

            var createResponse = await _employeeService.CreateEmployeeAsync(employeeDetail, user.Id);

            if (!createResponse.IsSuccess)
                return BadRequest(createResponse);

            return Ok(createResponse);
        }

        // PUT: api/employees
        /// <summary>
        /// Updates the employee details using the passed new details
        /// </summary>
        /// <param name="employeeDetail">New employee details</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(EntityApiResponse<EmployeeDetail>))]
        [ProducesResponseType(400, Type = typeof(EntityApiResponse<EmployeeDetail>))]
        public async Task<IActionResult> Put([FromForm]EmployeeDetail employeeDetail, IFormFile file)
        {
            if (!ModelState.IsValid) return ModelError();

            var user = await GetCurrentUser();

            employeeDetail.OrganizationId = user.OrganizationId;
            employeeDetail.ProfileImage = file;

            var updateResponse = await _employeeService.UpdateEmployeeAsync(employeeDetail, user.Id);

            if (!updateResponse.IsSuccess)
                return BadRequest(updateResponse);

            return Ok(updateResponse);
        }

        // DELETE: api/employeesFJD78AH89
        /// <summary>
        /// Deletes a employee by id
        /// </summary>
        /// <param name="employeeId">Id of the employee to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{employeeId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(404, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Delete(string employeeId)
        {
            var deleteResponse = await _employeeService.DeleteEmployeeAsync(employeeId);

            if (!deleteResponse.IsSuccess)
                return NotFound(deleteResponse);

            return Ok(deleteResponse);
        }

        #endregion
    }
}
