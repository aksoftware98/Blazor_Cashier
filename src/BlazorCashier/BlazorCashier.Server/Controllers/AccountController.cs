using BlazorCashier.Models;
using BlazorCashier.Models.Data;
using BlazorCashier.Models.Identity;
using BlazorCashier.Server.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCashier.Server.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public AccountController(
            ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IWebHostEnvironment env)
             => (_db, _userManager, _roleManager, _env)
                = (db, userManager, roleManager, env);

        [HttpGet]
        public IActionResult Register()
            => View(InitializeViewModel(new OrganizationViewModel()));

        [HttpPost]
        public async Task<IActionResult> Register(OrganizationViewModel model)
        {
            if (!ModelState.IsValid) return View(InitializeViewModel(model));

            var organization = new Organization
            {
                Address = model.Address,
                City = model.City,
                Country = model.CountryId,
                CurrencyId = model.CurrencyId,
                Email = model.Email,
                FinancialNumber = model.FinancialNumber,
                Phone = model.Phone,
                RegistrationDate = DateTime.UtcNow,
                TelePhone = model.Telephone,
                Website = model.Website,
                OwnerName = model.OwnerName,
                Name = model.FullName,
                Id = Guid.NewGuid().ToString(),
            };

            await _db.Organizations.AddAsync(organization);
            await _db.SaveChangesAsync();

            var user = new ApplicationUser()
            {
                FirstName = model.FullName,
                LastName = "Admin",
                ProfilePicture = $"{_env.WebRootPath.Replace("\\\\", "/")}/Images/Users/default.png",
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            // Check 
            await _userManager.AddToRoleAsync(user, "Owner");

            return RedirectToAction("Index", "Home");
        }

        #region Helper Methods

        public OrganizationViewModel InitializeViewModel(OrganizationViewModel viewModel)
        {
            viewModel.Countries = _db.Countries.Select(c => new CountryDetail { Id = c.Id, Name = c.Name });
            viewModel.Currencies = _db.Currencies.Select(c => new CurrencyDetail { Id = c.Id, Name = c.Name });
            return viewModel;
        }

        #endregion
    }
}
