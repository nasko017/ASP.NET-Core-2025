using Microsoft.EntityFrameworkCore;
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.Review;

namespace PhoneXchange.Services.Core
{
    public class ReviewService:IReviewService
    {
        private readonly IReviewRepository reviewRepository;

        public ReviewService(IReviewRepository _reviewRepository)
        {
            reviewRepository = _reviewRepository;
        }

        public async Task AddReviewAsync(ReviewCreateViewModel model, string userId)
        {
            var review = new Review
            {
                AdId = model.AdId,
                Comment = model.Comment,
                Rating = model.Rating,
                AuthorId = userId,
                CreatedOn = DateTime.UtcNow
            };

            await reviewRepository.AddAsync(review);
        }

        public async Task<IEnumerable<ReviewViewModel>> GetReviewsForAdAsync(int adId)
        {
            return await reviewRepository
                .GetAllAttached()
                .Where(r => r.AdId == adId)
                .Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Comment = r.Comment,
                    Rating = r.Rating,
                    CreatedOn = r.CreatedOn,
                    AuthorEmail = (r.Author == null ? null : r.Author.Email) ?? "Непознат"
                })
                .ToListAsync();
        }

    }
}
