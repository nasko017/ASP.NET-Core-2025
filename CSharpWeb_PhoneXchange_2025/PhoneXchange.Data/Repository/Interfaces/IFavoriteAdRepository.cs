using PhoneXchange.Data.Models;

namespace PhoneXchange.Data.Repository.Interfaces
{
    public interface IFavoriteAdRepository : IRepository<FavoriteAd, int>, IAsyncRepository<FavoriteAd, int>
    {
    }
}
