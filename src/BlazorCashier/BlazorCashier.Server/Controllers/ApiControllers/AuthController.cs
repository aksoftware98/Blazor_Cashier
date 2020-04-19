using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCashier.Server.Services;
using BlazorCashier.Shared.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserService _userSerivce;
        public AuthController(UserService userService)
        {
            _userSerivce = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest model)
        {
            var result = await _userSerivce.LoginUserAsync(model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest("There is something wrong"); 
        }

    }
}