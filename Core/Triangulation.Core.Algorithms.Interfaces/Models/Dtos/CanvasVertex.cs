namespace Triangulation.Core.Interfaces.Entities.Dtos
{
    public class CanvasVertex : ICanvasEnity
    {
        public CanvasVertex(Vertex primaryVertex, uint zoom)
        {
            this.Zoom = zoom;
            this.PrimaryVertex = primaryVertex;
        }

        public Vertex PrimaryVertex { get; }

        public uint Id => this.PrimaryVertex.Id;

        public uint Zoom { get; }

        public double X => this.PrimaryVertex.X * this.Zoom;

        public double Y => -this.PrimaryVertex.Y * this.Zoom;
    }
}