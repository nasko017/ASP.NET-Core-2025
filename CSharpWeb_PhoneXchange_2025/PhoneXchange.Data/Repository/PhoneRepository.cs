using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Web.Data;

namespace PhoneXchange.Data.Repository
{
    public class PhoneRepository : BaseRepository<Phone, int>, IPhoneRepository
    {
        public PhoneRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
