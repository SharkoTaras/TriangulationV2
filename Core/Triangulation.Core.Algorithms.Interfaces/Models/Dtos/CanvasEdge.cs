using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Entities.Dtos;

namespace Triangulation.Core.Algorithms.Interfaces.Models.Dtos
{
    public class CanvasEdge
    {
        public CanvasEdge(Edge primaryEdge, uint zoom)
        {
            this.PrimaryEdge = primaryEdge;
            this.Zoom = zoom;
        }

        public Edge PrimaryEdge { get; }

        public uint Zoom { get; }

        public CanvasVertex Start => new CanvasVertex(this.PrimaryEdge.Start, this.Zoom);

        public CanvasVertex End => new CanvasVertex(this.PrimaryEdge.End, this.Zoom);
    }
}
