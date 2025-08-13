using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PhoneXchange.Common.Tests;
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Services.Core;
using PhoneXchange.Tests.Common;
using PhoneXchange.Web.ViewModels.Message;

namespace PhoneXchange.Services.Tests
{
    [TestFixture]
    public class MessageServiceTests
    {
        [Test]
        public void SendAsync_Throws_When_SelfMessage()
        {
            var msgRepo = new Mock<IMessageRepository>(MockBehavior.Strict);
            var userMgr = UserManagerMock.Create();

            var svc = new MessageService(msgRepo.Object, userMgr.Object);
            var model = new MessageCreateViewModel { RecipientId = "u-1", Content = "hi", AdId = 1 };

            Assert.ThrowsAsync<System.InvalidOperationException>(async () => await svc.SendAsync("u-1", model));
        }

        [Test]
        public void SendAsync_Throws_When_Recipient_NotFound()
        {
            var msgRepo = new Mock<IMessageRepository>(MockBehavior.Strict);
            var userMgr = UserManagerMock.Create();
            userMgr.Setup(x => x.FindByIdAsync("u-2")).ReturnsAsync((ApplicationUser?)null);

            var svc = new MessageService(msgRepo.Object, userMgr.Object);
            var model = new MessageCreateViewModel { RecipientId = "u-2", Content = "hi", AdId = 1 };

            Assert.ThrowsAsync<System.InvalidOperationException>(async () => await svc.SendAsync("u-1", model));
        }

        [Test]
        public async Task SendAsync_Adds_When_Recipient_Exists()
        {
            using var db = TestDb.NewContext();

            var msgRepo = new Mock<IMessageRepository>(MockBehavior.Strict);
            var userMgr = UserManagerMock.Create();
            userMgr.Setup(x => x.FindByIdAsync("u-2")).ReturnsAsync(new ApplicationUser { Id = "u-2", Email = "u-2@ex.com" });

            msgRepo.Setup(r => r.AddAsync(It.IsAny<Message>()))
                   .Returns(async (Message m) => { await db.AddAsync(m); await db.SaveChangesAsync(); });

            var svc = new MessageService(msgRepo.Object, userMgr.Object);
            var model = new MessageCreateViewModel { RecipientId = "u-2", Content = "hi", AdId = 5 };

            await svc.SendAsync("u-1", model);

            Assert.That(await db.Set<Message>().CountAsync(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetMessagesAsync_Returns_Inbox()
        {
            using var db = TestDb.NewContext();

            // users
            var sender = new ApplicationUser { Id = "u-9", Email = "u-9@ex.com", UserName = "u-9@ex.com", EmailConfirmed = true };
            var recipient = new ApplicationUser { Id = "u-1", Email = "u-1@ex.com", UserName = "u-1@ex.com", EmailConfirmed = true };
            db.AddRange(sender, recipient);

            // message (не е нужно да създаваме Ad в InMemory; AdId е достатъчно)
            db.Add(new Message
            {
                Id = 1,
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                Content = "hello",
                AdId = 123
            });

            await db.SaveChangesAsync();

            var msgRepo = new Mock<IMessageRepository>(MockBehavior.Strict);
            // КРИТИЧНО: в теста връщаме IQueryable със зареден Sender
            msgRepo.Setup(r => r.GetAllAttached())
                   .Returns(db.Set<Message>().Include(m => m.Sender).AsQueryable());

            var userMgr = UserManagerMock.Create(); // не се ползва тук, но е нужен за конструктора
            var svc = new MessageService(msgRepo.Object, userMgr.Object);

            var list = await svc.GetMessagesAsync(recipient.Id);

            Assert.That(list, Has.Count.EqualTo(1));
            Assert.That(list[0].OtherUserEmail, Is.EqualTo(sender.Email));
            Assert.That(list[0].Content, Is.EqualTo("hello"));
        }



        [Test]
        public async Task GetSentMessagesAsync_Returns_Sent()
        {
            using var db = TestDb.NewContext();

            var sender = new ApplicationUser { Id = "u-1", Email = "u-1@ex.com", UserName = "u-1@ex.com", EmailConfirmed = true };
            var recipient = new ApplicationUser { Id = "u-9", Email = "u-9@ex.com", UserName = "u-9@ex.com", EmailConfirmed = true };
            db.AddRange(sender, recipient);

            db.Add(new Message
            {
                Id = 2,
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                Content = "hello",
                AdId = 456
            });

            await db.SaveChangesAsync();

            var msgRepo = new Mock<IMessageRepository>(MockBehavior.Strict);
            // Тук ни трябва Recipient
            msgRepo.Setup(r => r.GetAllAttached())
                   .Returns(db.Set<Message>().Include(m => m.Recipient).AsQueryable());

            var userMgr = UserManagerMock.Create();
            var svc = new MessageService(msgRepo.Object, userMgr.Object);

            var list = await svc.GetSentMessagesAsync(sender.Id);

            Assert.That(list, Has.Count.EqualTo(1));
            Assert.That(list[0].OtherUserEmail, Is.EqualTo(recipient.Email));
            Assert.That(list[0].Content, Is.EqualTo("hello"));
        }

    }
}
