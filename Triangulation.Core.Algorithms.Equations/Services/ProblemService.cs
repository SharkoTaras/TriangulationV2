using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Extreme.Mathematics;
using Extreme.Mathematics.LinearAlgebra;
using Triangulation.Core.Implementation.Utils;
using Triangulation.Core.Interfaces.Entities;
using Triangulation.Core.Interfaces.Extensions;

namespace Triangulation.Core.Algorithms.Equations.Services
{
    public class ProblemService
    {
        private Boundary boundary;

        #region Constructors
        public ProblemService()
        {
        }

        public ProblemService(IEnumerable<Vertex> vertices, List<Triangle> triangles, Boundary boundary)
        {
            Util.Check.NotNull(vertices);
            Util.Check.NotNull(triangles);
            Util.Check.NotNull(boundary, "Boundary vertices are undefined");

            Count = vertices.Count();

            Vertices = vertices;
            Triangles = triangles;
            this.boundary = boundary;

            MatrixA = Matrix.Create<double>(Count, Count);
            VectorF = Vector.Create<double>(Count);
        }
        #endregion

        #region Properties
        public IEnumerable<Vertex> Vertices { get; set; }

        public List<Triangle> Triangles { get; set; }

        public DenseMatrix<double> MatrixA { get; set; }

        public DenseVector<double> VectorF { get; set; }

        public List<DenseMatrix<double>> ElementMatrices { get; set; }

        public double[] Error { get; set; }

        public double[] CorrectAnswer { get; set; }

        public int Count { get; set; }

        public Boundary Boundary
        {
            get => boundary;
            set
            {
                Util.Check.NotNull(value, "Boundary vertices are undefined");
                boundary = value;
            }
        }
        #endregion

        #region Problem solving
        public List<double> SolveProblem(DenseMatrix<double> matrix, DenseVector<double> vector)
        {
            Util.Check.NotNull(Vertices);
            Util.Check.NotNull(Triangles);
            Util.Check.NotNull(boundary, "Boundary vertices are undefined");

            MatrixA = matrix;
            VectorF = vector;

            Debug.WriteLine("Initial matrix:\r\n");
            Debug.WriteLine(MatrixA);

            UpdateProblem();

            var ans = MatrixA.Solve(VectorF);
            SolutionError(ans.ToArray());
            return (-ans).ToList();
        }

        private void SolutionError(double[] sol)
        {
            var sorted = Vertices.Where(v => !v.IsOnBoundary(Boundary)).OrderBy(v => v.Id).ToList();
            Error = new double[sorted.Count];
            for (var i = 0; i < Error.Length; i++)
            {
                Error[i] = Math.Abs(sol[i] - EquationConstatns.u(sorted[i].X, sorted[i].Y));
            }
        }

        private void UpdateProblem()
        {
            var boundaryVertexIds = Vertices.Where(v => v.IsOnBoundary(Boundary)).Select(v => (int)v.Id).OrderBy(i => i).ToList();
            var boundaryVertexCount = boundaryVertexIds.Count;
            var newMatrix = Matrix.Create<double>(Vertices.Count() - boundaryVertexCount, Vertices.Count() - boundaryVertexCount);
            var newVector = Vector.Create<double>(Vertices.Count() - boundaryVertexCount);

            var j = 0;
            var indeciesToUse = Enumerable.Range(0, Vertices.Count()).Except(boundaryVertexIds).ToList();
            foreach (var ii in indeciesToUse)
            {
                newVector[j++] = VectorF[ii];
            }

            var i = 0;
            j = 0;
            foreach (var ii in indeciesToUse)
            {
                foreach (var jj in indeciesToUse)
                {
                    newMatrix[i, j] = MatrixA[ii, jj];
                    j++;
                }
                i++;
                j = 0;
            }
            MatrixA = newMatrix;
            Debug.WriteLine("New matrix: \r\n");
            Debug.WriteLine(MatrixA);
            VectorF = newVector;
            Debug.WriteLine("New vector: \r\n");
            Debug.WriteLine(VectorF);
        }

        //public (double[,], double[]) OptimizeSLAE(double[,] A, double[] F)
        //{
        //    Util.Check.NotNull(A);
        //    Util.Check.NotNull(F);

        //    var aMatr = new Matrix(A);
        //    var fmatr = new Matrix(F.Length, 1);
        //    for (var i = 0; i < F.Length; i++)
        //    {
        //        fmatr[i, 0] = F[i];
        //    }
        //    return this.OptimizeSLAE(aMatr, fmatr);
        //}


        //public (double[,], double[]) OptimizeSLAE(Matrix A, Matrix F)
        //{
        //    Util.Check.NotNull(this._boundary, "Boundary vertices are undefined");

        //    var vCount = this._boundary.Vertices.Count();
        //    int newDimention = this.Count - vCount;
        //    var changedMatrix = new double[newDimention, newDimention];
        //    var changedVector = new double[newDimention];

        //    var rowIndex = 0;
        //    int columnIndex;

        //    var boundaryVerticesIDs = this._boundary.Vertices.Select(v => v.Id);

        //    for (uint i = 0; i < A.Rows; i++)
        //    {
        //        if (!boundaryVerticesIDs.Contains(i))
        //        {
        //            // Manage vector
        //            changedVector[rowIndex] = F[i, 0];

        //            // Manage matrix
        //            columnIndex = 0;
        //            for (uint j = 0; j < A.Rows; j++)
        //            {
        //                if (!boundaryVerticesIDs.Contains(j))
        //                {
        //                    changedMatrix[rowIndex, columnIndex] = A[i, j];
        //                    columnIndex++;
        //                }
        //            }
        //            rowIndex++;
        //        }
        //    }
        //    return (changedMatrix, changedVector);
        //}

        private double[] BuildSolution(double[] slae, double boundaryCoef)
        {
            Util.Check.NotNull(boundary, "Boundary vertices are undefined");

            var fullDimention = slae.Length + boundary.Vertices.Count();
            var result = new double[fullDimention];

            var boundaryVerticesIDs = boundary.Vertices.Select(v => v.Id);
            var slaeCoefIndex = 0;
            for (uint i = 0; i < fullDimention; i++)
            {
                if (boundaryVerticesIDs.Contains(i))
                {
                    result[i] = boundaryCoef;
                }
                else
                {
                    result[i] = slae[slaeCoefIndex];
                    slaeCoefIndex++;
                }
            }
            CorrectAnswer = Vertices.Select(v => EquationConstatns.u(v.X, v.Y)).ToArray();
            return result;
        }
        #endregion
    }
}
