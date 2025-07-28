
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Web.Data;

namespace PhoneXchange.Data.Repository
{
    public class AdRepository : BaseRepository<Ad, int>, IAdRepository
    {
        public AdRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
