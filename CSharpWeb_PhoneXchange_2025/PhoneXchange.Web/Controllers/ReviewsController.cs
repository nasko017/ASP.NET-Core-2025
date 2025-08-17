using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.Review;
using System.Security.Claims;

namespace PhoneXchange.Web.Controllers
{
    [Authorize]
    public class ReviewsController : BaseController
    {
        private readonly IReviewService reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ReviewCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Ads", new { id = model.AdId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await reviewService.AddReviewAsync(model, userId);

            return RedirectToAction("Details", "Ads", new { id = model.AdId });
        }
    }
}
