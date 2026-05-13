using Microsoft.AspNetCore.Mvc;
using PhoneXchange.Services.Core.Interfaces;

namespace PhoneXchange.Web.Controllers
{
    [ApiController]
    [Route("api/ads")]
    public class AdsApiController : ControllerBase
    {
        private readonly IAdService adService;

        public AdsApiController(IAdService adService)
        {
            this.adService = adService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ads = await adService.GetAllAsync();
            return Ok(ads);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ad = await adService.GetByIdAsync(id);

            if (ad == null)
            {
                return NotFound();
            }

            return Ok(ad);
        }
    }
}