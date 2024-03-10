using APITemplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace APITemplate.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly HotelInfo _hotelInfo;

        public InfoController(IOptions<HotelInfo> hotelInfo)
        {
            _hotelInfo = hotelInfo.Value;
        }

        [HttpGet(Name = nameof(GetInfo))]
        public ActionResult<HotelInfo> GetInfo()
        {
            _hotelInfo.Href = Url.Link(nameof(GetInfo), null);

            return _hotelInfo;
        }   
    }
}
