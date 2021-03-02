using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Extensions;

namespace Triangulation.Core.Algorithms.Implementation.Algorithms
{
    /// <summary>
    /// Fix the corner elements: only one triangle's side must be located on the boundary
    /// </summary>
    public static class TriangulationOptimization
    {
        public static async Task<IEnumerable<Triangle>> Optimize(Boundary boundary, List<Triangle> triangles)
        {
            if (triangles.Count == 2)
            {
                return triangles;
            }

            var cornerTriangles = triangles.Where(t => t.Vertices.TrueForAll(v => v.IsOnBoundary(boundary))).ToList();
            if (cornerTriangles.Any())
            {
                for (var i = 0; i < cornerTriangles.Count(); i++)
                {
                    var neighbourTriangle = cornerTriangles[i].TrianglesWithSharedEdge.First();
                    var cornerTriangle = cornerTriangles[i];
                    triangles.Remove(cornerTriangle);
                    triangles.Remove(neighbourTriangle);

                    SwapLine(ref cornerTriangle, ref neighbourTriangle);
                    cornerTriangles[i] = cornerTriangle;
                    triangles.Add(cornerTriangle);
                    triangles.Add(neighbourTriangle);
                }
            }

            return triangles;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SwapLine(ref Triangle t1, ref Triangle t2)
        {
            var t1Lines = t1.Edges;
            var t2Lines = t2.Edges;
            var toSwap = t1Lines.Intersect(t2Lines).FirstOrDefault();
            var newLineVertices = t1.Vertices
                            .Concat(t2.Vertices)
                            .Except(toSwap.Vertices)
                            .ToList();

            var verticesOfNextTriangles = t1.Vertices
                .Concat(t2.Vertices)
                .Distinct()
                .Except(newLineVertices);
            t1 = new Triangle(newLineVertices[0], newLineVertices[1], verticesOfNextTriangles.First());
            t2 = new Triangle(newLineVertices[0], newLineVertices[1], verticesOfNextTriangles.Last());
        }
    }

}
