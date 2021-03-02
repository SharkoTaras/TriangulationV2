using System;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.Core.Implementation.Utils
{
    public class Checker
    {
        public void NotNullOrEmpty(string str, string message = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str", message);
            }
        }

        public void NotNull(object obj, string message = null)
        {
            if (obj is null)
            {
                throw new ArgumentNullException("obj", message);
            }
        }


        /// <summary>
        /// Check if target vertex is inside the triangle.
        /// </summary>
        /// <param name="triangle">Target triangle</param>
        /// <param name="vertex">Target vertex</param>
        /// <returns><see cref="bool"/> result</returns>
        /// <remarks>
        /// Algorithm: check if vertex is situated on the same side of the half-plane, created by each triangle's side
        /// </remarks>
        public bool IsInTriangle(Triangle triangle, Vertex vertex)
        {
            Util.Check.NotNull(triangle, "Parameter is null");
            Util.Check.NotNull(vertex, "Parameter is null");

            var x0 = vertex.X;
            var x1 = triangle.First.Start.X;
            var x2 = triangle.Second.Start.X;
            var x3 = triangle.Third.Start.X;

            var y0 = vertex.Y;
            var y1 = triangle.First.Start.Y;
            var y2 = triangle.Second.Start.Y;
            var y3 = triangle.Third.Start.Y;

            var p1 = ((x1 - x0) * (y2 - y1)) - ((x2 - x1) * (y1 - y0));
            var p2 = ((x2 - x0) * (y3 - y2)) - ((x3 - x2) * (y2 - y0));
            var p3 = ((x3 - x0) * (y1 - y3)) - ((x1 - x3) * (y3 - y0));

            return Math.Sign(p1) == Math.Sign(p2) && Math.Sign(p2) == Math.Sign(p3) && Math.Sign(p1) == Math.Sign(p3);
        }
    }
}
