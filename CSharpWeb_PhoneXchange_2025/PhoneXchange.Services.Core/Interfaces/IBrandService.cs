using PhoneXchange.Web.ViewModels.Brand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Services.Core.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandViewModel>> GetAllAsync();
    }

}
