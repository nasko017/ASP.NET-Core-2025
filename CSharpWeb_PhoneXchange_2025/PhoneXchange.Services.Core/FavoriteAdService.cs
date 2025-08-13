using Microsoft.EntityFrameworkCore;
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.GCommon.Helpers;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.FavoriteAd;

namespace PhoneXchange.Services.Core
{
    public class FavoriteAdService : IFavoriteAdService
    {
        private readonly IFavoriteAdRepository favoriteAdRepository;
        private readonly IAdRepository adRepository;

        public FavoriteAdService(
            IFavoriteAdRepository favoriteAdRepository,
            IAdRepository adRepository)
        {
            this.favoriteAdRepository = favoriteAdRepository;
            this.adRepository = adRepository;
        }

        public Task<bool> IsFavoriteAsync(string userId, int adId)
        {
            return favoriteAdRepository
                .GetAllAttached()
                .AnyAsync(f => f.ApplicationUserId == userId && f.AdId == adId);
        }


        public async Task AddToFavoritesAsync(string userId, int adId)
        {
            if (!await IsFavoriteAsync(userId, adId))
            {
                var favorite = new FavoriteAd
                {
                    ApplicationUserId = userId,
                    AdId = adId,
                    FavoritedOn = DateTime.UtcNow
                };

                await favoriteAdRepository.AddAsync(favorite);
            }
        }

        public async Task RemoveFromFavoritesAsync(string userId, int adId)
        {
            var favorite = await favoriteAdRepository
                .FirstOrDefaultAsync(f => f.ApplicationUserId == userId && f.AdId == adId);

            if (favorite != null)
            {
                await favoriteAdRepository.HardDeleteAsync(favorite);
            }
        }

        public async Task<IEnumerable<FavoriteAdViewModel>> GetFavoritesByUserAsync(string userId)
        {
            return (await favoriteAdRepository
                .GetAllAttached()
                .Where(f => f.ApplicationUserId == userId)
                .Select(f => new
                {
                    AdId = f.Ad.Id,
                    f.Ad.Title,
                    f.Ad.Price,
                    Img = f.Ad.Phone != null ? f.Ad.Phone.ImageUrlsSerialized : null
                })
                .ToListAsync())
                .Select(x => new FavoriteAdViewModel
                {
                    AdId = x.AdId,
                    Title = x.Title ?? string.Empty,
                    Price = x.Price,
                    ImageUrl = x.Img != null
                        ? (ImageUrlHelper.Deserialize(x.Img).FirstOrDefault() ?? string.Empty)
                        : string.Empty
                });
        }



    }
}
