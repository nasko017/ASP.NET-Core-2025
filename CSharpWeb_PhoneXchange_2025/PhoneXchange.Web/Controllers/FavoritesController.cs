using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneXchange.Services.Core.Interfaces;
using System.Security.Claims;

namespace PhoneXchange.Web.Controllers
{
    [Authorize]
    public class FavoritesController : BaseController
    {
        private readonly IFavoriteAdService favoriteAdService;

        public FavoritesController(IFavoriteAdService favoriteAdService)
        {
            this.favoriteAdService = favoriteAdService;
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int adId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await favoriteAdService.IsFavoriteAsync(userId, adId))
            {
                await favoriteAdService.RemoveFromFavoritesAsync(userId, adId);
                TempData["Success"] = "Обявата беше премахната от любими.";
            }
            else
            {
                await favoriteAdService.AddToFavoritesAsync(userId, adId);
                TempData["Success"] = "Обявата беше добавена в любими.";
            }

            return RedirectToAction("Details", "Ads", new { id = adId });
        }

        [HttpGet]
        public async Task<IActionResult> MyFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await favoriteAdService.GetFavoritesByUserAsync(userId);
            return View(favorites);
        }
    }

}

