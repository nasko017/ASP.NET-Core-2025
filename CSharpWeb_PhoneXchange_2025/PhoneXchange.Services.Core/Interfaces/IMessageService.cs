using PhoneXchange.Web.ViewModels.Message;

namespace PhoneXchange.Services.Core.Interfaces
{
    public interface IMessageService
    {
        Task SendAsync(string senderId, MessageCreateViewModel model);
        Task<List<MessageViewModel>> GetMessagesAsync(string userId);
        Task<List<MessageViewModel>> GetSentMessagesAsync(string userId);

    }
}
