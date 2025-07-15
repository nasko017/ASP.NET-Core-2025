using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhoneXchange.Data.Models;
using System.Numerics;

namespace PhoneXchange.Web.Data
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
        public virtual DbSet<FavoriteAd> FavoriteAds { get; set; }
        public virtual DbSet<ApplicationUserAd> ApplicationUserAds { get; set; }
        public virtual DbSet<PhoneImage> PhoneImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === Ad ===
            modelBuilder.Entity<Ad>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Title).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Description).IsRequired().HasMaxLength(1000);
                entity.Property(a => a.Price).IsRequired();
                entity.Property(a => a.CreatedOn).IsRequired();
                entity.Property(a => a.IsDeleted).IsRequired();

                entity.HasOne(a => a.Owner)
                      .WithMany()
                      .HasForeignKey(a => a.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Phone)
                      .WithOne(p => p.Ad)
                      .HasForeignKey<Phone>(p => p.AdId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Optional global query filter
                entity.HasQueryFilter(a => !a.IsDeleted);
            });

            // === Message ===
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Content).IsRequired().HasMaxLength(500);
                entity.Property(m => m.SentOn).IsRequired();
                entity.Property(m => m.IsDeleted).IsRequired();

                entity.HasOne(m => m.Sender)
                      .WithMany()
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Ad)
                      .WithMany(a => a.Messages)
                      .HasForeignKey(m => m.AdId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Optional global query filter
                entity.HasQueryFilter(m => !m.IsDeleted);
            });

            // === Phone ===
            modelBuilder.Entity<Phone>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Model).IsRequired().HasMaxLength(50);
                entity.Property(p => p.OS).IsRequired().HasMaxLength(50);
                entity.Property(p => p.IsNew).IsRequired();

                entity.HasOne(p => p.Brand)
                      .WithMany(b => b.Phones)
                      .HasForeignKey(p => p.BrandId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // === PhoneImage ===
            modelBuilder.Entity<PhoneImage>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.ImageUrl).IsRequired();

                entity.HasOne(i => i.Phone)
                      .WithMany(p => p.Images)
                      .HasForeignKey(i => i.PhoneId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // === Brand ===
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Name).IsRequired().HasMaxLength(50);

                entity.HasIndex(b => b.Name).IsUnique();
            });

            // === FavoriteAd (composite key) ===
            modelBuilder.Entity<FavoriteAd>(entity =>
            {
                entity.HasKey(f => new { f.ApplicationUserId, f.AdId });

                entity.Property(f => f.FavoritedOn).IsRequired();

                entity.HasOne(f => f.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(f => f.ApplicationUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Ad)
                      .WithMany(a => a.Favorites)
                      .HasForeignKey(f => f.AdId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // === ApplicationUserAd (composite key) ===
            modelBuilder.Entity<ApplicationUserAd>(entity =>
            {
                entity.HasKey(p => new { p.ApplicationUserId, p.AdId });

                entity.Property(p => p.PurchasedOn).IsRequired();

                entity.HasOne(p => p.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(p => p.ApplicationUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Ad)
                      .WithMany() // Ad няма Purchases колекция
                      .HasForeignKey(p => p.AdId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
