using System.ComponentModel.DataAnnotations;

namespace PhoneXchange.Web.ViewModels.Message
{
    public class MessageCreateViewModel
    {
        [Required(ErrorMessage = "Получателят е задължителен.")]
        public string RecipientId { get; set; } = null!;

        [Required]
        public int AdId { get; set; }

        [Required(ErrorMessage = "Съдържанието е задължително.")]
        [StringLength(1000, ErrorMessage = "Съобщението не може да надвишава 1000 символа.")]
        public string Content { get; set; } = string.Empty;
    }
}
