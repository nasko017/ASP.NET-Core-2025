using PhoneXchange.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Data.Repository.Interfaces
{
    public interface IMessageRepository : IRepository<Message, int>, IAsyncRepository<Message, int>
    {
    }
}
