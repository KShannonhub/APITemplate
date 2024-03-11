using APITemplate.Infrastructure;

namespace APITemplate.Models
{
    public class Room : Resource
    {
        [Sortable]
        public string Name { get; set; }
        
        [Sortable(Default = true)]
        public decimal Rate { get; set; }
    }
}
