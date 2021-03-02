using System.Collections.Generic;
using Triangulation.Core.Algorithms.Interfaces.Enums;
using Triangulation.Core.Algorithms.Interfaces.Models;

namespace Triangulation.Core.Interfaces.Entities.Dtos
{
    public class CanvasRegion : ICanvasEnity
    {
        public uint Zoom { get; set; }

        public IEnumerable<IEntity> Entities => this.PrimaryRegion.Entities;

        public RegionInitializingType InitializingType => this.PrimaryRegion.InitializingType;

        public Region PrimaryRegion { get; set; }
    }
}
