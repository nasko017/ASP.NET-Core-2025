using PhoneXchange.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Data.Repository.Interfaces
{
    public interface IBrandRepository : IRepository<Brand, int>, IAsyncRepository<Brand, int>
    {
    }
}
