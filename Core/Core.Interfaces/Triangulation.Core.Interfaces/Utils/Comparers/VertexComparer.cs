using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.Core.Interfaces.Utils.Comparers
{
    /// <summary>
    /// Equality comparer for <see cref="Vertex"/>
    /// </summary>
    public class VertexComparer : IEqualityComparer<Vertex>
    {
        /// <summary>
        /// Compare vertices by coordinates equality
        /// </summary>
        /// <param name="x">First vertex</param>
        /// <param name="y">Second vertex</param>
        /// <returns>Comparison index</returns>
        public bool Equals([AllowNull] Vertex x, [AllowNull] Vertex y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return x == y;
        }

        /// <summary>
        /// Get hesh code of the vertex object
        /// </summary>
        /// <param name="obj">Vertex object</param>
        /// <returns>Hash code value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetHashCode([DisallowNull] Vertex obj)
        {
            return obj is null ? 0 : obj.X.GetHashCode() ^ obj.Y.GetHashCode();
        }
    }
}
