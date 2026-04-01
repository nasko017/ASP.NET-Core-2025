using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneXchange.GCommon;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.Ad;
using System.Data;
using System.Security.Claims;

namespace PhoneXchange.Web.Controllers
{
    public class AdsController : Controller
    {
        private readonly IAdService adService;
        private readonly IBrandService brandService;
        private readonly IFavoriteAdService favoriteAdService;

        public AdsController(IAdService adService, IBrandService brandService, IFavoriteAdService favoriteAdService)
        {
            this.adService = adService;
            this.brandService = brandService;
            this.favoriteAdService = favoriteAdService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, int page = 1, int pageSize = 8)
        {
            var vm = await adService.GetFilteredAdsAsync(searchTerm, page, pageSize);
            return View(vm);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> My()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ads = await adService.GetAdsByUserAsync(userId!);
            return View(ads);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await brandService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = await brandService.GetAllAsync();
                return View(viewModel);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await adService.CreateAsync(viewModel, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ad = await adService.GetDetailsByIdAsync(id);
            if (ad == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                ad.IsFavorite = await favoriteAdService.IsFavoriteAsync(userId, ad.Id);
            }

            return View(ad);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var ad = await adService.GetByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ad == null)
                return NotFound();

            if (ad.OwnerId != userId && !User.IsInRole(Roles.Admin))
                return Forbid();

            var model = new AdEditViewModel
            {
                Id = ad.Id,
                Title = ad.Title,
                Description = ad.Description,
                Price = ad.Price,
                ImageUrls = ad.ImageUrls ?? new List<string>()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var ad = await adService.GetByIdAsync(model.Id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ad == null)
                return NotFound();

            if (ad.OwnerId != userId && !User.IsInRole(Roles.Admin))
                return Forbid();

            await adService.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var ad = await adService.GetByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ad == null)
                return NotFound();

            if (ad.OwnerId != userId && !User.IsInRole(Roles.Admin))
                return Forbid();

            return View(ad);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ad = await adService.GetByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ad == null)
                return NotFound();

            if (ad.OwnerId != userId && !User.IsInRole(Roles.Admin))
                return Forbid();

            await adService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}