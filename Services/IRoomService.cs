using APITemplate.Models;

namespace APITemplate.Services
{
    public interface IRoomService
    {
        Task<PagedResults<Room>> GetRoomsAsync(PagingOptions pagingOptions, SortOptions<Room, RoomEntity> sortOptions);
        Task<Room> GetRoomAsync(Guid id);
    }
}
