using BlazorCashier.Models.Identity;
using BlazorCashier.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers.ApiControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        #region Private Members

        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        public BaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        #endregion

        #region Helper Methods

        protected async Task<ApplicationUser> GetCurrentUser()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return await _userManager.FindByIdAsync(userId);
        }

        protected IActionResult ModelError()
            => BadRequest(new ApiResponse("Model has some errors"));

        #endregion
    }
}
