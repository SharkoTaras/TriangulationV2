using System;
using System.Collections.Generic;
using System.Diagnostics;
using Extreme.Mathematics;
using Extreme.Mathematics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Triangulation.Core.Algorithms.Equations.Services;
using Triangulation.Core.Interfaces.Collections;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.UnitTests.ServiceTests
{
    [TestClass]
    public class MatrixServiceTest
    {
        [TestMethod]
        public void IndexMatrixTest()
        {
            var vertices = new VertexCollection
            {
                (0, 0),
                (1, 0),
                (1, 1),
                (0, 1)
            };

            var t1 = new Triangle(vertices[0], vertices[1], vertices[2]);
            var t2 = new Triangle(vertices[0], vertices[2], vertices[3]);

            var service = new MatrixService(vertices, new List<Triangle> { t1, t2 });

            var im = Matrix.Create(new uint[,]
                                        {
                                            { 0, 0 },
                                            { 1, 2 },
                                            { 2, 3 }
                                        });

            Debug.WriteLine(im);
            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine(service.IndexMatrix);
            for (var i = 0; i < im.RowCount; i++)
            {
                for (var j = 0; j < im.ColumnCount; j++)
                {
                    Assert.AreEqual(im[i, j], service.IndexMatrix[i, j]);
                }
            }
        }

        [TestMethod]
        public void GlobalMatrixTest()
        {
            var mock = new Mock<SystemService>();
            mock.Setup(x => x.StiffnessMatrix).Returns((DenseMatrix<double>)(0.5 * Matrix.Create(new double[,] { { 1d, -1, 0 }, { -1, 2, -1 }, { 0, -1, 1 } })));
            mock.Setup(x => x.ForceVector).Returns((DenseVector<double>)(1 / 6d * Vector.Create<double>(1, 1, 1)));
            var m = new uint[,] { { 0, 2, 6, 3, 5, 6 }, { 1, 6, 2, 4, 6, 5 }, { 6, 1, 3, 6, 4, 0 } };
            var indexMatrix = Matrix.Create(m);
            var vertices = new VertexCollection()
            {
                (0,0), (1,0), (2,1), (2,2), (1,2), (0,1), (1,1)
            };
            var triangles = new List<Triangle>()
            {
                new Triangle(vertices[0], vertices[1], vertices[6]),
                new Triangle(vertices[2], vertices[6], vertices[1]),
                new Triangle(vertices[6], vertices[2], vertices[3]),
                new Triangle(vertices[3], vertices[4], vertices[6]),
                new Triangle(vertices[5], vertices[6], vertices[4]),
                new Triangle(vertices[6], vertices[5], vertices[0]),
            };
            var service = new MatrixService(vertices, triangles)
            {
                IndexMatrix = indexMatrix,
                SystemService = mock.Object
            };
            service.UpdateSystem();

            var gm = service.GlobalStiffnessMatrix;
            var gv = service.GlobalForceVector;
            Assert.AreEqual(1, gm[0, 0]);
            Assert.AreEqual(1.5, gm[1, 1]);
            Assert.AreEqual(1 / 3d, gv[0]);
            Assert.AreEqual(1, gv[6], 10e-2);
        }
    }
}
