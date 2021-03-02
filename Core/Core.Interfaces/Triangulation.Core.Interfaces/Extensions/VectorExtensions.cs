using System.Collections.Generic;
using System.Runtime.Intrinsics;

namespace Triangulation.Core.Interfaces.Extensions
{
    public static class VectorExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this Vector256<T> vector) where T : struct
        {
            var result = new T[Vector256<T>.Count];

            for (var i = 0; i < Vector256<T>.Count; i++)
            {
                result[i] = vector.GetElement(i);
            }

            return result;
        }
    }
}
