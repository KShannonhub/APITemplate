using APITemplate.Models;

namespace APITemplate.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetRoomsAsync();
        Task<Room> GetRoomAsync(Guid id);
    }
}
