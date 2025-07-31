using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Web.ViewModels.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Services.Core
{
    public class PhoneService
    {
        private readonly IPhoneRepository phoneRepository;

        public PhoneService(IPhoneRepository _phoneRepository)
        {
            phoneRepository= _phoneRepository;
        }
        public async Task CreateAsync(PhoneCreateViewModel viewModel)
        {
            var phone = new Phone
            {
                Model = viewModel.Model,
                OS = viewModel.OS,
                IsNew = viewModel.IsNew,
                BrandId = viewModel.BrandId,
                Images = viewModel.ImageUrls.Select(url => new PhoneImage { ImageUrl = url }).ToList()
            };
            await phoneRepository.AddAsync(phone);
        }

    }
}
