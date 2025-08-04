namespace PhoneXchange.Data.Models
{
    public class FavoriteAd
    {
        public string ApplicationUserId { get; set; } = null!;

        public int AdId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public virtual Ad Ad { get; set; } = null!;
        public DateTime FavoritedOn { get; set; } = DateTime.UtcNow;
    }
}
