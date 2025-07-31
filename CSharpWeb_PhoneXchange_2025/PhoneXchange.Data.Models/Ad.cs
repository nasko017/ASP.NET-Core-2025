using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PhoneXchange.Data.Models
{
    public class Ad
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; }=null!;

        public decimal Price { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public string OwnerId { get; set; }=null!;
        public virtual ApplicationUser Owner { get; set; } = null!;

        public virtual Phone Phone { get; set; } = null!;

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public virtual ICollection<FavoriteAd> Favorites { get; set; } = new List<FavoriteAd>();
    }
}