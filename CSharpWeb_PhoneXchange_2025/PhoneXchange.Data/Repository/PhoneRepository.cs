using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Data.Repository
{
    public class PhoneRepository : BaseRepository<Phone, int>, IPhoneRepository
    {
        public PhoneRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
