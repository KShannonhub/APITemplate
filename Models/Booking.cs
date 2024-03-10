namespace APITemplate.Models
{
    public class Booking : Resource
    {
        public Link Room { get; set; }
        public Link User { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public decimal Total { get; set; }
    }
}
