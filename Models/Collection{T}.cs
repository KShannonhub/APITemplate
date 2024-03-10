namespace APITemplate.Models
{
    public class Collection<T> : Resource
    {
        public T[] Value { get; set; }

    }
}
