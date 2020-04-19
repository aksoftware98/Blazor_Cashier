using BlazorCashier.Models;
using BlazorCashier.Services.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Common
{
    public interface ICurrencyService
    {
        /// <summary>
        /// Retrieves all currencies
        /// </summary>
        /// <returns>Response containing entities retrieved</returns>
        Task<CollectionEntityResponse<Currency>> GetAllCurrenciesAsync();
    }
}
