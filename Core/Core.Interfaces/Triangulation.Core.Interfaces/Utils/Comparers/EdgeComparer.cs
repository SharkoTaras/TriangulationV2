using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.Core.Interfaces.Utils.Comparers
{
    /// <summary>
    /// Equality comparer for <see cref="Edge"/>
    /// </summary>
    public class EdgeComparer : IEqualityComparer<Edge>
    {
        /// <summary>
        /// Compare sides by vertices equality
        /// </summary>
        /// <param name="x">First side</param>
        /// <param name="y">Second side</param>
        /// <returns>Comparison index</returns>
        public bool Equals([AllowNull] Edge x, [AllowNull] Edge y)
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
        /// Get hesh code of the side object
        /// </summary>
        /// <param name="obj">Side object</param>
        /// <returns>Hash code value</returns>
        public int GetHashCode([DisallowNull] Edge obj)
        {
            return obj is null ? 0 : obj.Start.GetHashCode() ^ obj.End.GetHashCode();
        }
    }
}
