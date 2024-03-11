using APITemplate.Infrastructure;

namespace APITemplate.Models
{
    public class Room : Resource
    {
        [Sortable]
        [Searchable]
        public string Name { get; set; }
        
        [Sortable(Default = true)]
        [Searchable]
        public decimal Rate { get; set; }
    }
}
