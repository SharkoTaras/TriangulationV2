using System.Collections.Generic;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.Core.Interfaces.Collections
{
    public class VertexCollection : List<Vertex>
    {
        private uint Id;
        public VertexCollection()
        {
            this.Id = 0;
        }

        public new void Add(Vertex vertex)
        {
            vertex.Id = this.Id++;
            base.Add(vertex);
        }
    }
}
