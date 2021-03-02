using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Triangulation.Core.Algorithms.Interfaces.Models;
using Triangulation.Core.Implementation.Serializers;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Services;
using Triangulation.Core.Interfaces.Utils;
using Unity;

namespace Triangulation.Core.Implementation.Services
{
    public class VertexRegionInitializer : IInitializer
    {
        #region Cotstructors
        public VertexRegionInitializer(TextReader reader)
        {
            this.Reader = reader;
        }

        #endregion

        #region Properties
        public TextReader Reader { get; }
        #endregion

        public void Initialize(IUnityContainer container)
        {
            var json = this.Reader.ReadToEnd();
            var doc = JsonDocument.Parse(json).RootElement;

            container.RegisterType<ISerializer<Vertex>, VertexSerializer>();
            container.RegisterFactory<IReader<IEnumerable<Vertex>>>((c) =>
            {
                return new VerticesReader(new StringReader(doc.GetProperty("vertices").GetString()))
                {
                    Serializer = c.Resolve<ISerializer<Vertex>>()
                };
            });

            container.RegisterFactory<IReader<Region>>((c) =>
            {
                return new RegionReader(new StringReader(doc.GetProperty("type").GetString()))
                {
                    VerticesReader = c.Resolve<IReader<IEnumerable<Vertex>>>()
                };
            });
        }
    }
}
