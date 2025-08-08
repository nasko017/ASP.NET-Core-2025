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
            // 1) Миграции
            await _context.Database.MigrateAsync();

            // 2) Роли
            await EnsureRolesAsync("Admin", "User");

            // 3) Админ акаунт (create-if-missing, без reset ако вече съществува)
            await EnsureAdminAsync(
                email: "admin@phones.com",
                password: "Admin!23456");

            // 4) Брандове
            await SeedBrandsAsync();

            // 5) Демо обяви
            await SeedAdsAsync();
        }

        private async Task EnsureRolesAsync(params string[] roles)
        {
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task EnsureAdminAsync(string email, string password)
        {
            var admin = await _userManager.FindByEmailAsync(email);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    Email = email,
                    UserName = email,   // важно: username = email
                    EmailConfirmed = true
                };

                var create = await _userManager.CreateAsync(admin, password);
                if (!create.Succeeded)
                    throw new InvalidOperationException("Failed to create admin: " +
                        string.Join("; ", create.Errors.Select(e => e.Description)));
            }

            if (!await _userManager.IsInRoleAsync(admin, "Admin"))
                await _userManager.AddToRoleAsync(admin, "Admin");
        }

        private async Task SeedBrandsAsync()
        {
            if (await _context.Brands.AnyAsync()) return;

            await _context.Brands.AddRangeAsync(new[]
            {
                new Brand { Name = "Apple" },
                new Brand { Name = "Samsung" },
                new Brand { Name = "Xiaomi" }
            });

            await _context.SaveChangesAsync();
        }

        private async Task SeedAdsAsync()
        {
            if (await _context.Ads.AnyAsync()) return;

            var owner = await _userManager.FindByEmailAsync("admin@phones.com");
            var apple = await _context.Brands.FirstAsync(b => b.Name == "Apple");
            var samsung = await _context.Brands.FirstAsync(b => b.Name == "Samsung");
            var xiaomi = await _context.Brands.FirstAsync(b => b.Name == "Xiaomi");

            var now = DateTime.UtcNow;

            var ads = new List<Ad>
            {
                new Ad
                {
                    Title = "iPhone 14 Pro",
                    Description = "Чисто нов с гаранция",
                    Price = 1999,
                    OwnerId = owner!.Id,
                    CreatedOn = now,
                    Phone = new Phone
                    {
                        Model = "iPhone 14 Pro",
                        OS = "iOS",
                        IsNew = true,
                        BrandId = apple.Id,
                        ImageUrlsSerialized = ImageUrlHelper.Serialize(new List<string>
                        {
                            "https://via.placeholder.com/400x250"
                        })
                    }
                },
                new Ad
                {
                    Title = "Samsung Galaxy S22",
                    Description = "Отлично състояние, с кутия и зарядно",
                    Price = 1599,
                    OwnerId = owner!.Id,
                    CreatedOn = now,
                    Phone = new Phone
                    {
                        Model = "Galaxy S22",
                        OS = "Android",
                        IsNew = false,
                        BrandId = samsung.Id,
                        ImageUrlsSerialized = ImageUrlHelper.Serialize(new List<string>
                        {
                            "https://via.placeholder.com/400x250"
                        })
                    }
                },
                new Ad
                {
                    Title = "Xiaomi Redmi Note 12",
                    Description = "Нов, отворена опаковка. Android 13",
                    Price = 499,
                    OwnerId = owner!.Id,
                    CreatedOn = now,
                    Phone = new Phone
                    {
                        Model = "Redmi Note 12",
                        OS = "Android 13",
                        IsNew = true,
                        BrandId = xiaomi.Id,
                        ImageUrlsSerialized = ImageUrlHelper.Serialize(new List<string>
                        {
                            "https://via.placeholder.com/400x250"
                        })
                    }
                }
            };

            await _context.Ads.AddRangeAsync(ads);
            await _context.SaveChangesAsync();
        }
    }
}
