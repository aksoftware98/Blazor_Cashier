using BlazorCashier.Models.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorCashier.Server.Extensions
{
    public static class ApplicationUserExtensions
    {
        public static JwtSecurityToken GenerateJwtToken(this ApplicationUser user, IConfiguration configuration, IWebHostEnvironment env)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim("OrgId", user.OrganizationId),
                new Claim("ProfilePicture", $"{env.WebRootPath.Replace("\\\\", "/")}/{user.ProfilePicture}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Key"]));

            return new JwtSecurityToken(
                issuer: configuration["Issuer"],
                audience: configuration["Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        }
    }
}
