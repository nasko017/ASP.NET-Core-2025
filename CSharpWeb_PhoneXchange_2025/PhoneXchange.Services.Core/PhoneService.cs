using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.GCommon.Helpers;
using PhoneXchange.Web.ViewModels.Phone;

namespace PhoneXchange.Services.Core
{
    public class PhoneService
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
    }
}
