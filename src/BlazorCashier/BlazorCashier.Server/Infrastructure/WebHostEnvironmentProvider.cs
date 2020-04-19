using BlazorCashier.Services;
using Microsoft.AspNetCore.Hosting;

namespace BlazorCashier.Server.Infrastructure
{
    public class WebHostEnvironmentProvider : IWebHostEnvironmentProvider
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public WebHostEnvironmentProvider(IWebHostEnvironment env)
            => (WebRootPath, ContentRootPath) 
             = (env.WebRootPath, env.ContentRootPath);

        #endregion

        #region Public Properties

        public string WebRootPath { get; }

        public string ContentRootPath { get; }

        #endregion
    }
}
