using Microsoft.AspNetCore.Mvc;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.Message;
using System.Security.Claims;

namespace PhoneXchange.Web.Controllers
{
    public class MessagesController:BaseController
    {
        private readonly IMessageService messageService;

        public MessagesController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> Inbox()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var messages = await messageService.GetMessagesAsync(userId);
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Send(MessageCreateViewModel model)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await messageService.SendAsync(senderId!, model);
            return RedirectToAction("Details", "Ads", new { id = model.AdId });
        }
    }
}
