using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorCashier.Client.Blazor.Services;
using BlazorCashier.Client.Blazor.Extensions;

namespace BlazorCashier.Client.Blazor
{
    public class Program
    {
        const string API_URL = "https://localhost:44306/api"; 
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddDevExpressBlazor();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore(); 
            builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

            builder.Services.AddAkApiClientService();
            builder.Services.AddApiServices(API_URL);

            await builder.Build().RunAsync();
        }
    }
}
