using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Web.Data;

namespace PhoneXchange.Data.Repository
{
    public class FavoriteAdRepository : BaseRepository<FavoriteAd, int>, IFavoriteAdRepository
    {
        public FavoriteAdRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
