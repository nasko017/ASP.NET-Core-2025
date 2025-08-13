using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhoneXchange.Data.Models;

namespace PhoneXchange.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Ad> Ads { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<FavoriteAd> FavoriteAds { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Phone> Phones { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Soft delete filters
            builder.Entity<Ad>().HasQueryFilter(a => !a.IsDeleted);
            builder.Entity<Message>().HasQueryFilter(m => !m.IsDeleted);

            // --- Ad ---
            builder.Entity<Ad>()
                .Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<Ad>()
                .Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Entity<Ad>()
                .Property(a => a.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Ad>()
                .HasOne(a => a.Phone)
                .WithOne(p => p.Ad)
                .HasForeignKey<Phone>(p => p.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Ad>()
                .HasOne(a => a.Owner)
                .WithMany(u => u.Ads)
                .HasForeignKey(a => a.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Phone ---
            builder.Entity<Phone>()
                .Property(p => p.Model)
                .IsRequired()
                .HasMaxLength(50);

            builder.Entity<Phone>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Phones)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Phone>()
                .Property(p => p.OS)
                .IsRequired()
                .HasMaxLength(30);

            builder.Entity<Phone>()
                .Property(p => p.ImageUrlsSerialized)
                .HasColumnName("ImageUrls")
                .HasMaxLength(1000);

            // --- Brand ---
            builder.Entity<Brand>()
                .Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(50);

            // --- Review ---
            builder.Entity<Review>()
                .Property(r => r.Comment)
                .HasMaxLength(1000);

            builder.Entity<Review>()
                .HasOne(r => r.Ad)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Author)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- FavoriteAd ---
            builder.Entity<FavoriteAd>()
                .HasKey(f => new { f.ApplicationUserId, f.AdId });

            builder.Entity<FavoriteAd>()
                .HasOne(f => f.ApplicationUser)
                .WithMany(u => u.FavoriteAds)
                .HasForeignKey(f => f.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FavoriteAd>()
                .HasOne(f => f.Ad)
                .WithMany(a => a.Favorites)
                .HasForeignKey(f => f.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Order ---
            builder.Entity<Order>()
                .Property(o => o.Note)
                .HasMaxLength(500);

            builder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Entity<Order>()
                .HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.Ad)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Message ---
            builder.Entity<Message>()
                .Property(m => m.Content)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany()
                .HasForeignKey(m => m.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.Ad)
                .WithMany(a => a.Messages)
                .HasForeignKey(m => m.AdId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
