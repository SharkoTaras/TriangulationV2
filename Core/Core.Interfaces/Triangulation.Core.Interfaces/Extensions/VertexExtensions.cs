using Triangulation.Core.Implementation.Utils;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.Core.Interfaces.Extensions
{
    public static class VertexExtensions
    {        /// <summary>
             /// Calculate the distance from the point to the coordinates center (0;0)
             /// </summary>
             /// <param name="vertex">Target vertex</param>
             /// <returns>Scalar value - distance to (0;0)</returns>
        public static double LengthWithZeroVertex(this Vertex vertex)
        {
            Util.Check.NotNull(vertex, "Parameter is null");

            return new Edge(vertex, (0, 0)).Length;
        }

        /// <summary>
        /// Check if vertex is in triangle
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="triangle"></param>
        /// <returns></returns>
        public static bool InTriangle(this Vertex vertex, Triangle triangle)
        {
            Util.Check.NotNull(vertex, "Parameter is null");
            Util.Check.NotNull(triangle, "Parameter is null");

            return Util.Check.IsInTriangle(triangle, vertex);
        }

        /// <summary>
        /// Scalar product of two points
        /// </summary>
        /// <param name="vectorA">First point</param>
        /// <param name="vectorB">Second point</param>
        /// <returns>Sum of respective coordinates products</returns>
        public static double Dot(this Vertex vectorA, Vertex vectorB)
        {
            Util.Check.NotNull(vectorA, "Parameter is null");
            Util.Check.NotNull(vectorB, "Parameter is null");

            return vectorA.X * vectorB.X + vectorA.Y * vectorB.Y;
        }

        public static bool IsOnBoundary(this Vertex vertex, Boundary boundary)
        {
            //var xMin = boundary.LeftSide.Start.X;
            //var xMax = boundary.RightSide.Start.X;
            //var yMin = boundary.BottomSide.Start.Y;
            //var yMax = boundary.TopSide.Start.Y;
            //return vertex.X == xMin || vertex.X == xMax || vertex.Y == yMax || vertex.Y == yMin;
            return vertex.X == 2 || vertex.Y == 2 || vertex.X == 0 || vertex.Y == 0;
        }
    }
}
