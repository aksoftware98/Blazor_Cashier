using AKSoftware.WebApi.Client;
using BlazorCashier.Client.Services;
using BlazorCashier.Client.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Client.Blazor.Extensions
{
    static class ServiceExtensions
    {

        /// <summary>
        /// Register all the API services in the Dependency Injection container 
        /// </summary>
        /// <param name="services">IServiceCollection object</param>
        /// <returns></returns>
        public static IServiceCollection AddApiServices(this IServiceCollection services, string apiBaseUrl)
        {
            // Add the authentication service 
            return services.RegisterApiService<IAuthenticationService, ApiAuthenticationService>(apiBaseUrl);
        }

        public static IServiceCollection AddAkApiClientService(this IServiceCollection services)
        {
            return services.AddScoped<ServiceClient>();
        }

        #region Helper Methods

        /// <summary>
        /// Register an API Service that implements
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        static IServiceCollection RegisterApiService<TService, TImplementation>(this IServiceCollection services, string baseUrl) where TService : IApiService
                                                                                                             where TImplementation : ApiService
        {
            return services.AddScoped(typeof(TService), s =>
            {
                    // Get the AKSoftware.WebApi.Client.ClientService instance 
                    var apiClientService = s.GetService<ServiceClient>();

                    // Create an instance of the API service type 
                    return (TImplementation)Activator.CreateInstance(typeof(TImplementation), apiClientService, baseUrl);
            });
        }
        #endregion 

    }
}
