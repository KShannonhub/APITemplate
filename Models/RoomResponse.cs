namespace APITemplate.Models
{
    public class RoomResponse : PagedCollection<Room>
    {
        public Link Openings { get; set; }
    }
}
