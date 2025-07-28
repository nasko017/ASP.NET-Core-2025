using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Web.ViewModels.Phone
{
    public class PhoneCreateViewModel
    {
        public string Model { get; set; } = null!;
        public string OS { get; set; } = null!;
        public bool IsNew { get; set; }
        public int BrandId { get; set; }
        public List<string> ImageUrls { get; set; } = new();
    }

}
