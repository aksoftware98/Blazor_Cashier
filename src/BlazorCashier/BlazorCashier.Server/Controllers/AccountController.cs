using BlazorCashier.Models.Data;
using BlazorCashier.Server.Models;
using BlazorCashier.Services.Common;
using BlazorCashier.Services.Organizations;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers
{
    public class AccountController : Controller
    {
        #region Private Members

        private readonly IOrganizationService _orgService;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;

        #endregion

        #region Constructors

        public AccountController(
            IOrganizationService orgService,
            ICountryService countryService,
            ICurrencyService currencyService)
            => (_orgService, _countryService, _currencyService) 
             = (orgService, countryService, currencyService);

        #endregion

        #region Actions

        [HttpGet]
        public IActionResult Register()
            => View(InitializeViewModel(new OrganizationViewModel()));

        [HttpPost]
        public async Task<IActionResult> Register(OrganizationViewModel model)
        {
            if (!ModelState.IsValid) return View(InitializeViewModel(model));

            var result = await _orgService.AddOrganizationAsync(model.ToOrganizationDetail());

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Error);
                return View(InitializeViewModel(model));
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Helper Methods

        public async Task<OrganizationViewModel> InitializeViewModel(OrganizationViewModel viewModel)
        {
            viewModel.Countries = (await _countryService.GetAllCountriesAsync()).Entities.Select(c => new CountryDetail { Id = c.Id, Name = c.Name });
            viewModel.Currencies = (await _currencyService.GetAllCurrenciesAsync()).Entities.Select(c => new CurrencyDetail { Id = c.Id, Name = c.Name });
            return viewModel;
        }

        #endregion
    }
}
