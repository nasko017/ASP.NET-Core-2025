
namespace PhoneXchange.Data.Models
{
    public class Phone
    {
        public Guid Id { get; set; }
        public string Model { get; set; }
        public string Specifications { get; set; }

        public Guid BrandId { get; set; }
        public virtual Brand Brand { get; set; }

        public virtual ICollection<Ad> Ads { get; set; } = new List<Ad>();
    }
}
