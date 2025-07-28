
namespace PhoneXchange.Web.ViewModels.Ad
{
  using System.ComponentModel.DataAnnotations;
   public class AdEditViewModel
   {
       public int Id { get; set; }
   
       [Required(ErrorMessage = "Заглавието е задължително!")]
       [StringLength(100, ErrorMessage = "Максимум 100 символа.")]
       public string Title { get; set; } = string.Empty;
   
       [Required(ErrorMessage = "Описанието е задължително!")]
       [StringLength(1000, ErrorMessage = "Максимум 1000 символа.")]
       public string Description { get; set; } = string.Empty;
   
       [Range(1, 100000, ErrorMessage = "Въведи валидна цена.")]
       public decimal Price { get; set; }
   
       [Url(ErrorMessage = "Невалиден URL за снимка.")]
       public string ImageUrl { get; set; } = string.Empty;
   }
   
   
}
