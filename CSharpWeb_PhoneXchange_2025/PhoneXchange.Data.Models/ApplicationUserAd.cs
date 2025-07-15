using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PhoneXchange.Data.Models
{
    public class ApplicationUserAd
    {
        public string ApplicationUserId { get; set; } = null!;

        public int AdId { get; set; }

        public virtual IdentityUser ApplicationUser { get; set; } = null!;
        public virtual Ad Ad { get; set; }=null!;

        public DateTime PurchasedOn { get; set; } = DateTime.UtcNow;
    }
}
