using System.Collections.Generic;
using System.Threading.Tasks;
using Triangulation.Core.Algorithms.Interfaces.Models;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.Core.Algorithms.Interfaces.Algorithms
{
    public interface IController
    {
        IRegion Region { get; set; }

        IEnumerable<Vertex> Vertices { get; }

        Task<IEnumerable<Vertex>> PrepareVertices(uint count);
    }
}
