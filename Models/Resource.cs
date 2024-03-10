using System.Text.Json.Serialization;

namespace APITemplate.Models
{
    public abstract class Resource : Link
    {
        [JsonIgnore]
       public Link Self { get; set; }
    }
}
