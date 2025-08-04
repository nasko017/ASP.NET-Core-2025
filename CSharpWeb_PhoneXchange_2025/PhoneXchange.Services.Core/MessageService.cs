using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public MessageService(IMessageRepository messageRepository, UserManager<ApplicationUser> userManager)
        {
            this.messageRepository = messageRepository;
            this.userManager = userManager;
        }

        public async Task SendAsync(string senderId, MessageCreateViewModel model)
        {
            if (senderId == model.RecipientId)
                throw new InvalidOperationException("Не може да изпращате съобщение до себе си.");

            var recipient = await userManager.FindByIdAsync(model.RecipientId);
            if (recipient == null)
                throw new InvalidOperationException("Получателят не съществува.");

            var message = new Message
            {
                SenderId = senderId,
                RecipientId = model.RecipientId,
                Content = model.Content,
                SentOn = DateTime.UtcNow,
                AdId = model.AdId
            };

            await messageRepository.AddAsync(message);
        }

        public async Task<List<MessageViewModel>> GetMessagesAsync(string userId)
        {
            var messages = await messageRepository
                .GetAllAttached()
                .Where(m => m.RecipientId == userId)
                .Include(m => m.Sender)
                .OrderByDescending(m => m.SentOn)
                .ToListAsync();

            return messages.Select(m => new MessageViewModel
            {
                OtherUserEmail = m.Sender?.Email ?? "Непознат",
                Content = m.Content,
                SentOn = m.SentOn
            }).ToList();
        }

        public async Task<List<MessageViewModel>> GetSentMessagesAsync(string userId)
        {
            var messages = await messageRepository
                .GetAllAttached()
                .Where(m => m.SenderId == userId)
                .Include(m => m.Recipient)
                .OrderByDescending(m => m.SentOn)
                .ToListAsync();

            return messages.Select(m => new MessageViewModel
            {
                OtherUserEmail = m.Recipient?.Email ?? "Получател неизвестен",
                Content = m.Content,
                SentOn = m.SentOn
            }).ToList();
        }
    }
}
