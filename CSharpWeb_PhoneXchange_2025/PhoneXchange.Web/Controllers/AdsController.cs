using Microsoft.AspNetCore.Mvc;
using PhoneXchange.Web.Data;
using PhoneXchange.Web.ViewModels.Ad;

namespace PhoneXchange.Web.Controllers
{
    public class AdsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ads = _context.Ads
                .Where(a => !a.IsDeleted)
                .Select(a => new AdViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Price = a.Price,
                    ImageUrl = a.Phone.Images.FirstOrDefault()!.ImageUrl
                })
                .ToList();

            return View(ads);
        }
    }
}
