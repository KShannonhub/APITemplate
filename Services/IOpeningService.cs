using APITemplate.Models;

namespace APITemplate.Services
{
    public interface IOpeningService
    {
        Task<PagedResults<Opening>> GetOpeningsAsync(
           PagingOptions pagingOptions,
           SortOptions<Opening, OpeningEntity> sortOptions);

        Task<IEnumerable<BookingRange>> GetConflictingSlots(
            Guid roomId,
            DateTimeOffset start,
            DateTimeOffset end);
    }
}
