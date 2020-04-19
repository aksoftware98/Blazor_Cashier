using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Services.Responses;
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
            => _currencyRepository = currencyRepository;

        #endregion

        #region Public Methods

        public async Task<CollectionEntityResponse<Currency>> GetAllCurrenciesAsync()
            => new CollectionEntityResponse<Currency>
                (entities: await _currencyRepository.TableNoTracking.ToListAsync());

        #endregion

        #region Helper Methods


        #endregion
    }
}
