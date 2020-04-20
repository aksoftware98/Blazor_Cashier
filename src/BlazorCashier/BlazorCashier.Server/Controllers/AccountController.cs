using BlazorCashier.Server.Models;
using BlazorCashier.Services.Account;
using BlazorCashier.Services.Common;
using BlazorCashier.Services.Organizations;
using BlazorCashier.Shared.Domain;
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
        private readonly IUserService _userService;

        #endregion

        #region Constructors

        public AccountController(IOrganizationService orgService,
                                 ICountryService countryService,
                                 ICurrencyService currencyService,
                                 IUserService userService)
        {
            _orgService = orgService;
            _countryService = countryService;
            _currencyService = currencyService;
            _userService = userService;
        }

        #endregion

        #region Actions

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            return View(await InitializeViewModel(new OrganizationViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> Register(OrganizationViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(await InitializeViewModel(model));

            var result = await _orgService.AddOrganizationAsync(model.ToOrganizationDetail());

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Error);
                return View(await InitializeViewModel(model));
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Helper Methods

        public async Task<OrganizationViewModel> InitializeViewModel(OrganizationViewModel viewModel)
        {
            viewModel.Countries = (await _countryService.GetAllCountriesAsync()).Entities.Select(c => new CountryDetail(c.Id, c.Code, c.Name));
            viewModel.Currencies = (await _currencyService.GetAllCurrenciesAsync()).Entities.Select(c => new CurrencyDetail(c.Id, c.Code));
            return viewModel;
        }

        #endregion
    }
}
