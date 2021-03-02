using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Triangulation.Core.Algorithms.Interfaces.Models.Dtos
{
    [DataContract, JsonObject]
    public class VertexDto
    {
        [DataMember, JsonProperty]
        public double X { get; set; }

        [DataMember, JsonProperty]
        public double Y { get; set; }
    }
}
