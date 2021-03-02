using System;
using System.Collections.Generic;
using System.IO;
using Triangulation.Core.Algorithms.Interfaces.Enums;
using Triangulation.Core.Algorithms.Interfaces.Models;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Services;
using Unity;

namespace Triangulation.Core.Implementation.Services
{
    public class RegionReader : IReader<Region>, IDisposable
    {
        #region Privete fields
        private bool isDisposed = false;
        #endregion

        #region Constructors
        [InjectionConstructor]
        public RegionReader()
        {
        }

        public RegionReader(TextReader regionFileReader)
        {
            this.Reader = regionFileReader;
        }
        #endregion

        #region Properties
        public string EntityName => nameof(Region);

        [Dependency]
        public IReader<IEnumerable<Vertex>> VerticesReader { get; set; }

        public TextReader Reader { get; private set; }
        #endregion

        public Region Read()
        {
            var vertices = this.VerticesReader.Read();
            var type = this.Reader.ReadToEnd();
            return new Region(vertices, Enum.Parse<RegionInitializingType>(type, ignoreCase: true));
        }

        #region IDisposable
        public void Dispose()
        {
            this.Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.Reader.Close();
                    this.Reader.Dispose();
                    this.isDisposed = true;
                }
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
