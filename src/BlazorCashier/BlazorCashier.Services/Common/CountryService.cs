using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Shared;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlazorCashier.Services.Common
{
    /// <summary>
    /// Represents a country service
    /// </summary>
    public class CountryService : ICountryService
    {
        #region Private Members

        private readonly IRepository<Country> _countryRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CountryService(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        #endregion

        #region Public Methods

        public async Task<EntitiesApiResponse<Country>> GetAllCountriesAsync()
        {
            var countries = await _countryRepository.TableNoTracking.ToListAsync();
            return new EntitiesApiResponse<Country>(countries);
        }

        #endregion

        #region Helper Methods


        #endregion
    }
}
