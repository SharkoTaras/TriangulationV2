using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Triangulation.Core.Implementation.Utils;
using Triangulation.Core.Interfaces.Utils;

namespace Triangulation.Core.Algorithms.Equations.Entities
{
    public class Vector : List<double>, ISerializable
    {
        public string SerializerName { get; }

        public Vector() : base() { }

        public Vector(int capacity) : base(capacity) { }

        public Vector(IEnumerable<double> collection) : base(collection) { }

        public Vector(IEnumerable<float> collection) : base((collection as List<float>).Count)
        {
            foreach (var item in collection)
            {
                this.Add(item);
            }
        }

        public static Vector operator +(Vector left, Vector right)
        {
            Util.Check.NotNull(left, "Null left vector value while adding vectors");
            Util.Check.NotNull(right, "Null right vector value while adding vectors");
            if (left.Count != right.Count)
            {
                throw new Exception("Cannot add two vectors with non-equal dimentions");
            }

            var result = new Vector(left.Capacity);

            Parallel.For(0, left.Count + 1, (i) =>
            {
                result[i] = left[i] + right[i];
            });
            return result;
        }

        #region ISerializable implementation
        public string Serialize()
        {
            return string.Join(" ", this.ToArray());
        }
        #endregion

    }
}
