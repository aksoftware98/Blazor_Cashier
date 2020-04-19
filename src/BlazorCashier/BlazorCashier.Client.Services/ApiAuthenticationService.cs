using AKSoftware.WebApi.Client;
using BlazorCashier.Client.Services.Interfaces;
using BlazorCashier.Shared.Identity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCashier.Client.Services
{
    public class ApiAuthenticationService : ApiService, IAuthenticationService
    {

        public ApiAuthenticationService(ServiceClient apiClient, string baseUrl) : base(apiClient, baseUrl)
        {

        }


        public async Task<(HttpResponseMessage, UserManagerResponse)> AuthenticateUserAsync(string username, string password)
        {
            var response = await ApiClient.PostAsync<UserManagerResponse>($"{BaseUrl}/auth/login", new LoginRequest
            {
                Email = username,
                Password = password
            });

            return (response.HttpResponse, response.Result); 
        }
    }
}
