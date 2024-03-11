using APITemplate.Infrastructure;

namespace APITemplate.Models
{
    public class Opening
    {
        [Sortable(EntityProperty = nameof(OpeningEntity.RoomId))]
        public Link Room { get; set; }
       
        [Sortable(Default = true)]
        public DateTimeOffset StartAt { get; set; }

        [Sortable]
        public DateTimeOffset EndAt { get; set; }

        [Sortable]
        public decimal Rate { get; set; }
    }
}
