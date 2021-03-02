using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Triangulation.Core.Algorithms.Interfaces.Algorithms;
using Triangulation.Core.Implementation.Utils;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Extensions;

namespace Triangulation.Core.Algorithms.Implementation.Algorithms
{
    /// <summary>
    /// Delaunay triangulation algorithm, implements <see cref="ITriangulation"/>
    /// </summary>
    public class DelaunayAlgorithm : IAlgorithm
    {
        #region ITriangulation
        /// <summary>
        /// Collection of triangles - the result of triangulation
        /// </summary>
        /// <remarks><see cref="ITriangulation.Triangles"/> property</remarks>
        public List<Triangle> Triangles { get; set; }

        /// <summary>
        /// Triangulation algorithm of building segments, based on vertices collection
        /// </summary>
        /// <param name="vertices">Collection of all predefined vertices (points)</param>
        /// <returns>Collection of non-intersecting segments which in total form a set of triangles</returns>
        /// <remarks><see cref="ITriangulation.Triangulate(VertexCollection)"/> implementation</remarks>
        public List<Edge> Triangulate(IEnumerable<Vertex> vertices)
        {
            Util.Check.NotNull(vertices, "Can't triangulate null collection");

            var i = 0u;
            foreach (var v in vertices)
            {
                v.Id = i++;
            };

            vertices = vertices.OrderBy(v => v.X).ThenBy(v => v.Y);

            var result = new List<Edge>();

            Debug.WriteLine($"Create Super Triangle:");

            var superTriangle = this.GetSuperTriangle(vertices);

            Debug.WriteLine($"{superTriangle}");

            var triangulation = new HashSet<Triangle>
            {
                superTriangle
            };

            foreach (var point in vertices)
            {
                var badTriangles = this.FindBadTriangles(point, triangulation);
                var polygon = this.FindHoleBoundaries(badTriangles);

                foreach (var triangle in badTriangles)
                {
                    foreach (var vertex in triangle.Vertices)
                    {
                        vertex.AdjacentTriangles.Remove(triangle);
                    }
                }

                triangulation.RemoveWhere(triangle => badTriangles.Contains(triangle));

                Debug.WriteLine("Start build new triangles");
                Debug.WriteLine($"Current point: {point}");
                var posibleEdges = polygon.Where(possibleEdge => possibleEdge.Start != point && possibleEdge.End != point).DistinctEdge().ToList();
                foreach (var edge in posibleEdges)
                {
                    Debug.WriteLine($"Edge for new triangle: {edge}");
                    if ((point.X == edge.Start.X && point.X == edge.End.X) ||
                       (point.Y == edge.Start.Y && point.Y == edge.End.Y))
                    {
                        Debug.WriteLine(new string('-', 20));
                        Debug.WriteLine("Something wrong");
                        continue;
                    }

                    var triangle = new Triangle(point, edge.Start, edge.End);
                    triangulation.Add(triangle);
                }
            }

            triangulation.RemoveWhere(t => t.Vertices.Any(v => superTriangle.Vertices.Contains(v)));

            this.Triangles = triangulation.ToList();

            foreach (var triangle in triangulation)
            {
                triangle.Vertices.
                    ForEach(v => v.AdjacentTriangles.RemoveWhere(t => t.Vertices.Intersect(superTriangle.Vertices).Any()));
                result.AddRange(triangle.Edges);
            }

            return result.DistinctEdge().ToList();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Build polygon, which is not triangulated yet
        /// </summary>
        /// <param name="badTriangles">Triangles which don't satisfy Delaunay condition</param>
        /// <returns>Segments of polygon, which still has to be triangulated</returns>
        private List<Edge> FindHoleBoundaries(ISet<Triangle> badTriangles)
        {
            var edges = new List<Edge>();
            foreach (var triangle in badTriangles)
            {
                edges.Add(new Edge(triangle.Vertices[0], triangle.Vertices[1]));
                edges.Add(new Edge(triangle.Vertices[1], triangle.Vertices[2]));
                edges.Add(new Edge(triangle.Vertices[2], triangle.Vertices[0]));
            }
            var grouped = edges.GroupBy(o => o);
            var boundaryEdges = edges.GroupBy(o => o).Where(o => o.Count() == 1).Select(o => o.First());
            return boundaryEdges.ToList();
        }

        /// <summary>
        /// Select triangles, which don't satisfy Delaunay condition for the vertex
        /// </summary>
        /// <param name="vertex">Target vertex</param>
        /// <param name="triangles">Set of triangles</param>
        /// <returns>Set of "bad" triangles</returns>
        private ISet<Triangle> FindBadTriangles(Vertex vertex, HashSet<Triangle> triangles)
        {
            var badTriangles = triangles.Where(t => t.IsPointInsideCircumcircle(vertex));

            Debug.WriteLine("Bad triangles:");

            foreach (var t in badTriangles)
            {
                Debug.WriteLine($"{t}");
                Debug.WriteLine(string.Empty);
            }

            return new HashSet<Triangle>(badTriangles);
        }

        /// <summary>
        /// Find super-triangle - the first step of Bowyer-Watson triangulation algorithm
        /// </summary>
        /// <param name="vertices">Collection of all vertices</param>
        /// <returns>Super-triangle</returns>
        /// <remarks>https://en.wikipedia.org/wiki/Bowyer?Watson_algorithm</remarks>
        private Triangle GetSuperTriangle(IEnumerable<Vertex> vertices)
        {
            var minX = vertices.First().X;
            var minY = vertices.First().Y;
            var maxX = minX;
            var maxY = minY;

            foreach (var vertex in vertices)
            {
                if (vertex.X < minX)
                {
                    minX = vertex.X;
                }

                if (vertex.Y < minY)
                {
                    minY = vertex.Y;
                }

                if (vertex.X > maxX)
                {
                    maxX = vertex.X;
                }

                if (vertex.Y > maxY)
                {
                    maxY = vertex.Y;
                }
            }

            var dx = maxX - minX;
            var dy = maxY - minY;
            var deltaMax = Math.Max(dx, dy);
            var midx = (minX + maxX) / 2;
            var midy = (minY + maxY) / 2;

            var p1 = new Vertex(midx - (20 * deltaMax), midy - deltaMax);
            var p2 = new Vertex(midx, midy + (20 * deltaMax));
            var p3 = new Vertex(midx + (20 * deltaMax), midy - deltaMax);

            return new Triangle(p1, p2, p3);
        }

        public List<Triangle> BuildTriangles(List<Edge> edges)
        {
            var triangles = new List<Triangle>();
            foreach (var edge in edges)
            {
                var adjacentEdgesStart = edges.Where(x => x.Contains(edge.Start) && x != edge).ToList();
                var adjacentEdgesEnd = edges.Where(x => x.Contains(edge.End) && x != edge).ToList();
                foreach (var secondEdge in adjacentEdgesStart)
                {
                    var thirdEdges = adjacentEdgesEnd.Where(x => x.Contains(secondEdge.Start) || x.Contains(secondEdge.End)).ToList();
                    if (thirdEdges.Count() != 0)
                    {
                        var triangle = new Triangle() { First = edge, Second = secondEdge, Third = thirdEdges.First() };
                        if (!triangles.Contains(triangle))
                        {
                            triangles.Add(triangle);
                        }
                    }
                }
            }
            return triangles;
        }

        #endregion
    }
}
