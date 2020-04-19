using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Services.Responses;
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

        public async Task<CollectionEntityResponse<Country>> GetAllCountriesAsync()
        {
            return new CollectionEntityResponse<Country>(entities: await _countryRepository.TableNoTracking.ToListAsync());
        }

        #endregion

        #region Helper Methods


        #endregion
    }
}
