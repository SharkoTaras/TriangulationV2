using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Extreme.Mathematics;
using Extreme.Mathematics.LinearAlgebra;
using Triangulation.Core.Implementation.Utils;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.Core.Algorithms.Equations.Services
{
    public class SystemService
    {
        private Triangle triangle;
        #region Constructor
        public SystemService()
        {
        }

        public SystemService(IEnumerable<Vertex> vertices)
        {
            Vertices = vertices;
            VerticesCount = Vertices.Count();
        }
        #endregion

        #region Properties
        public IEnumerable<Vertex> Vertices { get; }

        public int VerticesCount { get; private set; }

        public Triangle Triangle
        {
            get => triangle;

            set
            {
                if (triangle != value)
                {
                    Debug.WriteLine(nameof(SystemService));
                    Debug.WriteLine("New triangle");
                    triangle = value;
                    if (Vertices != null)
                    {
                        MainMatrix();
                    }

                    Debug.WriteLine("New matrix:");
                    Debug.WriteLine(StiffnessMatrix);
                    if (Vertices != null)
                    {
                        CalculateVectorF();
                    }

                    Debug.WriteLine("New vector");
                    Debug.WriteLine(ForceVector);
                }
            }
        }

        public Dictionary<string, DenseVector<double>> MinorCoefitients { get; private set; }

        public virtual DenseMatrix<double> StiffnessMatrix { get; set; }

        public virtual DenseVector<double> ForceVector { get; private set; }

        #endregion

        #region Public Methods
        public void CalculateVectorF()
        {
            Util.Check.NotNull(triangle, "Please, set the triangle before calculating vector F");
            var F = Triangle.Square * EquationConstatns.f / 3 * Vector.Create<double>(1, 1, 1);

            ForceVector = (DenseVector<double>)F;
        }
        #endregion


        private Dictionary<string, DenseVector<double>> CalculateMinorCoefs()
        {
            Util.Check.NotNull(Triangle, "Can't calculate minor coefs for null triangle object");

            var vertices = triangle.Vertices.ToArray();

            var third = vertices[2];
            var second = vertices[1];
            var first = vertices[0];

            var minorCoefs = new Dictionary<string, DenseVector<double>>(Vertices.Count());
            // 1
            var a1 = second.Y - third.Y;
            var b1 = -(second.X - third.X);
            var c1 = (second.X * third.Y) - (third.X * second.Y);
            // 2
            var a2 = -(first.Y - third.Y);
            var b2 = first.X - third.X;
            var c2 = -((first.X * third.Y) - (third.X * first.Y));
            // 3
            var a3 = first.Y - second.Y;
            var b3 = -(first.X - second.X);
            var c3 = (first.X * second.Y) - (second.X * first.Y);

            minorCoefs["a"] = Vector.Create(a1, a2, a3);
            minorCoefs["b"] = Vector.Create(b1, b2, b3);
            minorCoefs["c"] = Vector.Create(c1, c2, c3);

            MinorCoefitients = minorCoefs;

            return minorCoefs;
        }

        private void MainMatrix()
        {
            var coefs = CalculateMinorCoefs();

            var k1T = Matrix.Create<double>(3, 1, MatrixElementOrder.RowMajor).AddInPlace(coefs["a"], Dimension.Rows);
            var k1V = Matrix.Create<double>(1, 3, MatrixElementOrder.RowMajor).AddInPlace(coefs["a"], Dimension.Columns);
            var k2T = Matrix.Create<double>(3, 1, MatrixElementOrder.RowMajor).AddInPlace(coefs["b"], Dimension.Rows);
            var k2V = Matrix.Create<double>(1, 3, MatrixElementOrder.RowMajor).AddInPlace(coefs["b"], Dimension.Columns);

            var vec = Vector.Create(0d, 0, 0);
            for (var i = 0; i < vec.Length; i++)
            {
                vec[i] = ((coefs["a"][i] * Triangle.Circumcenter.X) + (coefs["b"][i] * Triangle.Circumcenter.Y) + coefs["c"][i]) / (2 * Triangle.Square);
            }

            var m1T = Matrix.Create<double>(3, 1, MatrixElementOrder.RowMajor).AddInPlace(vec, Dimension.Rows);
            var m1V = Matrix.Create<double>(1, 3, MatrixElementOrder.RowMajor).AddInPlace(vec, Dimension.Columns);

            StiffnessMatrix = (DenseMatrix<double>)((((EquationConstatns.a11 * (k1T * k1V)) +
                                                      (EquationConstatns.a22 * (k2T * k2V))) / (4 * triangle.Square)) +
                                                      (EquationConstatns.d * (m1T * m1V)));
        }
    }
}
