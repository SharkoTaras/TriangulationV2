using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Triangulation.Core.Algorithms.Interfaces.Enums;
using Triangulation.Core.Algorithms.Interfaces.Models;
using Triangulation.Core.Implementation.Services;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Services;
using Unity;

namespace Triangulation.UnitTests.ReaderTests
{
    [TestClass]
    public class ReaderTests : TestBase
    {
        private IReader<Region> Reader;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            var reader = new StringReader("{\"vertices\": \"(1,2);(3,2);(1,1)\", \"type\": \"Vertex\"}");
            new VertexRegionInitializer(reader).Initialize(this.Container);
            this.Reader = this.Container.Resolve<IReader<Region>>();
        }

        [TestMethod]
        public void VertexReaderTest()
        {
            var r = this.Container.Resolve<IReader<IEnumerable<Vertex>>>();
            var vertices = r.Read().ToList();

            var expected = new List<Vertex> { (1, 2), (3, 2), (1, 1) };
            this.AreEqual(expected, vertices);
        }

        [TestMethod]
        public void RegionReaderTest()
        {
            var expected = new List<Vertex> { (1, 2), (3, 2), (1, 1) };

            var region = this.Reader.Read();

            this.AreEqual(expected, (List<Vertex>)region.Entities);
            Assert.AreEqual(RegionInitializingType.Vertex, region.InitializingType);
        }

        public void AreEqual(List<Vertex> expected, List<Vertex> current)
        {
            Assert.AreEqual(expected.Count, current.Count);

            for (var i = 0; i < current.Count; i++)
            {
                Assert.AreEqual(expected[i], current[i]);
            }
        }
    }
}
