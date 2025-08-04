using PhoneXchange.Data.Models;

namespace PhoneXchange.Data.Repository.Interfaces
{
    public interface IMessageRepository : IRepository<Message, int>, IAsyncRepository<Message, int>
    {
    }
}
