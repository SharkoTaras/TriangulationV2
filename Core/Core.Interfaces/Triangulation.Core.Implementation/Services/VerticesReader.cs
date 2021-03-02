using System.Collections.Generic;
using System.IO;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Services;
using Triangulation.Core.Interfaces.Utils;
using Unity;

namespace Triangulation.Core.Implementation.Services
{
    public class VerticesReader : IReader<IEnumerable<Vertex>>
    {
        #region Constructors
        [InjectionConstructor]
        public VerticesReader()
        {
        }

        public VerticesReader(TextReader reader)
        {
            this.Stream = reader;
        }
        #endregion

        #region Properties
        public string EntityName => nameof(Vertex);

        public TextReader Stream { get; private set; }

        [Dependency]
        public ISerializer<Vertex> Serializer { get; set; }
        #endregion

        public IEnumerable<Vertex> Read()
        {
            var str = this.Stream.ReadToEnd();
            var result = new List<Vertex>();

            foreach (var item in str.Split(";"))
            {
                result.Add(this.Serializer.Deserialize(item));
            }
            
            return result;
        }
    }
}
