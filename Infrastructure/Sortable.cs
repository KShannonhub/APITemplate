
namespace APITemplate.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] 
    public class Sortable : Attribute
    {
        public bool Default { get; set; }
    }
}
