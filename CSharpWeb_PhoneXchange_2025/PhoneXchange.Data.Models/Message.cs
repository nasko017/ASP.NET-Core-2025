using Microsoft.AspNetCore.Identity;

namespace PhoneXchange.Data.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }

        public string SenderId { get; set; }
        public virtual IdentityUser Sender { get; set; }

        public Guid AdId { get; set; }
        public virtual Ad Ad { get; set; }
    }
}