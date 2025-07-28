
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

        [Required(ErrorMessage = "Цената е задължителна!")]
        [Range(1, 10000, ErrorMessage = "Цената трябва да е между 1 и 10 000 лв.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Моделът е задължителен!")]
        [StringLength(100, ErrorMessage = "Моделът не може да е повече от 100 символа.")]
        public string Model { get; set; } = null!;

        [Required(ErrorMessage = "Операционната система е задължителна!")]
        [StringLength(50, ErrorMessage = "Операционната система не може да е повече от 50 символа.")]
        public string OS { get; set; } = null!;

        [Required(ErrorMessage = "Марката е задължителна!")]
        public int BrandId { get; set; }

        [Display(Name = "Състояние (нов/употребяван)")]
        public bool IsNew { get; set; }

        [Required(ErrorMessage = "URL към снимка е задължителен!")]
        [Url(ErrorMessage = "Невалиден URL адрес за снимка.")]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;
    }

}
