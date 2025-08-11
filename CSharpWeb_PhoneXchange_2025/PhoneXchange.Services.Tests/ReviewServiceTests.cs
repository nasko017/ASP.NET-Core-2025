using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PhoneXchange.Common.Tests;
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Services.Core;
using PhoneXchange.Web.ViewModels.Review;

namespace PhoneXchange.Services.Tests
{
    [TestFixture]
    public class ReviewServiceTests
    {
        [Test]
        public async Task AddReviewAsync_Adds_Entity()
        {
            var repo = new Mock<IReviewRepository>(MockBehavior.Strict);
            Review? captured = null;

            repo.Setup(r => r.AddAsync(It.IsAny<Review>()))
                .Callback<Review>(r => captured = r)
                .Returns(Task.CompletedTask);

            var svc = new ReviewService(repo.Object);

            var vm = new ReviewCreateViewModel { AdId = 1, Comment = "ok", Rating = 4 };
            await svc.AddReviewAsync(vm, "u-1");

            Assert.IsNotNull(captured);
            Assert.That(captured!.AdId, Is.EqualTo(1));
            Assert.That(captured.Rating, Is.EqualTo(4));
            Assert.That(captured.AuthorId, Is.EqualTo("u-1"));
        }

        [Test]
        public async Task GetReviewsForAdAsync_Projects_To_VMs()
        {
            using var db = TestDb.NewContext();
            db.Set<Review>().Add(new Review
            {
                Id = 1,
                AdId = 1,
                Comment = "good",
                Rating = 5,
                Author = new ApplicationUser { Email = "x@y.z" }
            });
            await db.SaveChangesAsync();

            var repo = new Mock<IReviewRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetAllAttached()).Returns(db.Set<Review>().AsQueryable());

            var svc = new ReviewService(repo.Object);
            var vms = await svc.GetReviewsForAdAsync(1);

            Assert.That(vms.Count(), Is.EqualTo(1));
            Assert.That(vms.First().Rating, Is.EqualTo(5));
            Assert.That(vms.First().AuthorEmail, Is.EqualTo("x@y.z"));
        }
    }
}
