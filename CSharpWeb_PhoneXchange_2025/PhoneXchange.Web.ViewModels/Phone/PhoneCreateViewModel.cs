
using System.ComponentModel.DataAnnotations;

namespace PhoneXchange.Web.ViewModels.Phone
{
  
    public class PhoneCreateViewModel
    {
        [Required(ErrorMessage = "Моля, въведете модел на телефона.")]
        [StringLength(100, ErrorMessage = "Моделът трябва да е с дължина до 100 символа.")]
        public string Model { get; set; } = null!;

        [Required(ErrorMessage = "Моля, въведете операционна система.")]
        [StringLength(50, ErrorMessage = "Операционната система трябва да е с дължина до 50 символа.")]
        public string OS { get; set; } = null!;

        public bool IsNew { get; set; }

        [Required(ErrorMessage = "Моля, изберете марка.")]
        [Range(1, int.MaxValue, ErrorMessage = "Моля, изберете валидна марка.")]
        public int BrandId { get; set; }

        [MinLength(1, ErrorMessage = "Моля, добавете поне една снимка.")]
        public List<string> ImageUrls { get; set; } = new();
    }


}
