using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhoneXchange.Data.Models;

namespace PhoneXchange.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ad> Ads { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Ad>()
            .HasOne(a => a.Phone)
            .WithMany(p => p.Ads)
            .HasForeignKey(a => a.PhoneId)
            .OnDelete(DeleteBehavior.Cascade);

            // 👤 Ad - IdentityUser (много обяви към един потребител)
            builder.Entity<Ad>()
                .HasOne(a => a.Seller)
                .WithMany()
                .HasForeignKey(a => a.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 📩 Message - Ad (много съобщения към една обява)
            builder.Entity<Message>()
                .HasOne(m => m.Ad)
                .WithMany(a => a.Messages)
                .HasForeignKey(m => m.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            // 👤 Message - IdentityUser (много съобщения от един потребител)
            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // 📞 Phone - Brand (много телефони към една марка)
            builder.Entity<Phone>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Phones)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
