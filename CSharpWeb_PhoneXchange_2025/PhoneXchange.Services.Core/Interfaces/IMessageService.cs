using PhoneXchange.Web.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Services.Core.Interfaces
{
    public interface IMessageService
    {
        Task SendAsync(string senderId, MessageCreateViewModel model);
        Task<List<MessageViewModel>> GetMessagesAsync(string userId);
    }
}
