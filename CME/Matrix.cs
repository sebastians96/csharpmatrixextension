using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatrixextension
{
    class Matrix<T> where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        int RowNumber { get; }
        int ColumnNumber { get; }
        T[,] Data { get; set; }
        public Matrix(Matrix<T> m)
        {
            RowNumber = m.Data.GetLength(0);
            ColumnNumber = m.Data.GetLength(1);
            Data = new T[RowNumber, ColumnNumber];
            for (int i = 0; i < RowNumber; i++)
            {
                for (int j = 0; j < ColumnNumber; j++)
                {
                    Data[i, j] = m.Data[i, j];
                }
            }
        }
        public Matrix(int r, int c)
        {
            RowNumber = r;
            ColumnNumber = c;
            Data = new T[r, c];
        }
        public Matrix(T[] input, int r, int c)
        {
            RowNumber = r;
            ColumnNumber = c;
            Data = new T[r, c];
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    Data[i, j] = input[i * c + j];
                }
            }
        }

        public void Write()
        {
            for (int i = 0; i < RowNumber; i++)
            {
                for (int j = 0; j < ColumnNumber; j++)
                {
                    double[,] d = new double[1, 1];
                    float[,] f = new float[1, 1];
                    if (Data.GetType() == d.GetType() || Data.GetType() == f.GetType()) Console.Write("{0:N2} ", (Data[i, j]));
                    else Console.Write("{0} ", (Data[i, j]));
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Creates empty matrix filled with zeros.
        /// </summary>
        /// <param name="r">Row number</param>
        /// <param name="c">Column number</param>
        /// <returns>Matrix</returns>
        public static Matrix<T> Zeros(int r, int c)
        {
            int length = r * c;
            T[] array = new T[length];
            Array.Clear(array, 0, length);
            return new Matrix<T>(array, r, c);
        }

        public static Matrix<T> Zeros(int r)
        {
            int length = r * r;
            T[] array = new T[length];
            Array.Clear(array, 0, length);
            return new Matrix<T>(array, r, r);
        }

        public static Matrix<T> Identity(int r)
        {
            Matrix<T> array = Matrix<T>.Zeros(r, r);
            for (int i = 1; i <= r; i++)
            {
                array.PickValue(i, i) = (T)Convert.ChangeType(1, typeof(T));
            }
            return array;
        }

        public ref T PickValue(int r, int c)
        {
            return ref this.Data[r - 1, c - 1];
        }

        public T[] Row(int r)
        {
            T[] row = new T[Data.GetLength(1)];
            for (int i = 0; i < Data.GetLength(0); i++)
            {
                row[i] = (T)Data.GetValue(r - 1, i);
            }
            return row;
        }

        public T[] Column(int c)
        {
            T[] row = new T[Data.GetLength(0)];
            for (int i = 0; i < Data.GetLength(1); i++)
            {
                row[i] = (T)Data.GetValue(i, c - 1);
            }
            return row;
        }

        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
        {
            int r, c;
            if (a.Data.GetLength(0) > b.Data.GetLength(0)) r = a.Data.GetLength(0);
            else r = b.Data.GetLength(0);
            if (a.Data.GetLength(1) > b.Data.GetLength(1)) c = a.Data.GetLength(1);
            else c = b.Data.GetLength(1);
            Matrix<T> tmpa = new Matrix<T>(r, c);
            for (int i = 0; i < a.Data.GetLength(0); i++)
            {
                for (int j = 0; j < a.Data.GetLength(1); j++)
                {
                    tmpa.Data[i, j] = a.Data[i, j];
                }
            }
            Matrix<T> tmpb = new Matrix<T>(r, c);
            for (int i = 0; i < b.Data.GetLength(0); i++)
            {
                for (int j = 0; j < b.Data.GetLength(1); j++)
                {
                    tmpb.Data[i, j] = b.Data[i, j];
                }
            }
            double[,] d = new double[1, 1];
            float[,] f = new float[1, 1];
            if (a.Data.GetType() == d.GetType() || b.Data.GetType() == d.GetType())
            {
                Matrix<double> result = new Matrix<double>(r, c);
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        result.Data[i, j] = (double)(Object)tmpa.PickValue(i + 1, j + 1) + (double)(Object)tmpb.PickValue(i + 1, j + 1);
                    }
                }
                return (Matrix<T>)Convert.ChangeType(result, typeof(Matrix<double>));
            }
            else if (a.Data.GetType() == f.GetType() || b.Data.GetType() == f.GetType())
            {
                Matrix<float> result = new Matrix<float>(r, c);
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        result.Data[i, j] = (float)(Object)tmpa.PickValue(i + 1, j + 1) + (float)(Object)tmpb.PickValue(i + 1, j + 1);
                    }
                }
                return (Matrix<T>)Convert.ChangeType(result, typeof(Matrix<float>));
            }
            else
            {
                Matrix<int> result = new Matrix<int>(r, c);
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        result.Data[i, j] = (int)(Object)tmpa.PickValue(i + 1, j + 1) + (int)(Object)tmpb.PickValue(i + 1, j + 1);
                    }
                }
                return (Matrix<T>)Convert.ChangeType(result, typeof(Matrix<int>));
            }
        }

        public static Matrix<T> operator -(Matrix<T> a, Matrix<T> b)
        {
            int r, c;
            if (a.Data.GetLength(0) > b.Data.GetLength(0)) r = a.Data.GetLength(0);
            else r = b.Data.GetLength(0);
            if (a.Data.GetLength(1) > b.Data.GetLength(1)) c = a.Data.GetLength(1);
            else c = b.Data.GetLength(1);
            Matrix<T> tmpa = new Matrix<T>(r, c);
            for (int i = 0; i < a.Data.GetLength(0); i++)
            {
                for (int j = 0; j < a.Data.GetLength(1); j++)
                {
                    tmpa.Data[i, j] = a.Data[i, j];
                }
            }
            Matrix<T> tmpb = new Matrix<T>(r, c);
            for (int i = 0; i < b.Data.GetLength(0); i++)
            {
                for (int j = 0; j < b.Data.GetLength(1); j++)
                {
                    tmpb.Data[i, j] = b.Data[i, j];
                }
            }
            double[,] d = new double[1, 1];
            float[,] f = new float[1, 1];
            if (a.Data.GetType() == d.GetType() || b.Data.GetType() == d.GetType())
            {
                Matrix<double> result = new Matrix<double>(r, c);
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        result.Data[i, j] = (double)(Object)tmpa.PickValue(i + 1, j + 1) - (double)(Object)tmpb.PickValue(i + 1, j + 1);
                    }
                }
                return (Matrix<T>)Convert.ChangeType(result, typeof(Matrix<double>));
            }
            else if (a.Data.GetType() == f.GetType() || b.Data.GetType() == f.GetType())
            {
                Matrix<float> result = new Matrix<float>(r, c);
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        result.Data[i, j] = (float)(Object)tmpa.PickValue(i + 1, j + 1) - (float)(Object)tmpb.PickValue(i + 1, j + 1);
                    }
                }
                return (Matrix<T>)Convert.ChangeType(result, typeof(Matrix<float>));
            }
            else
            {
                Matrix<int> result = new Matrix<int>(r, c);
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        result.Data[i, j] = (int)(Object)tmpa.PickValue(i + 1, j + 1) - (int)(Object)tmpb.PickValue(i + 1, j + 1);
                    }
                }
                return (Matrix<T>)Convert.ChangeType(result, typeof(Matrix<int>));
            }
        }

        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
        {
            if (a.Data.GetLength(1) == b.Data.GetLength(0))
            {
                int ar = a.Data.GetLength(0);
                int ac = a.Data.GetLength(1);
                int br = b.Data.GetLength(0);
                int bc = b.Data.GetLength(1);
                double[,] d = new double[1, 1];
                float[,] f = new float[1, 1];
                if (a.Data.GetType() == d.GetType() || b.Data.GetType() == d.GetType())
                {
                    Matrix<double> result = new Matrix<double>(ar, bc);
                    for (int i = 0; i < ar; i++)
                    {
                        for (int j = 0; j < bc; j++)
                        {
                            double sum = 0.0;
                            for (int k = 0; k < ac; k++)
                            {
                                sum += (double)(Object)a.PickValue(i + 1, k + 1) * (double)(Object)b.PickValue(k + 1, j + 1);
                            }
                            result.Data[i, j] = sum;
                        }
                    }
                    return (Matrix<T>)Convert.ChangeType(result, typeof(Matrix<double>));
                }
                else if (a.Data.GetType() == f.GetType() || b.Data.GetType() == f.GetType())
                {
                    Matrix<float> result = new Matrix<float>(ar, bc);
                    for (int i = 0; i < ar; i++)
                    {
                        for (int j = 0; j < bc; j++)
                        {
                            float sum = 0.0f;
                            for (int k = 0; k < ac; k++)
                            {
                                sum += (float)(Object)a.PickValue(i + 1, k + 1) * (float)(Object)b.PickValue(k + 1, j + 1);
                            }
                            result.Data[i, j] = sum;
                        }
                    }
                    return (Matrix<T>)Convert.ChangeType(result, typeof(Matrix<float>));
                }
                else
                {
                    Matrix<int> result = new Matrix<int>(ar, bc);
                    for (int i = 0; i < ar; i++)
                    {
                        for (int j = 0; j < bc; j++)
                        {
                            int sum = 0;
                            for (int k = 0; k < ac; k++)
                            {
                                sum += (int)(Object)a.PickValue(i + 1, k + 1) * (int)(Object)b.PickValue(k + 1, j + 1);
                            }
                            result.Data[i, j] = sum;
                        }
                    }
                    return (Matrix<T>)Convert.ChangeType(result, typeof(Matrix<int>));
                }

            }
            else
            {
                throw new Exception("Incorrect sizes of matrices!");
            }
        }

        public Matrix<T> Power(int a)
        {
            if (Data.GetLength(0) == Data.GetLength(1))
            {
                double[,] d = new double[1, 1];
                float[,] f = new float[1, 1];
                if (Data.GetType() == d.GetType())
                {
                    Matrix<double> result;
                    if (a == 0)
                    {
                        result = Matrix<double>.Identity(Data.GetLength(0));
                    }
                    else
                    {
                        result = new Matrix<double>((Matrix<double>)(Object)this);
                        for (int i = 1; i < a; i++)
                        {
                            result *= (Matrix<double>)(Object)this;
                        }
                    }
                    return (Matrix<T>)Convert.ChangeType(result, typeof(Matrix<double>));
                }
                else if (Data.GetType() == f.GetType())
                {
                    Matrix<float> result;
                    if (a == 0)
                    {
                        result = Matrix<float>.Identity(Data.GetLength(0));
                    }
                    else
                    {
                        result = new Matrix<float>((Matrix<float>)(Object)this);
                        for (int i = 1; i < a; i++)
                        {
                            result *= (Matrix<float>)(Object)this;
                        }
                    }
                    return (Matrix<T>)Convert.ChangeType(result, typeof(Matrix<float>));
                }
                else
                {
                    Matrix<int> result;
                    if (a == 0)
                    {
                        result = Matrix<int>.Identity(Data.GetLength(0));
                    }
                    else
                    {
                        result = new Matrix<int>((Matrix<int>)(Object)this);
                        for (int i = 1; i < a; i++)
                        {
                            result *= (Matrix<int>)(Object)this;
                        }
                    }
                    return (Matrix<T>)Convert.ChangeType(result, typeof(Matrix<int>));
                }
            }
            else
            {
                throw new Exception("It's not a square matrix!");
            }
        }
        public static bool operator ==(Matrix<T> a, Matrix<T> b)
        {
            if (a.RowNumber == b.RowNumber)
            {
                if (a.ColumnNumber == b.ColumnNumber)
                {
                    for (int i = 0; i < a.RowNumber; i++)
                    {
                        for (int j = 0; j < a.ColumnNumber; j++)
                        {
                            if ((double)(Object)a.Data[i, j] != (double)(Object)b.Data[i, j])
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
            return true;

        }
        public static bool operator !=(Matrix<T> a, Matrix<T> b)
        {
            if (a.RowNumber == b.RowNumber)
            {
                if (a.ColumnNumber == b.ColumnNumber)
                {
                    for (int i = 0; i < a.RowNumber; i++)
                    {
                        for (int j = 0; j < a.ColumnNumber; j++)
                        {
                            if ((double)(Object)a.Data[i, j] != (double)(Object)b.Data[i, j])
                                return true;
                        }
                    }
                }
                else
                    return true;
            }
            else
            {
                return true;
            }
            return false;
        }
        public double Determinant()
        {

            if (RowNumber == 1)
                return (double)(Object)Data[0, 0];
            else if (RowNumber == 2)
            {
                return (double)(Object)Data[0, 0] * (double)(Object)Data[1, 1] - (double)(Object)Data[0, 1] * (double)(Object)Data[1, 0];
            }
            else if (RowNumber > 2)
            {
                double value = 0;
                for (int j = 0; j < RowNumber; j++)
                {
                    Matrix<T> Tmp = CreateSmallerMatrix(0, j);
                    value = value + (double)(Object)Data[0, j] * Math.Pow(-1, j) * Tmp.Determinant();
                }
                return value;
            }

            return 0;
        }

        public Matrix<T> CreateSmallerMatrix(int i, int j)
        {
            int Row = RowNumber - 1;
            Matrix<T> output = new Matrix<T>(Row, Row);
            int x = 0, y = 0;
            for (int m = 0; m < RowNumber; m++, x++)
            {
                if (m != i)
                {
                    y = 0;
                    for (int n = 0; n < RowNumber; n++)
                    {
                        if (n != j)
                        {
                            output.Data[x, y] = Data[m, n];
                            y++;
                        }
                    }
                }
                else
                    x--;
            }
            return output;
        }

        public Matrix<T> Transpose()
        {
            Matrix<T> result = new Matrix<T>(ColumnNumber, RowNumber);
            for (int i = 0; i < ColumnNumber; i++)
            {
                for (int j = 0; j < RowNumber; j++)
                {
                    result.Data[i, j] = Data[j, i];
                }
            }
            return result;
        }

        public Matrix<T> Complements()
        {
            Matrix<double> tmp2 = new Matrix<double>(RowNumber, ColumnNumber);
            Matrix<T> tmp = new Matrix<T>(RowNumber - 1, ColumnNumber - 1);
            for (int i = 0; i < RowNumber; i++)
            {
                for (int j = 0; j < ColumnNumber; j++)
                {
                    tmp = CreateSmallerMatrix(i, j);
                    tmp2.Data[i, j] = Math.Pow(-1, i + j) * tmp.Determinant();
                }
            }
            //for (int i = 0; i < RowNumber; i++)
            //{
            //    for (int j = 0; j < ColumnNumber; j++)
            //    {
            //        Console.WriteLine(tmp2[i, j]);
            //    }
            //}
            return (Matrix<T>)Convert.ChangeType(tmp2, typeof(Matrix<double>));
        }

        public Matrix<T> Inverse()
        {
            double det = Determinant();
            Matrix<double> tmp2 = new Matrix<double>(RowNumber, ColumnNumber);
            if (det == 0)
            {
                return this;
            }
            else
            {
                Matrix<T> tmp = new Matrix<T>(RowNumber, ColumnNumber);
                tmp = this.Transpose();
                // Write();
                tmp = tmp.Complements();
                // Write();
                for (int i = 0; i < RowNumber; i++)
                {
                    for (int j = 0; j < ColumnNumber; j++)
                    {
                        tmp2.Data[i, j] = (1 / det) * (double)(Object)tmp.Data[i, j];
                    }
                }
            }
            return (Matrix<T>)Convert.ChangeType(tmp2, typeof(Matrix<double>));
        }
    }
}
