using Extreme.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Triangulation.Core.Algorithms.Equations.Services;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.UnitTests
{
    [TestClass]
    public class TriangulationTests
    {
        [TestMethod]
        public void MatrixEquationTest()
        {
            var m = Matrix.Create(new double[,]
                                {
                                    { 1, 2, 3 },
                                    { 2, 2, 2 },
                                    { 3, 2, 3 }
                                });
            var b = Vector.Create(6d, 6, 8);

            var x = m.Solve(b);

            foreach (var elem in x)
            {
                Assert.AreEqual(1d, elem, 10e-5);
            }
        }

        [TestMethod]
        public void AAndBSumTest()
        {
            var triangle = new Triangle((0, 0), (1, 0), (1, 1));
            var service = new SystemService(triangle.Vertices)
            {
                Triangle = triangle
            };

            var a = service.MinorCoefitients["a"];
            var b = service.MinorCoefitients["b"];
            Assert.AreEqual(0, a.Sum());
            Assert.AreEqual(0, b.Sum());
        }

        [TestMethod]
        public void CalculateMatrixTest()
        {
            var triangle = new Triangle((0, 0), (1, 0), (1, 1));
            var service = new SystemService(triangle.Vertices)
            {
                Triangle = triangle
            };
            Assert.AreEqual(1 / 2d, triangle.Square, 10e-3);

            var m = 0.5 * Matrix.Create(new double[,]
                                            {
                                                { 1d, -1, 0 },
                                                { -1, 2, -1 },
                                                { 0, -1, 1 }
                                            });
            for (var i = 0; i < m.RowCount; i++)
            {
                for (var j = 0; j < m.ColumnCount; j++)
                {
                    Assert.AreEqual(m[i, j], service.StiffnessMatrix[i, j], 10e-3);
                }
            }
        }

    }
}
