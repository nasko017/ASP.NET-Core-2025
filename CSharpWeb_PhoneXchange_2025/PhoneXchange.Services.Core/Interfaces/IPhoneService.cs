using PhoneXchange.Web.ViewModels.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Services.Core.Interfaces
{
    public interface IPhoneService
    {
        Task<IEnumerable<PhoneViewModel>> GetAllAsync();
        Task<PhoneViewModel?> GetByIdAsync(int id);
        Task CreateAsync(PhoneCreateViewModel model);
        // По желание: EditAsync, DeleteAsync
    }

}
