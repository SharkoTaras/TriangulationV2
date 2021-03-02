using System.Collections.Generic;
using System.Linq;
using Triangulation.Core.Implementation.Utils;
using Triangulation.Core.Interfaces.Collections;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Utils.Comparers;

namespace Triangulation.Core.Interfaces.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<Vertex> DistinctVertex(this IEnumerable<Vertex> source)
        {
            return source.Distinct(new VertexComparer());
        }

        public static IEnumerable<Edge> DistinctEdge(this IEnumerable<Edge> source)
        {
            return source.Distinct(new EdgeComparer());
        }

        public static VertexCollection ToVertexCollection(this IEnumerable<Vertex> source)
        {
            Util.Check.NotNull(source, "Parameter is null");

            var result = new VertexCollection();
            foreach (var vertex in source)
            {
                result.Add(vertex);
            }
            return result;
        }

        /// <summary>
        /// Check if vertex is neither split nor merge
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="vertex"></param>
        /// <returns><see cref="bool"/> result</returns>
        /// <remarks>
        /// There is vectors management in checking algorithm
        /// </remarks>
        public static bool IsConvex(this LinkedList<Vertex> vertices, Vertex vertex)
        {
            Util.Check.NotNull(vertices, "Parameter is null");
            Util.Check.NotNull(vertex, "Parameter is null");

            var current = vertices.Find(vertex);
            var prev = current.Previous ?? vertices.Last;
            var next = current.Next ?? vertices.First;

            var currentVertex = current.Value;
            var prevVertex = prev.Value;
            var nextVertex = next.Value;

            var ab = new Vertex()
            {
                X = currentVertex.X - prevVertex.X,
                Y = currentVertex.Y - prevVertex.Y,
            };

            var bc = new Vertex()
            {
                X = currentVertex.X - nextVertex.X,
                Y = currentVertex.Y - nextVertex.Y,
            };

            var product = (ab.X * bc.Y) - (ab.Y * bc.X);

            return product > 0;
        }
    }
}
