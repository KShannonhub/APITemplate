using APITemplate.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace APITemplate.Services
{
    public class DefaultOpeningService : IOpeningService
    {
        private readonly HotelApiDbContext _context;
        private readonly IDateLogicService _dateLogicService;
        private readonly IMapper _mapper;

        public DefaultOpeningService(
            HotelApiDbContext context,
            IDateLogicService dateLogicService,
            IMapper mapper)
        {
            _context = context;
            _dateLogicService = dateLogicService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Opening>> GetOpeningsAsync()
        {
            var rooms = await _context.Rooms.ToArrayAsync();

            var allOpenings = new List<Opening>();

            foreach (var room in rooms)
            {
                // Generate a sequence of raw opening slots
                var allPossibleOpenings = _dateLogicService.GetAllSlots(
                        DateTimeOffset.UtcNow,
                        _dateLogicService.FurthestPossibleBooking(DateTimeOffset.UtcNow))
                    .ToArray();

                var conflictedSlots = await GetConflictingSlots(
                    room.Id,
                    allPossibleOpenings.First().StartAt,
                    allPossibleOpenings.Last().EndAt);

                // Remove the slots that have conflicts and project
                var openings = allPossibleOpenings
                    .Except(conflictedSlots, new BookingRangeComparer())
                    .Select(slot => new OpeningEntity
                    {
                        RoomId = room.Id,
                        Rate = room.Rate,
                        StartAt = slot.StartAt,
                        EndAt = slot.EndAt
                    })
                    .Select(model => _mapper.Map<Opening>(model));

                allOpenings.AddRange(openings);
            }

            return allOpenings;
        }

        public async Task<IEnumerable<BookingRange>> GetConflictingSlots(
            Guid roomId,
            DateTimeOffset start,
            DateTimeOffset end)
        {
            var bookings = await _context.Bookings
        .Where(b => b.Room.Id == roomId)
        .ToListAsync();

            var conflictingBookings = bookings
                .Where(b => _dateLogicService.DoesConflict(b, start, end))
                .SelectMany(existing => _dateLogicService.GetAllSlots(existing.StartAt, existing.EndAt))
                .ToArray();

            return conflictingBookings;
        }
    }
}
