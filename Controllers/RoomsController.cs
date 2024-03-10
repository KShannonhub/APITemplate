using APITemplate.Models;
using APITemplate.Services;
using Microsoft.AspNetCore.Mvc;

namespace APITemplate.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IOpeningService _openingService;

        public RoomsController(IRoomService roomService, IOpeningService openingService)
        {
            _roomService = roomService;
            _openingService = openingService;
        }

        [HttpGet(Name = nameof(GetAllRooms))]
        public async Task<ActionResult<Collection<Room>>> GetAllRooms()
        {
            var rooms = await _roomService.GetRoomsAsync();
            
            var collection = new Collection<Room>
            {
                Self = Link.ToCollection(nameof(GetAllRooms)),
                Value = rooms.ToArray()
            };

            return collection;
        }

        // GET /rooms/openings
        [HttpGet("openings", Name = nameof(GetAllRoomOpenings))]
        public async Task<ActionResult<Collection<Opening>>> GetAllRoomOpenings()
        {
            var openings = await _openingService.GetOpeningsAsync();

            var collection = new Collection<Opening>()
            {
                Self = Link.ToCollection(nameof(GetAllRoomOpenings)),
                Value = openings.ToArray()
            };

            return collection;
        }

        [HttpGet("{roomId}", Name = nameof(GetRoomById))]
        public async Task<ActionResult<Room>> GetRoomById(Guid roomId)
        {
            var room = await _roomService.GetRoomAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

    }
}
