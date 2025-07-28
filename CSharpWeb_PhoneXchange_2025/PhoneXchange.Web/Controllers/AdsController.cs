using Microsoft.AspNetCore.Mvc;
using PhoneXchange.Services.Core;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.Ad;
using System.Security.Claims;

namespace PhoneXchange.Web.Controllers
{
    public class AdsController : Controller
    {
        private readonly IAdService adService;
        private readonly IBrandService brandService;

        public AdsController(IAdService adService, IBrandService brandService)
        {
            this.adService = adService;
            this.brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ads = await adService.GetAllAsync();
            return View(ads);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await brandService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = await brandService.GetAllAsync(); // ВАЖНО!
                return View(viewModel);
            }

            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            await adService.CreateAsync(viewModel, userId);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ad = await adService.GetByIdAsync(id);
            if (ad == null)
                return NotFound();
            return View(ad);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ad = await adService.GetByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ad == null || (ad.OwnerId != userId && !User.IsInRole("Admin")))
                return Forbid();

            // Може да се наложи мапване към EditViewModel
            var model = new AdEditViewModel
            {
                Id = ad.Id,
                Title = ad.Title,
                Description = ad.Description,
                Price = ad.Price,
                ImageUrl = ad.ImageUrl
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdEditViewModel model)
        {
            var ad = await adService.GetByIdAsync(model.Id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ad == null || (ad.OwnerId != userId && !User.IsInRole("Admin")))
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            await adService.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ad = await adService.GetByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ad == null || (ad.OwnerId != userId && !User.IsInRole("Admin")))
                return Forbid();

            return View(ad);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ad = await adService.GetByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ad == null || (ad.OwnerId != userId && !User.IsInRole("Admin")))
                return Forbid();

            await adService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));

        }
    }
}
