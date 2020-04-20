using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Shared;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Common
{
    /// <summary>
    /// Represents a currency service
    /// </summary>
    public class CurrencyService : ICurrencyService
    {
        #region Private Members

        private readonly IRepository<Currency> _currencyRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CurrencyService(IRepository<Currency> currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        #endregion

        #region Public Methods

        public async Task<EntitiesApiResponse<Currency>> GetAllCurrenciesAsync()
        {
            return new EntitiesApiResponse<Currency>(entities: await _currencyRepository.TableNoTracking.ToListAsync());
        }

        #endregion

        #region Helper Methods


        #endregion
    }
}
