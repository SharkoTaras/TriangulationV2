using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Triangulation.Core.Algorithms.Interfaces.Algorithms;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Extensions;

namespace Triangulation.Core.Algorithms.Implementation.Algorithms
{
    /// <summary>
    /// Greedy triangulation algorithm, implements <see cref="ITriangulation"/>
    /// </summary>
    public class GreedyAlgorithm : IAlgorithm
    {
        public List<Triangle> Triangles { get; set; }

        /// <summary>
        /// Triangulation algorithm of building segments
        /// </summary>
        /// <param name="vertices">Collection of vertices</param>
        /// <returns>Collection of segments which is the result of triangulation</returns>
        /// <remarks><see cref="ITriangulation.Triangulate(VertexCollection)"/> implementation</remarks>
        public List<Edge> Triangulate(IEnumerable<Vertex> vertices)
        {
            var v = vertices.ToList();
            var edges = new ConcurrentBag<Edge>();
            Parallel.For(0, vertices.Count(), (i) =>
            {
                Parallel.For(0, vertices.Count(), (j) =>
                {
                    if (i != j)
                    {
                        edges.Add(new Edge(v[i], v[j]));
                    }
                });
            });

            var orderedByLength = edges.OrderBy(e => e.Length);

            var result = new List<Edge>
            {
                orderedByLength.First()
            };

            foreach (var edge in orderedByLength.Skip(1))
            {
                if (result.TrueForAll(e => !e.IsIntersect(edge)) && !result.Contains(edge))
                {
                    result.Add(edge);
                }
            }
            return result.DistinctEdge().ToList();
        }
    }
}
