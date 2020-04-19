using BlazorCashier.Shared.Identity;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorCashier.Client.Blazor.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private const string USER_KEY = "user_token"; 

        private readonly ILocalStorageService _localStorage;

        public JwtAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage; 
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if(await _localStorage.ContainKeyAsync(USER_KEY))
            {
                var userToken = await _localStorage.GetItemAsync<UserManagerResponse>(USER_KEY);
                // Get the user claims 
                var claims = ExtractUserClaims(userToken.Message);

                // TODO: Refresh the token if it's expired 
                var claimsIdentity = new ClaimsIdentity(claims, "JwtBearerToken");
                var user = new ClaimsPrincipal(claimsIdentity); 
                return new AuthenticationState(user);
            }

            // Empty user
            return new AuthenticationState(new ClaimsPrincipal()); 
        }

        /// <summary>
        /// SignIn the user 
        /// </summary>
        /// <param name="user">User object that contains the access token and it's expiration date</param>
        /// <returns></returns>
        public async Task SignUserInAsync(UserManagerResponse user)
        {
            if(!(await _localStorage.ContainKeyAsync(USER_KEY)))
            {
                await _localStorage.SetItemAsync(USER_KEY, user);

                // Notify the AuthorizeView about the change occured to recheck the authentication state
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync()); 
            }
        }

        public async Task SignUserOutAsync()
        {
            if(await _localStorage.ContainKeyAsync(USER_KEY))
            {
                await _localStorage.RemoveItemAsync(USER_KEY); 
            }
        }

        /// <summary>
        /// Extract all the claims from the granted token 
        /// </summary>
        /// <param name="accessToken">Token retrieved from the API</param>
        /// <returns>Set of claims that token holds</returns>
        private IEnumerable<Claim> ExtractUserClaims(string accessToken)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.ReadJwtToken(accessToken);
            return token.Claims;
        }

    }
}
