using AKSoftware.WebApi.Client;
using BlazorCashier.Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorCashier.Client.Services
{
    public abstract class ApiService : IApiService
    {

        protected readonly ServiceClient ApiClient;

        public ApiService(ServiceClient apiClient, string baseUrl)
        {
            ApiClient = apiClient;
            BaseUrl = baseUrl; 
        }

        public string BaseUrl { get; }
    }
}
