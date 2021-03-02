using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Triangulation.Core.Interfaces.Collections;
using Triangulation.Core.Interfaces.Extensions;
using Triangulation.Core.Interfaces.Utils.Comparers;

namespace Triangulation.Core.Interfaces.Entities
{
    /// <summary>
    /// Class representation of the triangle figure
    /// </summary>
    public class Triangle : IEntity
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Triangle()
        {
            Circumcenter = new Vertex();
            First = new Edge();
            Second = new Edge();
            Third = new Edge();
        }

        /// <summary>
        /// Constructor with three sides of the triangle
        /// </summary>
        /// <param name="first">First side (<see cref="Edge"/> object)</param>
        /// <param name="second">Second side (<see cref="Edge"/> object)</param>
        /// <param name="third">Third side (<see cref="Edge"/> object)</param>
        public Triangle(Edge first, Edge second, Edge third)
        {
            First = first;
            Second = second;
            Third = third;

            var points = first.Vertices.Concat(second.Vertices).Concat(Third.Vertices).DistinctVertex().ToList();

            SetupVertices(points[0], points[1], points[2]);

            UpdateCircumcircleDefault();

            if (Avx.IsSupported)
            {
                UpdateCircumcircleVector();
            }
            else
            {
                UpdateCircumcircleDefault();
            }
        }

        /// <summary>
        /// Constructor with three vertices (points) of the triangle
        /// </summary>
        /// <param name="first">First <see cref="Vertex"/> object</param>
        /// <param name="second">Second <see cref="Vertex"/> object</param>
        /// <param name="third">Third <see cref="Vertex"/> object</param>
        public Triangle(Vertex first, Vertex second, Vertex third)
        {
            Vertices = new List<Vertex>();

            SetupVertices(first, second, third);

            First = new Edge(Vertices[0], Vertices[1]);
            Second = new Edge(Vertices[1], Vertices[2]);
            Third = new Edge(Vertices[2], Vertices[0]);

            if (Avx.IsSupported)
            {
                UpdateCircumcircleVector();
            }
            else
            {
                UpdateCircumcircleDefault();
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Squared radius of the circumcircle (R^2)
        /// </summary>
        public double SquaredRadius { get; set; }

        /// <summary>
        /// Coordinate of the center of the circumcircle (middle perpendiculars intersection)
        /// </summary>
        public Vertex Circumcenter { get; private set; }

        /// <summary>
        /// Triangles which have one of the sides the same as current triangle's sides
        /// </summary>
        /// <remarks>Example: Current - ABC, then there are triangles ABD (AB side), BCK (BC side), ACK (AC side) if such triangles exist</remarks>
        public IEnumerable<Triangle> TrianglesWithSharedEdge
        {
            get
            {
                var neighbors = new HashSet<Triangle>();
                foreach (var vertex in Vertices)
                {
                    var trianglesWithSharedEdge = vertex.AdjacentTriangles.Where(t =>
                    {
                        return t != this && SharesEdgeWith(t);
                    });
                    neighbors.UnionWith(trianglesWithSharedEdge);
                }

                return neighbors;
            }
        }

        /// <summary>
        /// First triangle's side
        /// </summary>
        public Edge First { get; set; }

        /// <summary>
        /// Second triangle's side
        /// </summary>
        public Edge Second { get; set; }

        /// <summary>
        /// Third triangle's side
        /// </summary>
        public Edge Third { get; set; }

        /// <summary>
        /// Triangle's vertices in custom container <see cref="VertexCollection"/>
        /// </summary>
        public List<Vertex> Vertices { get; set; }

        /// <summary>
        /// Triangle segments
        /// </summary>
        public List<Edge> Edges => new List<Edge> { First, Second, Third };

        /// <summary>
        /// Indicates if triangle satisfies algorithm condition, suppose
        /// </summary>
        public bool IsBad { get; set; } = false;

        /// <summary>
        /// Scalar value representing the triangle's square
        /// </summary>
        public double Square
        {
            get
            {
                //Calculate by Gerone formula
                var a = First.Length;
                var b = Second.Length;
                var c = Third.Length;

                var p = (a + b + c) / 2;

                return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            }
        }

        public uint Id { get; }

        public string SerializerName => GlobalConstants.Serializers.TriangleSerializer;
        #endregion

        /// <summary>
        /// Check if target triangle has a common side with the current triangle
        /// </summary>
        /// <param name="triangle">Triangle to check with</param>
        /// <returns><see cref="true"/> if shares edge with target triangle, <see cref="false"/> otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SharesEdgeWith(Triangle triangle) => Vertices.Where(v => triangle.Vertices.Contains(v, new VertexComparer())).Count() == 2;

        /// <summary>
        /// Delaunay condition: check if vertex is inside the circumcircle of the current triangle
        /// </summary>
        /// <param name="point">Target point</param>
        /// <returns><see cref="true"/> if inside, <see cref="false"/> otherwise</returns>
        public bool IsPointInsideCircumcircle(Vertex point)
        {
            var dSquared = (point.X - Circumcenter.X).ToPow(2) +
                            (point.Y - Circumcenter.Y).ToPow(2);
            return dSquared < SquaredRadius;
        }

        #region Overrides
        /// <summary>
        /// Display information about <see cref="Triangle"/> (<see cref="object.ToString"/> overriding)
        /// </summary>
        /// <returns>Formatted string</returns>
        public override string ToString() => string.Join("\n", Edges);
        #endregion

        /// <summary>
        /// Triangle description condition: check if three vertices are set in clockwise order
        /// </summary>
        /// <param name="point1">First vertex</param>
        /// <param name="point2">Second vertex</param>
        /// <param name="point3">Third vertex</param>
        /// <returns><see cref="true"/> if points are situated clockwise, <see cref="false"/> otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsCounterClockwise(Vertex point1, Vertex point2, Vertex point3)
        {
            var result = ((point2.X - point1.X) * (point3.Y - point1.Y)) -
                ((point3.X - point1.X) * (point2.Y - point1.Y));
            return result > 0;
        }

        /// <summary>
        /// Setup triangle vertices
        /// </summary>
        /// <param name="first">First vertex</param>
        /// <param name="second">Second vertex</param>
        /// <param name="third">Third vertex</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetupVertices(Vertex first, Vertex second, Vertex third)
        {
            if (!IsCounterClockwise(first, second, third))
            {
                Vertices.Add(first);
                Vertices.Add(third);
                Vertices.Add(second);
            }
            else
            {
                Vertices.Add(first);
                Vertices.Add(second);
                Vertices.Add(third);
            }

            foreach (var vertex in Vertices)
            {
                vertex.AdjacentTriangles.Add(this);
            }
        }

        /// <summary>
        /// Find center and radius of the circumcenter
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void UpdateCircumcircleVector()
        {
            var p2 = Vertices[2];
            var p1 = Vertices[1];
            var p0 = Vertices[0];

            var d1 = p0.X * (p2.Y - p1.Y);
            var d2 = p1.X * (p0.Y - p2.Y);
            var d3 = p2.X * (p1.Y - p0.Y);

            var div = (d1 + d2 + d3) * 2;

            if (div == 0)
            {
                throw new DivideByZeroException();
            }

            var pointsX = Vector256.Create(p0.X, p1.X, p2.X, 0);
            var pointsY = Vector256.Create(p0.Y, p1.Y, p2.Y, 0);

            var squaredX = Avx.Multiply(pointsX, pointsX);
            var squaredY = Avx.Multiply(pointsY, pointsY);

            var d = Avx.Add(squaredX, squaredY).ToEnumerable().ToArray();

            var dC = d[2];
            var dB = d[1];
            var dA = d[0];

            var aux1 = (dA * (p2.Y - p1.Y)) + (dB * (p0.Y - p2.Y)) + (dC * (p1.Y - p0.Y));
            var aux2 = -((dA * (p2.X - p1.X)) + (dB * (p0.X - p2.X)) + (dC * (p1.X - p0.X)));

            var center = new Vertex(aux1 / div, aux2 / div);
            Circumcenter = center;
            SquaredRadius = (center.X - p0.X).ToPow(2) + (center.Y - p0.Y).ToPow(2);
        }

        /// <summary>
        /// Find center and radius of the circumcenter
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void UpdateCircumcircleDefault()
        {
            var p2 = Vertices[2];
            var p1 = Vertices[1];
            var p0 = Vertices[0];

            var div = 2 * ((p0.X * (p2.Y - p1.Y)) + (p1.X * (p0.Y - p2.Y)) + (p2.X * (p1.Y - p0.Y)));

            if (div == 0)
            {
                throw new DivideByZeroException();
            }

            var dA = (p0.X * p0.X) + (p0.Y * p0.Y);
            var dB = (p1.X * p1.X) + (p1.Y * p1.Y);
            var dC = (p2.X * p2.X) + (p2.Y * p2.Y);

            var aux1 = (dA * (p2.Y - p1.Y)) + (dB * (p0.Y - p2.Y)) + (dC * (p1.Y - p0.Y));
            var aux2 = -((dA * (p2.X - p1.X)) + (dB * (p0.X - p2.X)) + (dC * (p1.X - p0.X)));

            var center = new Vertex(aux1 / div, aux2 / div);
            Circumcenter = center;
            SquaredRadius = (center.X - p0.X).ToPow(2) + ((center.Y - p0.Y).ToPow(2));
        }
    }
}
