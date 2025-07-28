using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.Brand;

namespace PhoneXchange.Services.Core
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository brandRepository;

        public BrandService(IBrandRepository  _brandRepository)
        {
            brandRepository = _brandRepository;
        }

        public async Task<IEnumerable<BrandViewModel>> GetAllAsync()
        {
            var brands = await brandRepository.GetAllAsync();

            return brands.Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name
            });
        }
    }
}
