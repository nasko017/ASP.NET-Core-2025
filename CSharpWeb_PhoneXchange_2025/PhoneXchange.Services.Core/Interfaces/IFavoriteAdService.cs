using PhoneXchange.Web.ViewModels.FavoriteAd;

namespace PhoneXchange.Services.Core.Interfaces
{
    public interface IFavoriteAdService
    {
        Task<bool> IsFavoriteAsync(string userId, int adId);
        Task AddToFavoritesAsync(string userId, int adId);
        Task RemoveFromFavoritesAsync(string userId, int adId);
        Task<IEnumerable<FavoriteAdViewModel>> GetFavoritesByUserAsync(string userId);
    }
}
