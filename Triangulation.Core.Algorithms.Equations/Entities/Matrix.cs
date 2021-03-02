using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Triangulation.Core.Implementation.Utils;
using Triangulation.Core.Interfaces.Utils;

namespace Triangulation.Core.Algorithms.Equations.Entities
{
    public class Matrix : ICloneable, IEnumerable<double>, ISerializable
    {
        private readonly double[,] _array;

        #region Constructors
        /// <summary>
        /// Default ctor, creates matrix 3x3
        /// </summary>
        public Matrix()
        {
            this._array = new double[3, 3];
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    this._array[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Matrix(int rows, int columns)
        {
            if (rows > int.MaxValue || rows < 0)
            {
                throw new ArgumentOutOfRangeException("rows", "message");
            }

            if (columns > int.MaxValue || columns < 0)
            {
                throw new ArgumentOutOfRangeException("columns", "message");
            }

            this._array = new double[rows, columns];
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    this._array[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class with the specified elements.
        /// </summary>
        /// <param name="array">An array of <see cref="double"/>values that represents the elements of this Matrix.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Matrix(double[,] array)
        {
            Util.Check.NotNull(array, "Null array value");

            this._array = new double[array.GetLength(0), array.GetLength(1)];

            for (var i = 0; i < this.Rows; i++)
            {
                for (var j = 0; j < this.Columns; j++)
                {
                    this._array[i, j] = array[i, j];
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class with the specified elements.
        /// </summary>
        /// <param name="array">An array of <see cref="float"/> values that represents the elements of this Matrix.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Matrix(float[,] array)
        {
            Util.Check.NotNull(array, "Null array value");

            this._array = new double[array.GetLength(0), array.GetLength(1)];

            for (var i = 0; i < this.Rows; i++)
            {
                for (var j = 0; j < this.Columns; j++)
                {
                    this._array[i, j] = array[i, j];
                }
            }
        }
        #endregion

        public double this[uint row, uint column]
        {
            get => this[(int)row, (int)column];
            set => this[(int)row, (int)column] = value;
        }

        /// <summary>
        /// Allows instances of a Matrix to be indexed just like arrays.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <exception cref="ArgumentException"></exception>
        public double this[int row, int column]
        {
            get
            {
                if (row >= this.Rows || column >= this.Columns || row < 0 || column < 0)
                {
                    throw new ArgumentException("Matrix index out of range.");
                }

                return this._array[row, column];
            }
            set
            {
                if (row >= this.Rows || column >= this.Columns || row < 0 || column < 0)
                {
                    throw new ArgumentException("Matrix index out of range.");
                }

                this._array[row, column] = value;
            }
        }

        #region Properties
        /// <summary>
        /// Number of rows
        /// </summary>
        public int Rows
        {
            get => this._array.GetLength(0);
        }

        /// <summary>
        /// Number of columns
        /// </summary>
        public int Columns
        {
            get => this._array.GetLength(1);
        }

        /// <summary>
        /// Gets an array of floating-point values that represents the elements of this Matrix
        /// </summary>
        public double[,] Array
        {
            get => this._array;
        }
        public string SerializerName { get; }
        #endregion

        #region ICloneable implementation
        /// <summary>
        /// Creates a deep copy of this Matrix.
        /// </summary>
        /// <returns>A deep copy of the current object.</returns>
        public object Clone()
        {
            var copy = new Matrix(this.Rows, this.Columns);
            for (var i = 0; i < this.Rows; i++)
            {
                for (var j = 0; j < this.Columns; j++)
                {
                    copy[i, j] = this[i, j];
                }
            }
            return copy;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Adds two matrices.
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns>New <see cref="Matrix"/> object which is sum of two matrices.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MatrixException"></exception>
        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            Util.Check.NotNull(matrix1, "Null left matrix value in + operation");
            Util.Check.NotNull(matrix2, "Null right matrix value in + operation");
            return matrix1.Add(matrix2);
        }

        /// <summary>
        /// Subtracts two matrices.
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns>New <see cref="Matrix"/> object which is subtraction of two matrices</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MatrixException"></exception>
        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            Util.Check.NotNull(matrix1, "Null left matrix value in - operation");
            Util.Check.NotNull(matrix2, "Null right matrix value in - operation");
            return matrix1.Subtract(matrix2);
        }

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns>New <see cref="Matrix"/> object which is multiplication of two matrices.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MatrixException"></exception>
        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            Util.Check.NotNull(matrix1, "Null left matrix value in * operation");
            Util.Check.NotNull(matrix2, "Null right matrix value in * operation");
            return matrix1.Multiply(matrix2);
        }
        #endregion

        #region Operators as methods
        /// <summary>
        /// Adds <see cref="Matrix"/> to the current matrix.
        /// </summary>
        /// <param name="matrix"><see cref="Matrix"/> for adding.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MatrixException"></exception>
        public Matrix Add(Matrix matrix)
        {
            Util.Check.NotNull(matrix, "Null matrix value in Add operation");
            if (this.Rows != matrix.Rows || this.Columns != matrix.Columns)
            {
                throw new Exception();
            }

            var sumMatrix = new Matrix(this.Rows, this.Columns);
            for (var i = 0; i < this.Rows; i++)
            {
                for (var j = 0; j < this.Columns; j++)
                {
                    sumMatrix[i, j] = this[i, j] + matrix[i, j];
                }
            }
            return sumMatrix;
        }

        /// <summary>
        /// Subtracts <see cref="Matrix"/> from the current matrix.
        /// </summary>
        /// <param name="matrix"><see cref="Matrix"/> for subtracting.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MatrixException"></exception>
        public Matrix Subtract(Matrix matrix)
        {
            Util.Check.NotNull(matrix, "Null matrix value in Subtract operation");
            if (this.Rows != matrix.Rows || this.Columns != matrix.Columns)
            {
                throw new Exception();
            }

            var subtractMatrix = new Matrix(this.Rows, this.Columns);
            for (var i = 0; i < this.Rows; i++)
            {
                for (var j = 0; j < this.Columns; j++)
                {
                    subtractMatrix[i, j] = this[i, j] - matrix[i, j];
                }
            }
            return subtractMatrix;
        }

        /// <summary>
        /// Multiplies <see cref="Matrix"/> on the current matrix.
        /// </summary>
        /// <param name="matrix"><see cref="Matrix"/> for multiplying.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MatrixException"></exception>
        public Matrix Multiply(Matrix matrix)
        {
            Util.Check.NotNull(matrix, "Null matrix value in multiply operation");
            if (this.Columns != matrix.Rows)
            {
                throw new Exception();
            }

            var multiplyMatrix = new Matrix(this.Rows, matrix.Columns);
            for (var i = 0; i < this.Rows; i++)
            {
                for (var j = 0; j < matrix.Columns; j++)
                {
                    double sum = 0;
                    for (var k = 0; k < this.Columns; k++)
                    {
                        sum += this[i, k] * matrix[k, j];
                    }
                    multiplyMatrix[i, j] = sum;
                }
            }
            return multiplyMatrix;
        }
        #endregion

        #region object overridings
        /// <summary>
        /// Tests if <see cref="Matrix"/> is identical to this Matrix.
        /// </summary>
        /// <param name="obj">Object to compare with. (Can be null)</param>
        /// <returns>True if matrices are equal, false if are not equal.</returns>
        /// <exception cref="InvalidCastException">Thrown when object has wrong type.</exception>
        /// <exception cref="MatrixException">Thrown when matrices are incomparable.</exception>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }
            else
            {
                Matrix matrix;
                if (obj is Matrix)
                {
                    matrix = obj as Matrix;
                }
                else if (obj is double[,])
                {
                    matrix = new Matrix(obj as double[,]);
                }
                else if (obj is double)
                {
                    matrix = new Matrix(0, 0);
                    matrix[0, 0] = (double)obj;
                }
                else
                {
                    matrix = new Matrix(0, 0);
                }
                if (this.Rows == matrix.Rows && this.Columns == matrix.Columns)
                {
                    for (var i = 0; i < this.Rows; i++)
                    {
                        for (var j = 0; j < this.Columns; j++)
                        {
                            if (this[i, j] != matrix[i, j])
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            for (var i = 0; i < this.Rows; i++)
            {
                for (var j = 0; j < this.Columns; j++)
                {
                    hashCode += this._array[i, j].GetHashCode();
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            res.AppendLine();
            for (var i = 0; i < this.Rows; i++)
            {
                for (var j = 0; j < this.Columns; j++)
                {
                    res.Append($"{this._array[i, j]}  ");
                }
                res.AppendLine();
            }
            return res.ToString();
        }
        #endregion

        #region IEnumerable<double> implementation
        public IEnumerator<double> GetEnumerator()
        {
            foreach (var raw in this.Array)
            {
                yield return raw;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region ISerializable implementation
        public string Serialize()
        {
            var res = new StringBuilder();
            for (var i = 0; i < this.Rows; i++)
            {
                for (var j = 0; j < this.Columns; j++)
                {
                    var str = this._array[i, j] >= 0 ? $" {this._array[i, j]:0.000}" : $"{this._array[i, j]:0.000}";
                    res.Append($"{str} ");
                }
                res.AppendLine();
            }
            return res.ToString();
        }
        #endregion
    }
}