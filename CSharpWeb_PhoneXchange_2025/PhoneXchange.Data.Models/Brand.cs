namespace PhoneXchange.Data.Models
{
    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
    }
}
