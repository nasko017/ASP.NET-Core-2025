
namespace PhoneXchange.Data.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string AuthorId { get; set; } = null!;
        public virtual ApplicationUser Author { get; set; } = null!;

        public int AdId { get; set; }
        public virtual Ad Ad { get; set; } = null!;

        public int Rating { get; set; } // 1 to 5
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
