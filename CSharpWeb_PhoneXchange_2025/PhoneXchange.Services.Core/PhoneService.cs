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
        public async Task CreateAsync(PhoneCreateViewModel model)
        {
            var phone = new Phone
            {
                Model = model.Model,
                OS = model.OS,
                IsNew = model.IsNew,
                BrandId = model.BrandId,
                Images = model.ImageUrls.Select(url => new PhoneImage { ImageUrl = url }).ToList()
            };
            await phoneRepository.AddAsync(phone);
        }

    }
}
