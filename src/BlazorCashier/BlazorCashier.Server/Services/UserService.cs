using BlazorCashier.Models.Extensions;
using BlazorCashier.Models.Identity;
using BlazorCashier.Server.Extensions;
using BlazorCashier.Services.Account;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Services
{
    public class UserService : IUserService
    {
        #region Private Members

        private UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        #endregion

        #region Constructors

        public UserService(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, 
            IWebHostEnvironment env)
        {
            _userManger = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _env = env;
        }

        #endregion

        #region Public Methods

        public async Task<ApiResponse> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManger.DeleteAsync(user);

            if (!result.Succeeded)
                return new ApiResponse(result.Errors.Select(error => error.Description).AllInOne());

            return new ApiResponse();
        }

        public async Task<EntityApiResponse<ApplicationUser>> CreateUserAsync(CreateApplicationUser userDetail)
        {
            if (userDetail is null)
                throw new ArgumentNullException(nameof(userDetail));

            var role = await _roleManager.FindByIdAsync(userDetail.RoleId);

            if (role is null)
                return new EntityApiResponse<ApplicationUser>(error: "Role does not exist");

            var user = new ApplicationUser
            {
                FirstName = userDetail.FirstName,
                LastName = userDetail.LastName,
                Email = userDetail.Email,
                UserName = userDetail.Email,
                ProfilePicture = userDetail.ProfilePicture,
                OrganizationId = userDetail.OrganizationId
            };

            var result = await _userManger.CreateAsync(user, userDetail.Password);

            if (!result.Succeeded)
                return new EntityApiResponse<ApplicationUser>(error: result.Errors.Select(e => e.Description).AllInOne());

            await _userManger.AddToRoleAsync(user, role.Name);

            return new EntityApiResponse<ApplicationUser>(entity: user);
        }

        public async Task<EntityApiResponse<ApplicationUser>> UpdateUserAsync(string userId, UpdateApplicationUser userDetail)
        {
            if (userDetail is null)
                throw new ArgumentNullException(nameof(userDetail));

            var user = await _userManger.FindByIdAsync(userId);

            user.FirstName = userDetail.FirstName;
            user.LastName = userDetail.LastName;
            user.ProfilePicture = userDetail.ProfilePicture;

            var result = await _userManger.UpdateAsync(user);

            if (!result.Succeeded)
                return new EntityApiResponse<ApplicationUser>(error: result.Errors.Select(error => error.Description).AllInOne());

            return new EntityApiResponse<ApplicationUser>(entity: user);
        }

        public async Task<ApiResponse> ChangePasswordForUserAsync(ChangePasswordRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var user = await _userManger.FindByIdAsync(request.Id);

            if (user is null)
                return new ApiResponse("User does not exist");

            if (request.NewPassword != request.ConfirmNewPassword)
                return new ApiResponse("Passwords do not match");

            var result = await _userManger.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
                return new ApiResponse(result.Errors.Select(error => error.Description).AllInOne());

            return new ApiResponse();
        }

        public async Task<IdentityApiResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManger.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new IdentityApiResponse("Invalid login attempt");
            }

            var result = await _userManger.CheckPasswordAsync(user, request.Password);
            
            if (!result)
                return new IdentityApiResponse("Invalid login attempt");

            var token = user.GenerateJwtToken(_configuration, _env);

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new IdentityApiResponse
            {
                AccessToken = tokenAsString,
                 ExpireDate = token.ValidTo
            };  
        }

        #endregion
    }
}
