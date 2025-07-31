using Microsoft.AspNetCore.Identity;

namespace PhoneXchange.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        
        public virtual ICollection<Ad> Ads { get; set; } = new HashSet<Ad>();
        public virtual ICollection<FavoriteAd> FavoriteAds { get; set; } = new HashSet<FavoriteAd>();
        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
