using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhoneXchange.Data.Models;
using PhoneXchange.Data.Seeding.Interfaces;
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

            // 1. Roles
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. User
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
                    // FullName = "Test User" // Ако искаш
                };

                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, "User");
            }

            // 3. Brands
            if (!_context.Brands.Any())
            {
                var apple = new Brand { Name = "Apple" };
                var samsung = new Brand { Name = "Samsung" };

                await _context.Brands.AddRangeAsync(apple, samsung);
                await _context.SaveChangesAsync();

                // 4. Ads + Phone + Image
                if (!_context.Ads.Any())
                {
                    var ad = new Ad
                    {
                        Title = "iPhone 14 Pro",
                        Description = "Чисто нов с гаранция",
                        Price = 1999,
                        OwnerId = user.Id,
                        CreatedOn = DateTime.UtcNow
                    };

                    var phone = new Phone
                    {
                        Model = "iPhone 14 Pro",
                        OS = "iOS",
                        IsNew = true,
                        Brand = apple // ВАЖНО: директно задаваме обекта, не BrandId
                    };

                    var image = new PhoneImage
                    {
                        ImageUrl = "https://via.placeholder.com/400x250"
                    };

                    phone.Images.Add(image);
                    ad.Phone = phone;

                    await _context.Ads.AddAsync(ad);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
