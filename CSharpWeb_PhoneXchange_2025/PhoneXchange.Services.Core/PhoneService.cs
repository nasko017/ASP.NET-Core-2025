using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.GCommon.Helpers;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.Phone;

namespace PhoneXchange.Services.Core
{
    public class PhoneService : IPhoneService
    {
        private readonly IPhoneRepository phoneRepository;

        public PhoneService(IPhoneRepository _phoneRepository)
        {
            phoneRepository = _phoneRepository;
        }

        public async Task CreateAsync(PhoneCreateViewModel viewModel)
        {
            var phone = new Phone
            {
                Model = viewModel.Model,
                OS = viewModel.OS,
                IsNew = viewModel.IsNew,
                BrandId = viewModel.BrandId,
                ImageUrlsSerialized = ImageUrlHelper.Serialize(viewModel.ImageUrls)
            };

            await phoneRepository.AddAsync(phone);
        }

        public async Task<IEnumerable<PhoneViewModel>> GetAllAsync()
        {
            var phones = await phoneRepository.GetAllAsync();

            return phones.Select(p => new PhoneViewModel
            {
                Id = p.Id,
                Model = p.Model,
                OS = p.OS,
                IsNew = p.IsNew,
                BrandId = p.BrandId,
                BrandName = p.Brand != null ? p.Brand.Name : string.Empty,
                ImageUrls =ImageUrlHelper.Deserialize(p.ImageUrlsSerialized)
            });
        }

        public async Task<PhoneViewModel?> GetByIdAsync(int id)
        {
            var phone = await phoneRepository.GetByIdAsync(id);

            if (phone == null)
            {
                return null;
            }

            return new PhoneViewModel
            {
                Id = phone.Id,
                Model = phone.Model,
                OS = phone.OS,
                IsNew = phone.IsNew,
                BrandId = phone.BrandId,
                BrandName = phone.Brand != null ? phone.Brand.Name : string.Empty,
                ImageUrls =ImageUrlHelper.Deserialize(phone.ImageUrlsSerialized)
            };
        }

    }
}
