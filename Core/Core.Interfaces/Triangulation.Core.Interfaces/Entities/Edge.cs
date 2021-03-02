using System;
using System.Linq;
using Triangulation.Core.Interfaces.Extensions;

namespace Triangulation.Core.Interfaces.Entities
{
    /// <summary>
    /// Side of the polygon, IOW, the geometric segment
    /// </summary>
    public class Edge : IEntity
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Edge()
        {
            this.Start = new Vertex();
            this.End = new Vertex();
        }

        /// <summary>
        /// Constructor with two vertexes (points)
        /// </summary>
        /// <param name="firstPoint">Start <see cref="Vertex"/> value</param>
        /// <param name="secondPoint">End <see cref="Vertex"/> value</param>
        public Edge(Vertex firstPoint, Vertex secondPoint)
        {
            this.Start = firstPoint;
            this.End = secondPoint;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Start vertex (point) of the segment
        /// </summary>
        public Vertex Start { get; set; }

        /// <summary>
        /// End vertex (point) of the segment
        /// </summary>
        public Vertex End { get; set; }

        /// <summary>
        /// Scalar value representing the length of the segment
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt((this.End.X - this.Start.X).ToPow(2) + (this.End.Y - this.Start.Y).ToPow(2));
            }
        }

        /// <summary>
        /// Array of segment's vertices (points)
        /// </summary>
        /// <remarks>The amount of elements is supposed to always be 2</remarks>
        public Vertex[] Vertices
        {
            get
            {
                return new Vertex[] { this.Start, this.End };
            }
        }

        public uint Id { get; }

        public string SerializerName => GlobalConstants.Serializers.EdgeSerializer;
        #endregion

        #region Overrides
        /// <summary>
        /// Comparison method, checking for equality (<see cref="object.Equals(object)"/> overriding)
        /// </summary>
        /// <param name="obj"><see cref="object"/> parameter to compare with</param>
        /// <returns>True if equals to <paramref name="obj"/> and false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            var edge = obj as Edge;

            return this == edge;
        }

        /// <summary>
        /// Get hash code of the instance
        /// </summary>
        /// <returns>Hash code value</returns>
        public override int GetHashCode()
        {
            var hCode = (int)Start.X ^ (int)Start.Y ^ (int)End.X ^ (int)End.Y;
            return hCode.GetHashCode();
        }

        /// <summary>
        /// Display information about <see cref="Edge"/> (<see cref="object.ToString"/> overriding)
        /// </summary>
        /// <returns>Formatted string</returns>
        public override string ToString()
        {
            return $"{Start}-{End}";
        }
        #endregion

        /// <summary>
        /// Check if point is one of the segment's vertices
        /// </summary>
        /// <param name="vertex">Vertex (point) to check</param>
        /// <returns><see cref="bool"/> result</returns>
        public bool Contains(Vertex vertex)
        {
            return vertex is null ? false : Start == vertex || End == vertex;
        }

        /// <summary>
        /// Change segment (vector) start and end points
        /// </summary>
        public void Flip()
        {
            var tmp = Start;
            Start = End;
            End = tmp;
        }

        public string Serialize()
        {
            return string.Format("{{ {0} }}", string.Join(", ", this.Vertices.Select(vert => vert.Id.ToString())));
        }

        #region Operators
        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="left">Left <see cref="Edge"/> object</param>
        /// <param name="right">Right <see cref="Edge"/> object</param>
        /// <returns><see cref="bool"> result of <see cref="Edge"/> objects' equality</returns>
        public static bool operator ==(Edge left, Edge right)
        {
            return left?.Start == right?.Start && left?.End == right?.End
                || left?.Start == right?.End && left?.End == right.Start;
        }

        /// <summary>
        /// Not-equal operator
        /// </summary>
        /// <param name="left">Left <see cref="Edge"/> object</param>
        /// <param name="right">Right <see cref="Edge"/> object</param>
        /// <returns>True if <see cref="Edge"/> objects are not equal, false otherwise</returns>
        public static bool operator !=(Edge left, Edge right)
        {
            return !(left == right);
        }
        #endregion
    }
}
