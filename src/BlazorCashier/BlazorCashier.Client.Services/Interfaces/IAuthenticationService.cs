using BlazorCashier.Shared;
using BlazorCashier.Shared.Identity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCashier.Client.Services.Interfaces
{
    public interface IAuthenticationService : IApiService
    {
        /// <summary>
        /// Request an access token for the user based on his username and password credentials 
        /// </summary>
        /// <param name="username">Username associated with the user account</param>
        /// <param name="password">Password for the user account</param>
        /// <returns>HttpResponseMessage from the service and the UserManagerResponse object that holds the user access token and the expiration date for that token</returns>
        Task<(HttpResponseMessage , IdentityApiResponse)> AuthenticateUserAsync(string username, string password);     
    }


}
