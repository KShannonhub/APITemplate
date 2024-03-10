using APITemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace APITemplate
{
    public class HotelApiDbContext : DbContext
    {
        public HotelApiDbContext(DbContextOptions<HotelApiDbContext> options) : base(options)
        {
        }

        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<BookingEntity> Bookings { get; set; }
    }
}
