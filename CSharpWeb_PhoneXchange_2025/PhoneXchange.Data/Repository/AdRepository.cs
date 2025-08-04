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

        //public override async ValueTask<Ad?> GetByIdAsync(int id)
        //{
        //    return await context.Ads
        //        .Include(a => a.Phone)
        //        .FirstOrDefaultAsync(a => a.Id == id);
        //}
        public async Task<Ad?> GetByIdWithDetailsAsync(int id)
        {
            return await context.Ads
                .Include(a => a.Phone)
                    .ThenInclude(p => p.Brand)
                .Include(a => a.Reviews)
                    .ThenInclude(r => r.Author)
                .FirstOrDefaultAsync(a => a.Id == id);
        }


    }
}
