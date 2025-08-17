using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneXchange.Services.Core;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels;
using System.Diagnostics;

namespace PhoneXchange.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IAdService adService;
        public HomeController(IAdService _adService)
        {
            adService = _adService;
        }  

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var latest = await adService.GetFilteredAdsAsync(
                searchTerm: null,
                page: 1,
                pageSize: 3);
            return View(latest.Ads);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
