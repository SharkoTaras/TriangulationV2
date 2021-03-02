using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Triangulation.Core.Algorithms.Interfaces.Models.Dtos
{
    [DataContract, JsonObject]
    public class RegionDto
    {
        [DataMember, JsonProperty("type")]
        public string InitializingType { get; set; }

        [DataMember, JsonIgnore]
        public VertexDto[] Vertices { get; set; }
    }
}
