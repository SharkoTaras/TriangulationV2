using System.Collections.Generic;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.Core.Algorithms.Interfaces.Algorithms
{
    public interface IAlgorithm
    {
        List<Triangle> Triangles { get; set; }

        List<Edge> Triangulate(IEnumerable<Vertex> vertices);
    }
}
