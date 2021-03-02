namespace Triangulation.Core.Interfaces
{
    public class GlobalConstants
    {
        public class Services
        {
            public const string RegionServiceEntity = "Region";
        }

        public class FilesPath
        {
            public const string Region = "region.json"; 
        }

        public class Serializers
        {
            public const string VertexSerializer = nameof(VertexSerializer);
            public const string EdgeSerializer = nameof(EdgeSerializer);
            public const string TriangleSerializer = nameof(TriangleSerializer);
        }
    }
}
