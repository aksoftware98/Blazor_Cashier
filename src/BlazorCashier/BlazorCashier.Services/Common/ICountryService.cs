using BlazorCashier.Models;
using BlazorCashier.Services.Responses;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Common
{
    public interface ICountryService
    {
        /// <summary>
        /// Retrieves all countries
        /// </summary>
        /// <returns>Response containing entities retrieved</returns>
        Task<CollectionEntityResponse<Country>> GetAllCountriesAsync();
    }
}
