using APITemplate.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace APITemplate.Services
{
    public class DefaultRoomService : IRoomService
    {
        private readonly HotelApiDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _mappingConfiguration;

        public DefaultRoomService(HotelApiDbContext context, AutoMapper.IConfigurationProvider mappingConfiguration)
        {
            _context = context;
            _mappingConfiguration = mappingConfiguration;
        }

        public async Task<Room> GetRoomAsync(Guid id)
        {
            var entity = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return null;
            }

            return MapEntityToModel(entity);
        }

        private Room MapEntityToModel(RoomEntity entity)
        {
            var mapper = _mappingConfiguration.CreateMapper();
            return mapper.Map<Room>(entity);
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
           var query = _context.Rooms
                .ProjectTo<Room>(_mappingConfiguration);

            return await query.ToArrayAsync();
        }
    }
}
