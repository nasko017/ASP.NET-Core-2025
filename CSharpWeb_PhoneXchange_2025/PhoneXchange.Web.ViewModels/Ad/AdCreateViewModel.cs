
namespace PhoneXchange.Web.ViewModels.Ad
{
    using System.ComponentModel.DataAnnotations;

    public class AdCreateViewModel
    {
        [Required(ErrorMessage = "Заглавието е задължително!")]
        [StringLength(100, ErrorMessage = "Заглавието не може да е повече от 100 символа.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Описанието е задължително!")]
        [StringLength(1000, ErrorMessage = "Описанието не може да е повече от 1000 символа.")]
        public string Description { get; set; } = null!;

        [Range(1, 10000, ErrorMessage = "Моля, въведете валидна цена.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Моделът е задължителен!")]
        [StringLength(100, ErrorMessage = "Моделът не може да е повече от 100 символа.")]
        public string Model { get; set; } = null!;

        [Required(ErrorMessage = "Операционната система е задължителна!")]
        [StringLength(50, ErrorMessage = "ОС не може да е повече от 50 символа.")]
        public string OS { get; set; } = null!;

        public bool IsNew { get; set; }

        [Required(ErrorMessage = "Моля, изберете марка.")]
        [Range(1, int.MaxValue, ErrorMessage = "Моля, изберете валидна марка.")]
        public int BrandId { get; set; }

        [MinLength(1, ErrorMessage = "Моля, добавете поне една снимка.")]
        public List<string> ImageUrls { get; set; } = new();
    }

}
