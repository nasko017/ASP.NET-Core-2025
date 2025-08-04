namespace PhoneXchange.Web.ViewModels.Message
{
    public class MessageViewModel
    {
        public string OtherUserEmail { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime SentOn { get; set; }
    }
}
