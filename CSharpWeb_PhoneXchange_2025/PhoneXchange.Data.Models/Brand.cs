

namespace PhoneXchange.Data.Models
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
    }

}
