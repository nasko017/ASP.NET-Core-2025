using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Web.ViewModels.Message
{
    public class MessageCreateViewModel
    {
        public string RecipientId { get; set; } = null!;
        public int AdId { get; set; } 
        public string Content { get; set; } = string.Empty;
    }
}
