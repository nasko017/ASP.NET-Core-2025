﻿
namespace PhoneXchange.Web.ViewModels.Ad
{
    public class AdViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;

        public string OwnerId { get; set; } = string.Empty;
    }
}
