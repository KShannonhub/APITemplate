using APITemplate.Models;

namespace APITemplate.Services
{
    public interface IOpeningService
    {
        Task<IEnumerable<Opening>> GetOpeningsAsync();

        Task<IEnumerable<BookingRange>> GetConflictingSlots(
            Guid roomId,
            DateTimeOffset start,
            DateTimeOffset end);
    }
}
