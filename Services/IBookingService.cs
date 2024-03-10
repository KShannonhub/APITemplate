using APITemplate.Models;

namespace APITemplate.Services
{
    public interface IBookingService
    {
        Task<Booking> GetBookingAsync(Guid bookingId);

        Task<Guid> CreateBookingAsync(
            Guid userId,
            Guid roomId,
            DateTimeOffset startAt,
            DateTimeOffset endAt);
    }
}
