
using Microsoft.AspNetCore.Identity;

namespace PhoneXchange.Data.Models
{
    public class Ad
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsSold { get; set; }
        public DateTime CreatedOn { get; set; }

        public Guid PhoneId { get; set; }
        public virtual Phone Phone { get; set; }

        public string SellerId { get; set; }
        public virtual IdentityUser Seller { get; set; }

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
