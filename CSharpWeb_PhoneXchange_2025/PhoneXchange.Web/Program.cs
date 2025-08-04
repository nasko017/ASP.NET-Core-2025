using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhoneXchange.Data.Seeding.Interfaces;
using PhoneXchange.Data.Seeding;
using PhoneXchange.Web.Data;
using PhoneXchange.Data.Repository.Interfaces;
using PhoneXchange.Data.Repository;
using PhoneXchange.Services.Core.Interfaces;
using PhoneXchange.Services.Core;
using System.Globalization;
using PhoneXchange.Data.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace PhoneXchange.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
            })
              .AddRoles<IdentityRole>() // ако използваш роли
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();


            builder.Services.AddScoped<ISeeder, ApplicationDbSeeder>();

            builder.Services.AddScoped<IAdRepository, AdRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IPhoneRepository, PhoneRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();


            builder.Services.AddScoped<IAdService, AdService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<IPhoneService, PhoneService>();
            builder.Services.AddScoped<IMessageService, MessageService>();





            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
                await seeder.SeedAsync();
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();



            app.Run();
        }
    }
}
