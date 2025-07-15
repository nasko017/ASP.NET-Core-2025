using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Data.Models
{
    public class PhoneImage
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; } = null!;

        public int PhoneId { get; set; }
        public virtual Phone Phone { get; set; } = null!;
    }
}
