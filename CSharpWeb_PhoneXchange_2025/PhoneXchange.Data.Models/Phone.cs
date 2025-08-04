
namespace PhoneXchange.Data.Models
{
    public class Phone
    {
        public int Id { get; set; }

        public string Model { get; set; } = null!;

        public string OS { get; set; } = null!;

        public bool IsNew { get; set; }

        public int AdId { get; set; }
        public virtual Ad Ad { get; set; } = null!;

        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; } = null!;

        public string ImageUrlsSerialized { get; set; } = string.Empty;

    }
}
