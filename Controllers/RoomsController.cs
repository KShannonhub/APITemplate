using APITemplate.Infrastructure;
using APITemplate.Models;
using APITemplate.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace APITemplate.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IOpeningService _openingService;
        private readonly PagingOptions _defaultPagingOptions;

        public RoomsController(IRoomService roomService, IOpeningService openingService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _roomService = roomService;
            _openingService = openingService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpGet(Name = nameof(GetAllRooms))]
        public async Task<ActionResult<Collection<Room>>> GetAllRooms(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<Room, RoomEntity> sortOptions,
            [FromQuery] SearchOptions<Room, RoomEntity> searchOptions)
        {
            pagingOptions.Offset ??= _defaultPagingOptions.Offset;
            pagingOptions.Limit ??= _defaultPagingOptions.Limit;

            var rooms = await _roomService.GetRoomsAsync(pagingOptions, sortOptions, searchOptions);

            var collection = PagedCollection<Room>.Create<RoomResponse>(
               Link.ToCollection(nameof(GetAllRooms)),
               rooms.Items.ToArray(),
               rooms.TotalSize,
               pagingOptions);
            collection.Openings = Link.ToCollection(nameof(GetAllRoomOpenings));

            return collection;
        }

        // GET /rooms/openings
        [HttpGet("openings", Name = nameof(GetAllRoomOpenings))]
        public async Task<ActionResult<Collection<Opening>>> GetAllRoomOpenings([FromQuery] PagingOptions pagingOptions, [FromQuery] SortOptions<Opening, OpeningEntity> sortOptions)
        {
            var openings = await _openingService.GetOpeningsAsync(pagingOptions, sortOptions);

            pagingOptions.Offset ??= _defaultPagingOptions.Offset;
            pagingOptions.Limit ??= _defaultPagingOptions.Limit;

            var collection = PagedCollection<Opening>.Create(Link.ToCollection(nameof(GetAllRoomOpenings)), openings.Items.ToArray(), openings.TotalSize, pagingOptions);

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
