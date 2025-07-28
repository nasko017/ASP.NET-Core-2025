using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.Ad;

namespace PhoneXchange.Services.Core
{
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
            // Винаги попълвай OwnerId!
            return ads.Select(a => new AdViewModel
            {
                Id = a.Id,
                Title = a.Title ?? "",
                Description = a.Description ?? "",
                Price = a.Price,
                ImageUrl = a.Phone?.Images?.FirstOrDefault()?.ImageUrl ?? "",
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
                Title = ad.Title ?? "",
                Description = ad.Description ?? "",
                Price = ad.Price,
                ImageUrl = ad.Phone?.Images?.FirstOrDefault()?.ImageUrl ?? "",
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
                    Images = new List<PhoneImage>
            {
                new PhoneImage { ImageUrl = model.ImageUrl }
            }
                }
            };

            await adRepository.AddAsync(ad);
        }
        public async Task EditAsync(AdEditViewModel model)
        {
            var ad = await adRepository.GetByIdAsync(model.Id);
            if (ad == null) return;

            ad.Title = model.Title;
            ad.Description = model.Description;
            ad.Price = model.Price;

            // Ако имаш само едно изображение:
            if (ad.Phone?.Images?.Any() == true)
            {
                ad.Phone.Images.First().ImageUrl = model.ImageUrl;
            }

            await adRepository.UpdateAsync(ad);
        }

        public async Task DeleteAsync(int id)
        {
            var ad = await adRepository.GetByIdAsync(id);
            if (ad != null)
            {
                await adRepository.DeleteAsync(ad);
            }
        }
    }
}
