using BlazorCashier.Models.Identity;
using BlazorCashier.Services.Account;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    public class AuthController : BaseController
    {
        #region Private Members

        private readonly IUserService _userService;

        #endregion

        #region Constructors

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IUserService userService) : base(userManager)
        {
            _userService = userService;
        }

        #endregion

        #region Endpoints

        [Route("login")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(IdentityApiResponse))]
        [ProducesResponseType(400, Type = typeof(IdentityApiResponse))]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var loginResponse = await _userService.LoginAsync(request);

            if (loginResponse.IsSuccess)
                return Ok(loginResponse);

            return BadRequest(loginResponse);
        }

        //[Route("changepassword")]
        //[HttpPut]
        //[ProducesResponseType(200, Type = typeof(ApiResponse))]
        //[ProducesResponseType(400, Type = typeof(ApiResponse))]
        //public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        //{
        //    if (!ModelState.IsValid) return ModelError();

        //    var response = await _userService.ChangePasswordForUserAsync(request);

        //    if (!response.IsSuccess)
        //        return BadRequest(response);

        //    return Ok(response);
        //}

        #endregion
    }
}