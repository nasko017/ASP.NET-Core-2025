
using PhoneXchange.Web.ViewModels.Review;

namespace PhoneXchange.Web.ViewModels.Ad
{
    public class AdViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public string OwnerId { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public List<ReviewViewModel> Reviews { get; set; } = new();
    }

}
