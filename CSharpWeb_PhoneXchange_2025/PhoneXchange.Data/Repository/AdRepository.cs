using Microsoft.EntityFrameworkCore;
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Web.Data;

namespace PhoneXchange.Data.Repository
{
    public class AdRepository : BaseRepository<Ad, int>, IAdRepository
    {
        private readonly ApplicationDbContext context;

        public AdRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            context = dbContext;
        }


    }
}
