
namespace PhoneXchange.Web.ViewModels.Ad
{
  using System.ComponentModel.DataAnnotations;
    public class AdEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително!")]
        [StringLength(100, ErrorMessage = "Заглавието не може да е повече от 100 символа.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Описанието е задължително!")]
        [StringLength(1000, ErrorMessage = "Описанието не може да е повече от 1000 символа.")]
        public string Description { get; set; } = null!;

        [Range(1, 10000, ErrorMessage = "Моля, въведете валидна цена.")]
        public decimal Price { get; set; }

        [MinLength(1, ErrorMessage = "Моля, добавете поне една снимка.")]
        public List<string> ImageUrls { get; set; } = new();
    }

}
