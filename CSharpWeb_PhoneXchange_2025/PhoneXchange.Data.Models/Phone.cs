using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual ICollection<PhoneImage> Images { get; set; } = new List<PhoneImage>();
    }
}
