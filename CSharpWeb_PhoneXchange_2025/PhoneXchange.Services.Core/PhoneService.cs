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

        Task<IEnumerable<PhoneViewModel>> IPhoneService.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<PhoneViewModel?> IPhoneService.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
