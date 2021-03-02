using System.Collections.Generic;
using System.Diagnostics;
using Extreme.Mathematics;
using Extreme.Mathematics.LinearAlgebra;
using Triangulation.Core.Interfaces.Collections;
using Triangulation.Core.Interfaces.Entities;

namespace Triangulation.Core.Algorithms.Equations.Services
{
    public class MatrixService
    {
        #region Constructors
        public MatrixService()
        {
        }

        public MatrixService(VertexCollection vertices, List<Triangle> triangles)
        {
            Vertices = vertices;
            Triangles = triangles;
            SystemService = new SystemService(Vertices);
            GlobalStiffnessMatrix = Matrix.Create<double>(vertices.Count, vertices.Count);
            GlobalForceVector = Vector.Create<double>(vertices.Count);
            CalculateIndexMatrix();
        }
        #endregion

        #region Properties
        public VertexCollection Vertices { get; }

        public List<Triangle> Triangles { get; }

        public SystemService SystemService { get; set; }
        #endregion

        public DenseMatrix<double> GlobalStiffnessMatrix { get; private set; }

        public DenseVector<double> GlobalForceVector { get; private set; }

        public DenseMatrix<uint> IndexMatrix { get; set; }

        public void UpdateSystem()
        {
            for (var i = 0; i < Triangles.Count; i++)
            {
                var triangle = Triangles[i];
                Debug.WriteLine(triangle);
                SystemService.Triangle = triangle;
                var sm = SystemService.StiffnessMatrix;
                Debug.WriteLine(sm);
                var fv = SystemService.ForceVector;
                Debug.WriteLine(fv);
                for (var m = 0; m < sm.RowCount; m++)
                {
                    for (var n = 0; n < sm.ColumnCount; n++)
                    {
                        GlobalStiffnessMatrix[(int)IndexMatrix[m, i], (int)IndexMatrix[n, i]] += sm[m, n];
                    }
                    GlobalForceVector[(int)IndexMatrix[m, i]] += fv[m];
                }
            }
        }

        #region Overrides
        public override string ToString() => base.ToString();
        #endregion

        private void CalculateIndexMatrix()
        {
            IndexMatrix = Matrix.Create<uint>(3, Triangles.Count);
            for (var i = 0; i < Triangles.Count; i++)
            {
                var vertices = Triangles[i].Vertices;
                IndexMatrix[2, i] = vertices[2].Id;
                IndexMatrix[1, i] = vertices[1].Id;
                IndexMatrix[0, i] = vertices[0].Id;
            }
            Debug.WriteLine("Index matrix:");
            Debug.WriteLine(IndexMatrix);
        }
    }
}
