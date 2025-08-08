using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.Ad;
using PhoneXchange.Web.ViewModels.Review;

namespace PhoneXchange.Services.Core
{
    using Microsoft.EntityFrameworkCore;
    using PhoneXchange.GCommon.Helpers;
    public class AdService : IAdService
    {
        private readonly IAdRepository adRepository;

        public AdService(IAdRepository adRepository)
        {
            this.adRepository = adRepository;
        }

        public async Task<IEnumerable<AdViewModel>> GetAllAsync()
        {
            var ads = await adRepository.GetAllAsync();

            return ads.Select(a => new AdViewModel
            {
                Id = a.Id,
                Title = a.Title ?? string.Empty,
                Description = a.Description ?? string.Empty,
                Price = a.Price,
                ImageUrls = a.Phone != null
                    ? ImageUrlHelper.Deserialize(a.Phone.ImageUrlsSerialized)
                    : new List<string>(),
                OwnerId = a.OwnerId
            });
        }

        public async Task<AdViewModel?> GetByIdAsync(int id)
        {
            var ad = await adRepository.GetByIdAsync(id);

            if (ad == null) return null;

            return new AdViewModel
            {
                Id = ad.Id,
                Title = ad.Title,
                Description = ad.Description,
                Price = ad.Price,
                ImageUrls = ad.Phone != null
                    ? ImageUrlHelper.Deserialize(ad.Phone.ImageUrlsSerialized)
                    : new List<string>(),
                OwnerId = ad.OwnerId
            };
        }

        public async Task CreateAsync(AdCreateViewModel model, string userId)
        {
            var ad = new Ad
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                OwnerId = userId,
                CreatedOn = DateTime.UtcNow,
                Phone = new Phone
                {
                    Model = model.Model,
                    OS = model.OS,
                    IsNew = model.IsNew,
                    BrandId = model.BrandId,
                    ImageUrlsSerialized = ImageUrlHelper.Serialize(model.ImageUrls)
                }
            };

            await adRepository.AddAsync(ad);
        }

        public async Task<IEnumerable<AdListViewModel>> GetAdsByUserAsync(string userId)
        {
            return await adRepository
                .GetAllAttached()
                .Where(a => a.OwnerId == userId)
                .Select(a => new AdListViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Price = a.Price,
                    ImageUrl = ImageUrlHelper.Deserialize(a.Phone.ImageUrlsSerialized).FirstOrDefault() ?? string.Empty
                })
                .ToListAsync();
        }

        public async Task<AdDetailsViewModel?> GetDetailsByIdAsync(int id)
        {
            var ad = await adRepository.GetByIdWithDetailsAsync(id);
            if (ad == null) return null;

            return new AdDetailsViewModel
            {
                Id = ad.Id,
                Title = ad.Title,
                Description = ad.Description,
                Price = ad.Price,
                Model = ad.Phone.Model,
                OS = ad.Phone.OS,
                IsNew = ad.Phone.IsNew,
                BrandName = ad.Phone.Brand.Name,
                ImageUrls = ImageUrlHelper.Deserialize(ad.Phone.ImageUrlsSerialized),
                OwnerId = ad.OwnerId,
                ReviewsCount = ad.Reviews.Count,
                AvgRating = ad.Reviews.Any() ? ad.Reviews.Average(r => r.Rating) : 0,
                Reviews = ad.Reviews.Select(r => new ReviewViewModel
                {
                    AuthorEmail = r.Author?.Email ?? "Непознат",
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedOn = r.CreatedOn
                }).ToList()
            };
        }

        public async Task EditAsync(AdEditViewModel model)
        {
            var ad = await adRepository.GetByIdAsync(model.Id);
            if (ad == null) return;

            ad.Title = model.Title;
            ad.Description = model.Description;
            ad.Price = model.Price;

            if (ad.Phone != null)
            {
                ad.Phone.ImageUrlsSerialized = ImageUrlHelper.Serialize(model.ImageUrls);
            }

            await adRepository.UpdateAsync(ad);
        }

        public async Task DeleteAsync(int id)
        {
            var ad = await adRepository.GetByIdAsync(id);
            if (ad != null)
            {
                await adRepository.HardDeleteAsync(ad);
            }
        }

        public async Task<AdsListViewModel> GetFilteredAdsAsync(string? searchTerm, int page, int pageSize)
        {
            var query = adRepository.GetAllAttached();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => a.Title.Contains(searchTerm) || a.Description.Contains(searchTerm));
            }

            int totalCount = await query.CountAsync();

            var ads = await query
                .OrderByDescending(a => a.CreatedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AdViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Price = a.Price,
                    ImageUrls = a.Phone != null ? ImageUrlHelper.Deserialize(a.Phone.ImageUrlsSerialized) : new List<string>(),
                    OwnerId = a.OwnerId
                })
                .ToListAsync();

            return new AdsListViewModel
            {
                Ads = ads,
                SearchTerm = searchTerm,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

    }
}
