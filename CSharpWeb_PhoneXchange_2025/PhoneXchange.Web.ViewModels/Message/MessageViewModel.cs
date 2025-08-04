using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneXchange.Web.ViewModels.Message
{
    public class MessageViewModel
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime SentOn { get; set; }
    }
}
