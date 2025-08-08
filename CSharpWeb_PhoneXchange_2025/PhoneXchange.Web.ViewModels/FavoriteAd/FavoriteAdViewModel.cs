namespace PhoneXchange.Web.ViewModels.FavoriteAd
{
    public class FavoriteAdViewModel
    {
        public int AdId { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
