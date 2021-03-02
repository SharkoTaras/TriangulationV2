using System.Collections.Generic;

namespace Triangulation.Core.Interfaces.Entities
{
    public class Boundary
    {
        public Boundary(Vertex bottomLeft, Vertex bottomRight, Vertex topRight, Vertex topLeft)
        {
            this.BottomLeft = bottomLeft;
            this.BottomRight = bottomRight;
            this.TopRight = topRight;
            this.TopLeft = topLeft;
        }

        #region Properties
        public Vertex BottomLeft { get; }

        public Vertex BottomRight { get; }

        public Vertex TopRight { get; }

        public Vertex TopLeft { get; }

        public IEnumerable<Vertex> Vertices => new Vertex[] { this.BottomLeft, this.BottomRight, this.TopRight, this.TopLeft };
        #endregion
    }
}
