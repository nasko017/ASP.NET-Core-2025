namespace PhoneXchange.Web.ViewModels.Message
{
    public class MessageActivityViewModel
    {
        public int MessageId { get; set; }
        public string AdTitle { get; set; } = null!;
        public int AdId { get; set; }

        public string Content { get; set; } = null!;
        public DateTime SentOn { get; set; }

        public string FromEmail { get; set; } = null!;
        public string ToEmail { get; set; } = null!;
    }
}
