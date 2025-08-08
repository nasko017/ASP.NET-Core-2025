using System.ComponentModel.DataAnnotations;

namespace PhoneXchange.Web.ViewModels.Review
{
    public class ReviewCreateViewModel
    {
        [Required]
        public int AdId { get; set; }

        [Range(1, 5, ErrorMessage = "Оценката трябва да е между 1 и 5.")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Коментарът е задължителен.")]
        [StringLength(500, ErrorMessage = "Коментарът не може да е над 500 символа.")]
        public string Comment { get; set; } = string.Empty;
    }
}
