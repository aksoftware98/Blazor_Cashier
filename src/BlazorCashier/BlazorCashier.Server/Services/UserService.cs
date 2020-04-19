using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Models.Identity;
using BlazorCashier.Shared.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Services
{
    public class UserService
    {
        private UserManager<ApplicationUser> _userManger;
        private IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public UserService(UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext db, IWebHostEnvironment env)
        {
            _userManger = userManager;
            _configuration = configuration;
            _db = db;
            _env = env; 
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterRequest model, string organizationId, string userId)
        {
            if (model == null)
                throw new NullReferenceException("Reigster Model is null");

            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };

            var identityUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                OrganizationId = organizationId,
                ProfilePicture = "Images/users/default.png",
            };

            var result = await _userManger.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                await _userManger.AddToRoleAsync(identityUser, model.RoleId); 

                var employee = new Employee
                {
                    Id = Guid.NewGuid().ToString(),
                    Address = model.Address1,
                    StreetAddress = model.Address2,
                    City = model.City,
                    UserId = identityUser.Id,
                    BirthDate = model.Birthdate,
                    CreatedById = userId,
                    ModifiedById = userId,
                    Description = model.Description,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    OrganizationId = organizationId
                };

                return new UserManagerResponse
                {
                    Message = "User created successfully!",
                    IsSuccess = true,
                };
            }

            return new UserManagerResponse
            {
                Message = "User did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginRequest model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that Email address",
                    IsSuccess = false,
                };
            }

            var result = await _userManger.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new UserManagerResponse
                {
                    Message = "Invalid password",
                    IsSuccess = false,
                };
            
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim("OrgId", user.OrganizationId),
                new Claim("ProfilePicture", $"{_env.WebRootPath.Replace("\\\\", "/")}/{user.ProfilePicture}"), 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }
    }
}
