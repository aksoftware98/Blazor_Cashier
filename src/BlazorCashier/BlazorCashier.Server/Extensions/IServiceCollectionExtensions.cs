using BlazorCashier.Models.Data;
using BlazorCashier.Models.Identity;
using BlazorCashier.Server.Infrastructure;
using BlazorCashier.Server.Services;
using BlazorCashier.Services;
using BlazorCashier.Services.Account;
using BlazorCashier.Services.Bills;
using BlazorCashier.Services.Common;
using BlazorCashier.Services.Items;
using BlazorCashier.Services.Organizations;
using BlazorCashier.Services.Stocks;
using BlazorCashier.Services.Vendors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlazorCashier.Server.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("BlazorCashier.Server")));

            services.AddScoped<IDbContext, ApplicationDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 5;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddApplicationAuthenticaiton(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["Audience"],
                    ValidIssuer = configuration["Issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Key"])),
                    ValidateIssuerSigningKey = true
                };
            });

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add services here
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<IWebHostEnvironmentProvider, WebHostEnvironmentProvider>();

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
        }
    }
}
