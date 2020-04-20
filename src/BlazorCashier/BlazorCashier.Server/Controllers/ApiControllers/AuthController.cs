using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCashier.Server.Services;
using BlazorCashier.Services.Account;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var loginResponse = await _userService.LoginAsync(request);

            if (loginResponse.IsSuccess)
                return Ok(loginResponse);

            return BadRequest(new IdentityApiResponse(loginResponse.Error));
        }
    }
}