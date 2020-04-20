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
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var loginResponse = await _userService.LoginAsync(request);

            if (loginResponse.IsSuccess)
                return Ok(loginResponse);

            return BadRequest(loginResponse);
        }

        #endregion
    }
}