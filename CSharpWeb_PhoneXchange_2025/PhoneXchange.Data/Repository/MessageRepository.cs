using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Web.Data;

namespace PhoneXchange.Data.Repository
{
    public class MessageRepository : BaseRepository<Message, int>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
