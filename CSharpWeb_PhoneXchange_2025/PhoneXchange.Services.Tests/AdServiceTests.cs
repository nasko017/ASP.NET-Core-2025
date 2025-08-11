using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PhoneXchange.Common.Tests;
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.GCommon.Helpers;
using PhoneXchange.Services.Core;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Web.Data;
using PhoneXchange.Web.ViewModels.Ad;

namespace PhoneXchange.Services.Tests
{
    [TestFixture]
    public class AdServiceTests
    {
        private static void SeedAds(ApplicationDbContext db, IEnumerable<Ad> ads)
        {
            db.Set<Ad>().AddRange(ads);
            db.SaveChanges();
        }

        private static List<Ad> MakeAds(int count = 10, string ownerA = "owner-x", string ownerB = "other-y")
        {
            var list = new List<Ad>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Ad
                {
                    Id = i,
                    Title = i % 2 == 0 ? $"iPhone {i}" : $"Galaxy {i}",
                    Description = i % 2 == 0 ? "Fast and shiny" : "Big and bright",
                    Price = 400 + i,
                    OwnerId = i % 2 == 0 ? ownerA : ownerB,
                    CreatedOn = DateTime.UtcNow.AddMinutes(-i),
                    Phone = new Phone
                    {
                        Id = i,
                        Model = i % 2 == 0 ? "iPhone" : "Galaxy",
                        OS = i % 2 == 0 ? "iOS" : "Android",
                        BrandId = i % 2 == 0 ? 1 : 2,
                        ImageUrlsSerialized = ImageUrlHelper.Serialize(new List<string> { $"https://img{i}" })
                    },
                    Reviews = new List<Review>()
                });
            }
            return list;
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_When_NotFound()
        {
            var repo = new Mock<IAdRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetByIdAsync(999)).Returns(new ValueTask<Ad?>(result: null));

            IAdService service = new AdService(repo.Object);

            var vm = await service.GetByIdAsync(999);
            Assert.IsNull(vm);
        }

        [Test]
        public async Task CreateAsync_Maps_And_Adds()
        {
            Ad? captured = null;
            var repo = new Mock<IAdRepository>(MockBehavior.Strict);
            repo.Setup(r => r.AddAsync(It.IsAny<Ad>()))
                .Callback<Ad>(a => captured = a)
                .Returns(Task.CompletedTask);

            IAdService service = new AdService(repo.Object);

            var vm = new AdCreateViewModel
            {
                Title = "Nice phone",
                Description = "Like new",
                Price = 777,
                BrandId = 1,
                Model = "iPhone 15",
                OS = "iOS",
                IsNew = true,
                ImageUrls = new List<string> { "https://img1", "https://img2" }
            };

            await service.CreateAsync(vm, "u-1");

            Assert.IsNotNull(captured);
            Assert.That(captured!.Title, Is.EqualTo(vm.Title));
            Assert.That(captured.OwnerId, Is.EqualTo("u-1"));
            Assert.That(captured.Price, Is.EqualTo(777));
            Assert.That(captured.Phone!.Model, Is.EqualTo("iPhone 15"));
            Assert.That(captured.Phone.ImageUrlsSerialized, Is.EqualTo(ImageUrlHelper.Serialize(vm.ImageUrls)));
        }

        [Test]
        public async Task GetAdsByUserAsync_Returns_Only_User_Ads()
        {
            using var db = TestDb.NewContext();
            SeedAds(db, MakeAds(10, ownerA: "owner-x", ownerB: "other-y"));

            var repo = new Mock<IAdRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetAllAttached()).Returns(db.Set<Ad>().AsQueryable());

            IAdService service = new AdService(repo.Object);

            var list = await service.GetAdsByUserAsync("owner-x");

            Assert.That(list.Count(), Is.EqualTo(5));
            Assert.That(list.Any());
        }

        [Test]
        public async Task EditAsync_Updates_Title_Price_And_Images()
        {
            var ad = new Ad
            {
                Id = 1,
                Title = "Old",
                Description = "Old",
                Price = 1,
                Phone = new Phone { ImageUrlsSerialized = ImageUrlHelper.Serialize(new List<string>()) }
            };

            var repo = new Mock<IAdRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetByIdAsync(1)).Returns(new ValueTask<Ad?>(ad));
            repo.Setup(r => r.UpdateAsync(ad)).ReturnsAsync(true);

            IAdService service = new AdService(repo.Object);

            var editVm = new AdEditViewModel
            {
                Id = 1,
                Title = "Updated",
                Description = "Better",
                Price = 999,
                ImageUrls = new List<string> { "https://new" }
            };

            await service.EditAsync(editVm);

            Assert.That(ad.Title, Is.EqualTo("Updated"));
            Assert.That(ad.Description, Is.EqualTo("Better"));
            Assert.That(ad.Price, Is.EqualTo(999));
            Assert.That(ad.Phone!.ImageUrlsSerialized, Is.EqualTo(ImageUrlHelper.Serialize(editVm.ImageUrls)));
            repo.Verify(r => r.UpdateAsync(ad), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Deletes_When_Found()
        {
            using var db = TestDb.NewContext();
            var ad = MakeAds(1).First();
            db.Set<Ad>().Add(ad);
            await db.SaveChangesAsync();

            var repo = new Mock<IAdRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetByIdAsync(ad.Id)).Returns(new ValueTask<Ad?>(ad));
            repo.Setup(r => r.HardDeleteAsync(ad)).ReturnsAsync(true);

            IAdService service = new AdService(repo.Object);

            await service.DeleteAsync(ad.Id);

            repo.Verify(r => r.HardDeleteAsync(ad), Times.Once);
        }

        [Test]
        public async Task GetFilteredAdsAsync_Filters_And_Paginates()
        {
            using var db = TestDb.NewContext();
            SeedAds(db, MakeAds(21));

            var repo = new Mock<IAdRepository>(MockBehavior.Strict);
            repo.Setup(r => r.GetAllAttached()).Returns(db.Set<Ad>().AsQueryable());

            IAdService service = new AdService(repo.Object);

            var result = await service.GetFilteredAdsAsync("iPhone", page: 2, pageSize: 5);

            Assert.That(result.SearchTerm, Is.EqualTo("iPhone"));
            Assert.That(result.CurrentPage, Is.EqualTo(2));
            Assert.That(result.TotalPages, Is.GreaterThanOrEqualTo(2));
            Assert.That(result.Ads.Count(), Is.EqualTo(5));
            Assert.That(result.Ads.All(a => a.Title.Contains("iPhone") || a.Description.Contains("iPhone")));
        }

        [Test]
        public async Task GetDetailsByIdAsync_Returns_Details_With_Relations()
        {
            using var db = TestDb.NewContext();

            var brand = new Brand { Id = 1, Name = "Apple" };
            var owner = new ApplicationUser { Id = "u-1", Email = "owner@ex.com" };
            var ad = new Ad
            {
                Id = 100,
                Title = "iPhone 15 Pro",
                Description = "Mint",
                Price = 1500,
                OwnerId = owner.Id,
                Owner = owner,
                CreatedOn = DateTime.UtcNow,
                Phone = new Phone
                {
                    Id = 200,
                    Model = "iPhone 15 Pro",
                    OS = "iOS",
                    IsNew = true,
                    BrandId = 1,
                    Brand = brand,
                    ImageUrlsSerialized = ImageUrlHelper.Serialize(new List<string> { "https://img" })
                },
                Reviews = new List<Review>
                {
                    new Review
                    {
                        Id = 1, AdId = 100, Rating = 5, Comment = "Great",
                        Author = new ApplicationUser { Email = "a@b.c" },
                        CreatedOn = DateTime.UtcNow
                    }
                }
            };

            db.Set<Brand>().Add(brand);
            db.Set<ApplicationUser>().Add(owner);
            db.Set<Ad>().Add(ad);
            await db.SaveChangesAsync();

            var repo = new Mock<IAdRepository>(MockBehavior.Strict);
            // СЕРВИЗЪТ вече прави проекция през GetAllAttached()
            repo.Setup(r => r.GetAllAttached()).Returns(db.Set<Ad>().AsQueryable());

            IAdService service = new AdService(repo.Object);

            var vm = await service.GetDetailsByIdAsync(100);

            Assert.IsNotNull(vm);
            Assert.That(vm!.BrandName, Is.EqualTo("Apple"));
            Assert.That(vm.ReviewsCount, Is.EqualTo(1));
            Assert.That(vm.AvgRating, Is.EqualTo(5));
            Assert.That(vm.ImageUrls.First(), Is.EqualTo("https://img"));
        }
    }
}
