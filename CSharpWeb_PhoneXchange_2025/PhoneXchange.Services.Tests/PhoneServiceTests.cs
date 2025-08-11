using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.GCommon.Helpers;
using PhoneXchange.Services.Core;
using PhoneXchange.Web.ViewModels.Phone;

namespace PhoneXchange.Services.Tests
{
    [TestFixture]
    public class PhoneServiceTests
    {
        [Test]
        public async Task CreateAsync_Maps_And_Adds()
        {
            var repo = new Mock<IPhoneRepository>(MockBehavior.Strict);
            Phone? captured = null;

            repo.Setup(r => r.AddAsync(It.IsAny<Phone>()))
                .Callback<Phone>(p => captured = p)
                .Returns(Task.CompletedTask);

            var svc = new PhoneService(repo.Object);

            var vm = new PhoneCreateViewModel
            {
                Model = "Pixel 9",
                OS = "Android",
                BrandId = 2,
                IsNew = false,
                ImageUrls = new List<string> { "https://imgA", "https://imgB" }
            };

            await svc.CreateAsync(vm);

            Assert.IsNotNull(captured);
            Assert.That(captured!.Model, Is.EqualTo("Pixel 9"));
            Assert.That(captured.BrandId, Is.EqualTo(2));
            Assert.That(captured.ImageUrlsSerialized, Is.EqualTo(ImageUrlHelper.Serialize(vm.ImageUrls)));
        }
    }
}
