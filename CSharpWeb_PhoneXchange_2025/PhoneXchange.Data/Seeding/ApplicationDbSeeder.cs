using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Seeding.Interfaces;
using PhoneXchange.GCommon.Helpers;
using PhoneXchange.Web.Data;

namespace PhoneXchange.Data.Seeding
{
    public class ApplicationDbSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbSeeder(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();
        
            // 1. Създаване на роли
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        
            // 2. Създаване на потребител
            string email = "test@phones.com";
            string password = "123456";
            var user = await _userManager.FindByEmailAsync(email);
        
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "testuser",
                    Email = email,
                    EmailConfirmed = true
                };
        
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, "User");
            }
        
            // 3. Добавяне на марки
            var apple = await _context.Brands.FirstOrDefaultAsync(b => b.Name == "Apple");
            if (apple == null)
            {
                apple = new Brand { Name = "Apple" };
                await _context.Brands.AddAsync(apple);
            }
        
            var samsung = await _context.Brands.FirstOrDefaultAsync(b => b.Name == "Samsung");
            if (samsung == null)
            {
                samsung = new Brand { Name = "Samsung" };
                await _context.Brands.AddAsync(samsung);
            }
        
            var xiaomi = await _context.Brands.FirstOrDefaultAsync(b => b.Name == "Xiaomi");
            if (xiaomi == null)
            {
                xiaomi = new Brand { Name = "Xiaomi" };
                await _context.Brands.AddAsync(xiaomi);
            }
        
            await _context.SaveChangesAsync();
        
            // 4. Добавяне на обяви
            if (!_context.Ads.Any())
            {
                var ads = new List<Ad>
                {
                    new Ad
                    {
                        Title = "iPhone 14 Pro",
                        Description = "Чисто нов с гаранция",
                        Price = 1999,
                        OwnerId = user.Id,
                        CreatedOn = DateTime.UtcNow,
                        Phone = new Phone
                        {
                            Model = "iPhone 14 Pro",
                            OS = "iOS",
                            IsNew = true,
                            BrandId = apple.Id,
                            ImageUrlsSerialized = ImageUrlHelper.Serialize(new List<string> { "https://via.placeholder.com/400x250" })
                        }
                    },
                    new Ad
                    {
                        Title = "Samsung Galaxy S22",
                        Description = "Отлично състояние, с кутия и зарядно",
                        Price = 1599,
                        OwnerId = user.Id,
                        CreatedOn = DateTime.UtcNow,
                        Phone = new Phone
                        {
                            Model = "Galaxy S22",
                            OS = "Android",
                            IsNew = false,
                            BrandId = samsung.Id,
                            ImageUrlsSerialized = ImageUrlHelper.Serialize(new List<string> { "https://via.placeholder.com/400x250" })
                        }
                    },
                    new Ad
                    {
                        Title = "Xiaomi Redmi Note 12",
                        Description = "Нов, отворена опаковка. Android 13",
                        Price = 499,
                        OwnerId = user.Id,
                        CreatedOn = DateTime.UtcNow,
                        Phone = new Phone
                        {
                            Model = "Redmi Note 12",
                            OS = "Android 13",
                            IsNew = true,
                            BrandId = xiaomi.Id,
                            ImageUrlsSerialized = ImageUrlHelper.Serialize(new List<string> { "https://via.placeholder.com/400x250" })
                        }
                    }
                };
        
                await _context.Ads.AddRangeAsync(ads);
                await _context.SaveChangesAsync();
            }
        }
    }
}
