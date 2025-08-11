namespace PhoneXchange.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using PhoneXchange.Data.Models;

    public static class TestDataFactory
    {
        public static List<Ad> Ads(int count = 10, string ownerIdA = "u-1", string ownerIdB = "u-2")
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
                    OwnerId = i % 2 == 0 ? ownerIdA : ownerIdB,
                    CreatedOn = DateTime.UtcNow.AddMinutes(-i),
                    Phone = new Phone
                    {
                        Id = i,
                        Model = i % 2 == 0 ? "iPhone" : "Galaxy",
                        OS = i % 2 == 0 ? "iOS" : "Android",
                        BrandId = i % 2 == 0 ? 1 : 2,
                        ImageUrlsSerialized = "[\"https://img" + i + "\"]"
                    },
                    Reviews = new List<Review>()
                });
            }
            return list;
        }

        public static List<Brand> Brands() => new()
        {
            new Brand { Id = 1, Name = "Apple" },
            new Brand { Id = 2, Name = "Samsung" },
        };

        public static List<FavoriteAd> Favorites(string userId = "u-1") => new()
        {
            new FavoriteAd { ApplicationUserId = userId, AdId = 1, FavoritedOn = DateTime.UtcNow },
        };

        public static List<Message> Messages(string sender = "u-1", string recipient = "u-2") => new()
        {
            new Message { Id = 1, SenderId = sender, RecipientId = recipient, Content = "hi", SentOn = DateTime.UtcNow, AdId = 1,
                Sender = new ApplicationUser{ Id = sender, Email = sender + "@ex.com" },
                Recipient = new ApplicationUser{ Id = recipient, Email = recipient + "@ex.com" },
                Ad = new Ad{ Id = 1, Title = "iPhone 13" } }
        };

        public static List<Review> Reviews(int adId = 1, string authorId = "u-9") => new()
        {
            new Review { Id = 1, AdId = adId, AuthorId = authorId, Comment = "ok", Rating = 5, CreatedOn = DateTime.UtcNow, Author = new ApplicationUser{ Email="a@b.c" } }
        };
    }
}
