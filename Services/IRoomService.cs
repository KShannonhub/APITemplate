using APITemplate.Infrastructure;
using APITemplate.Models;

namespace APITemplate.Services
{
    public interface IRoomService
    {
        Task<PagedResults<Room>> GetRoomsAsync(PagingOptions pagingOptions, SortOptions<Room, RoomEntity> sortOptions, SearchOptions<Room, RoomEntity> searchOptions);
        Task<Room> GetRoomAsync(Guid id);
    }
}
