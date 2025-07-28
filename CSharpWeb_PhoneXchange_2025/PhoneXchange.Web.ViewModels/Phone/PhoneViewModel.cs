using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Web.ViewModels.Phone
{
    public class PhoneViewModel
    {
        public int Id { get; set; }
        public string Model { get; set; } = null!;
        public string OS { get; set; } = null!;
        public bool IsNew { get; set; }
        public string BrandName { get; set; } = null!;
        public int BrandId { get; set; }
        public int? AdId { get; set; }
        public List<string> ImageUrls { get; set; } = new();
    }

}
