using Microsoft.AspNetCore.Mvc;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.Phone;

namespace PhoneXchange.Web.Controllers
{
    public class PhonesController:BaseController
    {
        private readonly IPhoneService phoneService;
        private readonly IBrandService brandService;

        public PhonesController(IPhoneService phoneService, IBrandService brandService)
        {
            this.phoneService = phoneService;
            this.brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var phones = await phoneService.GetAllAsync();
            return View(phones);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await brandService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhoneCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = await brandService.GetAllAsync();
                return View(model);
            }
            await phoneService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
