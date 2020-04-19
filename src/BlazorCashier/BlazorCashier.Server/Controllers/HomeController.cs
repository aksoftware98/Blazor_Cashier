using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlazorCashier.Server.Models;
using BlazorCashier.Models.Data;
using Microsoft.AspNetCore.Identity;
using BlazorCashier.Models.Identity;
using BlazorCashier.Models;
using Microsoft.AspNetCore.Hosting;

namespace BlazorCashier.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env; 
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult GetStarted()
        {
            var model = new OrganizationViewModel
            {
                Countries = _db.Countries.Select(c => new CountryDetail { Id = c.Id, Name = c.Name }),
                Currencies = _db.Currencies.Select(c => new CurrencyDetail { Id = c.Id, Name = $"{c.Name} ({c.Symbol})" })
            };

            return View(model); 
        }

        

        // DONT CALL THIS ACTION 
        public async Task<IActionResult> HiddenAction()
        {
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = "Owner",
            });
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = "Sales",
            });
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = "Supervisor",
            });

            return RedirectToAction("GetStarted"); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
