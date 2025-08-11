using PhoneXchange.Data.Models;

namespace PhoneXchange.Data.Repository.Interfaces
{
    public interface IPhoneRepository : IRepository<Phone, int>, IAsyncRepository<Phone, int>
    {
    }
}
