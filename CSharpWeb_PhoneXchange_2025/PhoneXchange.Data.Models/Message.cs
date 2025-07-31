using Microsoft.AspNetCore.Identity;

namespace PhoneXchange.Data.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime SentOn { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }
        public string SenderId { get; set; } = null!;
        public virtual ApplicationUser Sender { get; set; } = null!;

        public int AdId { get; set; }
        public virtual Ad Ad { get; set; } = null!;
    }
}