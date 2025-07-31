using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhoneXchange.Data.Models;
using System.Numerics;

namespace PhoneXchange.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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

            // Ad
            modelBuilder.Entity<Ad>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Description)
                      .IsRequired();

                entity.Property(e => e.Price)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.CreatedOn)
                      .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.IsDeleted)
                      .HasDefaultValue(false);

                entity.HasOne(e => e.Owner)
                      .WithMany(u => u.Ads)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Phone)
                      .WithOne(p => p.Ad)
                      .HasForeignKey<Phone>(p => p.AdId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Messages)
                      .WithOne(m => m.Ad)
                      .HasForeignKey(m => m.AdId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Favorites)
                      .WithOne(f => f.Ad)
                      .HasForeignKey(f => f.AdId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FullName)
                      .HasMaxLength(100);

                entity.HasMany(u => u.Ads)
                      .WithOne(a => a.Owner)
                      .HasForeignKey(a => a.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.FavoriteAds)
                      .WithOne(f => f.ApplicationUser)
                      .HasForeignKey(f => f.ApplicationUserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Messages)
                      .WithOne(m => m.Sender)
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ApplicationUserAd
            modelBuilder.Entity<ApplicationUserAd>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationUserId, e.AdId });

                entity.HasOne(e => e.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(e => e.ApplicationUserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Ad)
                      .WithMany()
                      .HasForeignKey(e => e.AdId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.PurchasedOn)
                      .HasDefaultValueSql("getutcdate()");
            });

            // Brand
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasMany(b => b.Phones)
                      .WithOne(p => p.Brand)
                      .HasForeignKey(p => p.BrandId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // FavoriteAd
            modelBuilder.Entity<FavoriteAd>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationUserId, e.AdId });

                entity.HasOne(e => e.ApplicationUser)
                      .WithMany(u => u.FavoriteAds)
                      .HasForeignKey(e => e.ApplicationUserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Ad)
                      .WithMany(a => a.Favorites)
                      .HasForeignKey(e => e.AdId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.FavoritedOn)
                      .HasDefaultValueSql("getutcdate()");
            });

            // Message
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Content)
                      .IsRequired();

                entity.Property(e => e.SentOn)
                      .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.IsDeleted)
                      .HasDefaultValue(false);

                entity.HasOne(m => m.Sender)
                      .WithMany(u => u.Messages)
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Ad)
                      .WithMany(a => a.Messages)
                      .HasForeignKey(m => m.AdId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Phone
            modelBuilder.Entity<Phone>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Model)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.OS)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasOne(p => p.Brand)
                      .WithMany(b => b.Phones)
                      .HasForeignKey(p => p.BrandId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Ad)
                      .WithOne(a => a.Phone)
                      .HasForeignKey<Phone>(p => p.AdId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Images)
                      .WithOne(i => i.Phone)
                      .HasForeignKey(i => i.PhoneId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // PhoneImage
            modelBuilder.Entity<PhoneImage>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ImageUrl)
                      .IsRequired();

                entity.HasOne(i => i.Phone)
                      .WithMany(p => p.Images)
                      .HasForeignKey(i => i.PhoneId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
