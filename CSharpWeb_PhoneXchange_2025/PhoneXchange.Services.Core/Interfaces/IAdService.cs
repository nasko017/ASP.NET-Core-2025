
using PhoneXchange.Data.Models;
using PhoneXchange.Web.ViewModels.Ad;

namespace PhoneXchange.Services.Core.Interfaces
{
    public interface IAdService
    {
        Task<IEnumerable<AdViewModel>> GetAllAsync();
        Task<AdViewModel?> GetByIdAsync(int id);
        Task<AdDetailsViewModel?> GetDetailsByIdAsync(int id);
        Task<IEnumerable<AdListViewModel>> GetAdsByUserAsync(string userId);
        Task<AdsListViewModel> GetFilteredAdsAsync(string? searchTerm, int page, int pageSize);
        Task CreateAsync(AdCreateViewModel model, string userId);
        Task EditAsync(AdEditViewModel model);
        Task DeleteAsync(int id);
    }
}
