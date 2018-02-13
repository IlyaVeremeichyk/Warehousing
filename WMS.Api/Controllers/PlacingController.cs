using Microsoft.AspNetCore.Mvc;
using WMS.BL.Interfaces;

namespace WMS.Api.Controllers
{
    [Route("api/[controller]")]
    public class PlacingController : Controller
    {
        private IPlacingService _placingService;
        public PlacingController(IPlacingService placingService)
        {
            _placingService = placingService;
        }

        [HttpGet]
        public string Get()
        {
            return "It works!"  + _placingService.GetMessage();
        }
    }
}