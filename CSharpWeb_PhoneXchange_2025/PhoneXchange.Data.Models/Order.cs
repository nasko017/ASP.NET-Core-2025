using PhoneXchange.Data.Models.Enums;
namespace PhoneXchange.Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string BuyerId { get; set; } = null!;
        public virtual ApplicationUser Buyer { get; set; } = null!;

        public int AdId { get; set; }
        public virtual Ad Ad { get; set; } = null!;

        public DateTime OrderedOn { get; set; } = DateTime.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string? Note { get; set; }
    }
}
