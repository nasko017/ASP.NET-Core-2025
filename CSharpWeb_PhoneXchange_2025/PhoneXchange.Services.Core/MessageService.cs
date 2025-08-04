using Microsoft.AspNetCore.Identity;
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.Message;
namespace PhoneXchange.Services.Core
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository messageRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public MessageService(IMessageRepository _messageRepository, UserManager<ApplicationUser> userManager)
        {
            this.messageRepository = _messageRepository;
            this.userManager = userManager;
        }

        public async Task SendAsync(string senderId, MessageCreateViewModel model)
        {
            var message = new Message
            {
                SenderId = senderId,
                RecipientId = model.RecipientId,
                Content = model.Content,
                SentOn = DateTime.UtcNow
            };

            await messageRepository.AddAsync(message);
        }

        public async Task<List<MessageViewModel>> GetMessagesAsync(string userId)
        {
            var messages = await messageRepository
                .GetAllAsync();

            return messages
                .Where(m => m.RecipientId == userId)
                .Select(m => new MessageViewModel
                {
                    SenderEmail = m.Sender?.Email ?? "Непознат",
                    Content = m.Content,
                    SentOn = m.SentOn
                })
                .ToList();
        }
    }

}
