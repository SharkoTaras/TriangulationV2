using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Triangulation.Core.Implementation.Serializers;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Utils;

namespace Triangulation.UnitTests.SerializersTests
{
    [TestClass]
    public class VertexSerializerTest
    {
        private ISerializer<Vertex> Serializer;

        [TestInitialize]
        public void TestInitialize()
        {
            this.Serializer = new VertexSerializer();
        }

        [TestMethod]
        public void Deserialize_Should_Return_Vertex()
        {
            var vertex = this.Serializer.Deserialize("(1,1)");

            Assert.IsNotNull(vertex);
            Assert.AreEqual((1, 1), vertex);

            var vertex2 = this.Serializer.Deserialize("  ( 2  , 3  )   ");
            Assert.IsNotNull(vertex2);
            Assert.AreEqual((2, 3), vertex2);
        }

        [TestMethod]
        public void Deserialize_Should_Return_Null()
        {
            var vertex = this.Serializer.Deserialize("");

            Assert.IsNull(vertex);

            var vertex2 = this.Serializer.Deserialize(null);

            Assert.IsNull(vertex2);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Deserialize_Should_Throw_FormatException()
        {
            this.Serializer.Deserialize("(1,dfd)");
        }

        [TestMethod]
        public void Serialize_Should_Return_String()
        {
            var str = this.Serializer.Serialize((1, 1));

            Assert.AreEqual("(1,1)", str);
        }

        [TestMethod]
        public void Serialize_Should_Return_Null()
        {
            var str = this.Serializer.Serialize(null);

            Assert.IsNull(str);
        }
    }
}
