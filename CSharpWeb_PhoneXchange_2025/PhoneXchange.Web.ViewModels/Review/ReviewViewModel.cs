
namespace PhoneXchange.Web.ViewModels.Review
{
    public class ReviewViewModel
    {
        public int Id { get; set; }

        public int Rating { get; set; } // Стойност от 1 до 5

        public string Comment { get; set; } = string.Empty;

        public string AuthorEmail { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
    }
}
