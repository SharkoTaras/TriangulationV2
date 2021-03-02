using Triangulation.Core.Algorithms.Interfaces.Models;
using Triangulation.Core.Interfaces;
using Triangulation.Core.Interfaces.Services;
using Unity;

namespace Triangulation.Core.Implementation.Services
{
    public class RegionService : IService<Region>
    {
        [InjectionConstructor]
        public RegionService()
        {
        }

        public string ServiceEntity => GlobalConstants.Services.RegionServiceEntity;

        [Dependency]
        public IReader<Region> Reader { get; set; }

        public Region Initialize()
        {
            return this.Reader.Read();
        }
    }
}
