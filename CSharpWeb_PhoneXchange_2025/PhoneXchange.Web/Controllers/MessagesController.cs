using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.ViewModels.Message;
using System.Security.Claims;

namespace PhoneXchange.Web.Controllers
{
    [Authorize]
    public class MessagesController : BaseController
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

            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Потребителят не е автентикиран.";
                return RedirectToAction("Index", "Home");
            }

            var messages = await messageService.GetMessagesAsync(userId);

            return View(messages); // Очаква се View да е към List<MessageViewModel>
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(MessageCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Моля, попълнете всички полета правилно.";
                return RedirectToAction("Details", "Ads", new { id = model.AdId });
            }

            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(senderId))
            {
                TempData["Error"] = "Грешка при изпращането на съобщението.";
                return RedirectToAction("Details", "Ads", new { id = model.AdId });
            }

            try
            {
                await messageService.SendAsync(senderId, model);
                TempData["Success"] = "Съобщението беше изпратено успешно.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["Error"] = "Възникна неочаквана грешка при изпращането.";
            }

            return RedirectToAction("Details", "Ads", new { id = model.AdId });
        }
        [HttpGet]
        public async Task<IActionResult> Sent()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Потребителят не е автентикиран.";
                return RedirectToAction("Index", "Home");
            }

            var messages = await messageService.GetSentMessagesAsync(userId);
            return View(messages);
        }


    }
}
