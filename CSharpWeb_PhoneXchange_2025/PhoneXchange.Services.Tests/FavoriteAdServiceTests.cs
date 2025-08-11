using Microsoft.EntityFrameworkCore;
using Moq;
using PhoneXchange.Common.Tests;
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.GCommon.Helpers;
using PhoneXchange.Services.Core;
using System.Linq.Expressions;
using System;

namespace PhoneXchange.Services.Tests
{
    [TestFixture]
    public class FavoriteAdServiceTests
    {
        [Test]
        public async Task IsFavoriteAsync_Returns_True_When_Existing()
        {
            using var db = TestDb.NewContext();
            db.Set<FavoriteAd>().Add(new FavoriteAd { ApplicationUserId = "u-1", AdId = 3 });
            await db.SaveChangesAsync();

            var favRepo = new Mock<IFavoriteAdRepository>(MockBehavior.Strict);
            favRepo.Setup(r => r.GetAllAttached()).Returns(db.Set<FavoriteAd>().AsQueryable());

            var adRepo = new Mock<IAdRepository>(MockBehavior.Strict);
            var svc = new FavoriteAdService(favRepo.Object, adRepo.Object);

            var isFav = await svc.IsFavoriteAsync("u-1", 3);
            Assert.That(isFav, Is.True);
        }

        [Test]
        public async Task AddToFavoritesAsync_Adds_When_NotExisting()
        {
            using var db = TestDb.NewContext();

            var favRepo = new Mock<IFavoriteAdRepository>(MockBehavior.Strict);
            favRepo.Setup(r => r.GetAllAttached()).Returns(db.Set<FavoriteAd>().AsQueryable());
            favRepo.Setup(r => r.AddAsync(It.IsAny<FavoriteAd>()))
                   .Returns(async (FavoriteAd f) => { await db.AddAsync(f); await db.SaveChangesAsync(); });

            var adRepo = new Mock<IAdRepository>(MockBehavior.Strict);
            var svc = new FavoriteAdService(favRepo.Object, adRepo.Object);

            await svc.AddToFavoritesAsync("u-1", 1);

            Assert.That(await db.Set<FavoriteAd>().CountAsync(), Is.EqualTo(1));
        }

        [Test]
        public async Task RemoveFromFavoritesAsync_Deletes_When_Existing()
        {
            using var db = TestDb.NewContext();
            var existing = new FavoriteAd { ApplicationUserId = "u-1", AdId = 1 };
            db.Set<FavoriteAd>().Add(existing);
            await db.SaveChangesAsync();

            var favRepo = new Mock<IFavoriteAdRepository>(MockBehavior.Strict);

            // съвпадение по предикат: връщаме наличния в паметта
            favRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<FavoriteAd, bool>>>()))
                   .ReturnsAsync((Expression<Func<FavoriteAd, bool>> pred) =>
                        db.Set<FavoriteAd>().AsEnumerable().FirstOrDefault(pred.Compile()));

            favRepo.Setup(r => r.HardDeleteAsync(existing))
                   .Returns(async () => { db.Remove(existing); await db.SaveChangesAsync(); return true; });

            var adRepo = new Mock<IAdRepository>(MockBehavior.Strict);
            var svc = new FavoriteAdService(favRepo.Object, adRepo.Object);

            await svc.RemoveFromFavoritesAsync("u-1", 1);

            Assert.That(await db.Set<FavoriteAd>().CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetFavoritesByUserAsync_Projects_To_ViewModels()
        {
            using var db = TestDb.NewContext();

            var ad = new Ad
            {
                Id = 10,
                Title = "iPhone 14",
                Price = 999,
                Phone = new Phone { ImageUrlsSerialized = ImageUrlHelper.Serialize(new List<string> { "https://img" }) }
            };
            db.Set<Ad>().Add(ad);
            db.Set<FavoriteAd>().Add(new FavoriteAd { ApplicationUserId = "u-1", AdId = 10, Ad = ad });
            await db.SaveChangesAsync();

            var favRepo = new Mock<IFavoriteAdRepository>(MockBehavior.Strict);

            // ВАЖНО: върни IQueryable със заредени навигации
            var q = db.Set<FavoriteAd>()
                      .Include(f => f.Ad)
                      .ThenInclude(a => a.Phone)
                      .AsQueryable();

            favRepo.Setup(r => r.GetAllAttached()).Returns(q);

            var adRepo = new Mock<IAdRepository>(MockBehavior.Strict);
            var svc = new FavoriteAdService(favRepo.Object, adRepo.Object);

            var list = await svc.GetFavoritesByUserAsync("u-1");

            Assert.That(list.Count(), Is.EqualTo(1));
            Assert.That(list.First().Title, Is.EqualTo("iPhone 14"));
            Assert.That(list.First().ImageUrl, Is.EqualTo("https://img"));
        }
    }
}
