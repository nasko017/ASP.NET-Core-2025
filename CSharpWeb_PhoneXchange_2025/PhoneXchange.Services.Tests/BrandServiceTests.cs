using Moq;
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Services.Core;

namespace PhoneXchange.Services.Tests
{
    [TestFixture]
    public class BrandServiceTests
    {
        [Test]
        public async Task GetAllAsync_Maps_To_ViewModels()
        {
            var brands = new List<Brand>
            {
                new Brand{ Id=1, Name="Apple"},
                new Brand{ Id=2, Name="Samsung"}
            };

            var repo = new Mock<IBrandRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetAllAsync()).ReturnsAsync(brands);

            var svc = new BrandService(repo.Object);
            var result = await svc.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(x => x.Name == "Apple"));
        }
    }
}
