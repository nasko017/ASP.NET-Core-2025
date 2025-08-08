using System.ComponentModel.DataAnnotations;

namespace PhoneXchange.Web.ViewModels.Brand
{
    public class BrandViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на марката е задължително.")]
        [StringLength(50, ErrorMessage = "Името на марката не може да е по-дълго от 50 символа.")]
        public string Name { get; set; } = null!;
    }

}
