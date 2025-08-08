using PhoneXchange.Web.ViewModels.Review;

namespace PhoneXchange.Services.Core.Interfaces
{
    public interface IReviewService
    {
        Task AddReviewAsync(ReviewCreateViewModel model, string userId);
        Task<IEnumerable<ReviewViewModel>> GetReviewsForAdAsync(int adId);
    }
}
