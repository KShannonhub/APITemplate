using Newtonsoft.Json;
using System.ComponentModel;

namespace APITemplate.Models
{
    public class Link
    {
        public const string GetMethod = "GET";

        public static Link To(string routeName, object routeValues = null)
            => new Link
                {
                    RouteName = routeName,
                    RouteValues = routeValues,
                    Method = GetMethod,
                    Relations = null
                };

        public static Link ToCollection(string routeName, object routeValues = null)
            => new Link
            {
                    RouteName = routeName,
                    RouteValues = routeValues,
                    Method = GetMethod,
                    Relations = new string[] { "collection" }
                };
        

        [JsonProperty(Order = -4)]
        public string Href { get; set; }

        [JsonProperty(Order = -3,
            PropertyName = "rel",
            NullValueHandling = NullValueHandling.Ignore)]
        public string[] Relations { get; set; }
        
        [JsonProperty(Order = -2,
                       DefaultValueHandling = DefaultValueHandling.Ignore,
                       NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(GetMethod)]
        public string Method { get; set; }
        
        // Stores the route name and route values before being rewritten by the LinkRewritingFilter
        [JsonIgnore]
        public string RouteName { get; set; }
        [JsonIgnore]
        public object RouteValues { get; set; }
    }
}
