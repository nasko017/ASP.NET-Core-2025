using PhoneXchange.Web.ViewModels.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Web.ViewModels.Ad
{
    public class AdDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }

        public string Model { get; set; } = null!;
        public string OS { get; set; } = null!;
        public bool IsNew { get; set; }

        public string BrandName { get; set; } = null!;
        public List<string> ImageUrls { get; set; } = new();

        public int ReviewsCount { get; set; }
        public double AvgRating { get; set; }
        public List<ReviewViewModel> Reviews { get; set; } = new();
    }


}
