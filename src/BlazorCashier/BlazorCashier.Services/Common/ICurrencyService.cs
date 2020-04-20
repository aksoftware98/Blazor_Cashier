using BlazorCashier.Models;
using BlazorCashier.Shared;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Common
{
    public interface ICurrencyService
    {
        /// <summary>
        /// Retrieves all currencies
        /// </summary>
        /// <returns>Response containing entities retrieved</returns>
        Task<EntitiesApiResponse<Currency>> GetAllCurrenciesAsync();
    }
}
