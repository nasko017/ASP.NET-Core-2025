using PhoneXchange.Data.Models;

namespace PhoneXchange.Data.Repository.Interfaces
{
    public interface IBrandRepository : IRepository<Brand, int>, IAsyncRepository<Brand, int>
    {
    }
}
