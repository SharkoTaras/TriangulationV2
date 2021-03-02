using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Triangulation.Core.Interfaces.Entities
{
    public class Vertex : IEntity
    {
        #region Constructors
        public Vertex() : this(0, 0)
        {
        }

        public Vertex(double x, double y)
        {
            AdjacentTriangles = new HashSet<Triangle>();

            X = x;
            Y = y;
        }
        #endregion

        #region Properties
        public uint Id { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public string SerializerName => GlobalConstants.Serializers.VertexSerializer;

        public HashSet<Triangle> AdjacentTriangles { get; }
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            if (obj is Vertex v)
            {
                return this == v;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => Id.GetHashCode() ^ X.GetHashCode() ^ Y.GetHashCode();

        public override string ToString() => string.Format($"({X:0.00000}, {Y:0.00000})");
        #endregion

        #region Operators
        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="left">Left <see cref="Vertex"/> object</param>
        /// <param name="right">Right <see cref="Vertex"/> object</param>
        /// <returns>Result of objects' equality</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vertex left, Vertex right)
        {
            if (left is null || right is null)
            {
                return false;
            }

            return left.X == right.X && left.Y == right.Y;
        }

        /// <summary>
        /// Not-equal operator
        /// </summary>
        /// <param name="left">Left <see cref="Vertex"/> object</param>
        /// <param name="right">Right <see cref="Vertex"/> object</param>
        /// <returns>True if objects are not equal, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vertex left, Vertex right) => !(left == right);

        /// <summary>
        /// Implicit cast operator for <see cref="Vertex"/>, based on a tuple of <see cref="float"/>
        /// </summary>
        /// <param name="value">Tuple with two <see cref="float"/> coordinates values</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vertex((float x, float y) value) => new Vertex(value.x, value.y);

        /// <summary>
        /// Implicit cast operator for <see cref="Vertex"/>, based on a tuple of <see cref="double"/>
        /// </summary>
        /// <param name="value">Tuple with two <see cref="double"/> coordinates values</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vertex((double x, double y) value) => new Vertex(value.x, value.y);

        /// <summary>
        /// Implicit cast operator for <see cref="Vertex"/>, based on a tuple of <see cref="int"/>
        /// </summary>
        /// <param name="value">Tuple with two <see cref="int"/> coordinates values</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vertex((int x, int y) value) => new Vertex(value.x, value.y);

        /// <summary>
        /// Subtraction operator for <see cref="Vertex"/>
        /// </summary>
        /// <param name="left">Minuend <see cref="Vertex"/> value</param>
        /// <param name="rigth">Subtrahend <see cref="Vertex"/> value</param>
        /// <returns>Remainder (resulting <see cref="Vertex"/> value after subtraction)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vertex operator -(Vertex left, Vertex rigth) => new Vertex(left.X - rigth.X, left.Y - rigth.Y);

        /// <summary>
        /// Subtraction operator for <see cref="Vertex"/>
        /// </summary>
        /// <param name="left">Minuend <see cref="Vertex"/> value</param>
        /// <param name="number">Subtrahend <see cref="double"/> value (scalar)</param>
        /// <returns>Remainder (resulting <see cref="Vertex"/> value after subtraction)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vertex operator -(Vertex left, double number) => new Vertex(left.X - number, left.Y - number);

        /// <summary>
        /// Addition operator for <see cref="Vertex"/>
        /// </summary>
        /// <param name="left">First addition <see cref="Vertex"/> value</param>
        /// <param name="rigth">Second addition <see cref="Vertex"/> value</param>
        /// <returns>Resulting <see cref="Vertex"/> value after addition</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vertex operator +(Vertex left, Vertex rigth) => new Vertex(left.X + rigth.X, left.Y + rigth.Y);

        /// <summary>
        /// Addition operator for <see cref="Vertex"/>
        /// </summary>
        /// <param name="left">First addition <see cref="Vertex"/> value</param>
        /// <param name="number">Second addition <see cref="double"/> value (scalar)</param>
        /// <returns>Resulting <see cref="Vertex"/> value after addition</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vertex operator +(Vertex left, double number) => new Vertex(left.X + number, left.Y + number);

        /// <summary>
        /// Multiplication operator for <see cref="Vertex"/>
        /// </summary>
        /// <param name="vertex">First multiplicant <see cref="Vertex"/> value</param>
        /// <param name="number">Second multiplicant <see cref="double"/> value (scalar)</param>
        /// <returns>Product <see cref="Vertex"/> value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vertex operator *(Vertex vertex, double number) => new Vertex(vertex.X * number, vertex.Y * number);

        /// <summary>
        /// Multiplication operator for <see cref="Vertex"/>
        /// </summary>
        /// <param name="number">First multiplicant <see cref="double"/> value (scalar)</param>
        /// <param name="vertex">Second multiplicant <see cref="Vertex"/> value</param>
        /// <returns>Product <see cref="Vertex"/> value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vertex operator *(double number, Vertex vertex) => new Vertex(vertex.X * number, vertex.Y * number);
        #endregion
    }
}
