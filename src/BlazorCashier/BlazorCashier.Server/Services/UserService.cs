using BlazorCashier.Models.Identity;
using BlazorCashier.Server.Extensions;
using BlazorCashier.Services.Account;
using BlazorCashier.Shared;
using BlazorCashier.Shared.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Services
{
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> _userManger;
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public UserService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IWebHostEnvironment env)
        {
            _userManger = userManager;
            _configuration = configuration;
            _env = env;
        }

        //public async Task<UserManagerResponse> RegisterUserAsync(RegisterRequest model, string organizationId, string userId)
        //{
        //    if (model == null)
        //        throw new NullReferenceException("Reigster Model is null");

        //    if (model.Password != model.ConfirmPassword)
        //        return new UserManagerResponse
        //        {
        //            Message = "Confirm password doesn't match the password",
        //            IsSuccess = false,
        //        };

        //    var identityUser = new ApplicationUser
        //    {
        //        Email = model.Email,
        //        UserName = model.Email,
        //        FirstName = model.FirstName,
        //        LastName = model.LastName,
        //        OrganizationId = organizationId,
        //        ProfilePicture = "Images/users/default.png",
        //    };

        //    var result = await _userManger.CreateAsync(identityUser, model.Password);

        //    if (result.Succeeded)
        //    {
        //        await _userManger.AddToRoleAsync(identityUser, model.RoleId); 

        //        var employee = new Employee
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            Address = model.Address1,
        //            StreetAddress = model.Address2,
        //            City = model.City,
        //            UserId = identityUser.Id,
        //            BirthDate = model.Birthdate,
        //            CreatedById = userId,
        //            ModifiedById = userId,
        //            Description = model.Description,
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,
        //            OrganizationId = organizationId
        //        };

        //        return new UserManagerResponse
        //        {
        //            Message = "User created successfully!",
        //            IsSuccess = true,
        //        };
        //    }

        //    return new UserManagerResponse
        //    {
        //        Message = "User did not create",
        //        IsSuccess = false,
        //        Errors = result.Errors.Select(e => e.Description)
        //    };
        //}
        
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
    }
}
