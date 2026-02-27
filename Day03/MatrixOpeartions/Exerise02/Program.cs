using System;

namespace Exercise02
{
    class Matrix
    {
        private int[,] data;
        
        public int Rows => data.GetLength(0);  // Number of rows in the matrix
        public int Cols => data.GetLength(1);  // Number of columns in the matrix
        
        public Matrix(int rows, int cols)
        {
            data = new int[rows, cols];
        }

        public Matrix(int[,] array)
        {
            data = array;
        }

        public int this[int row, int col]
        {
            get => data[row, col];
            set => data[row, col] = value;
        }

        public Matrix Add(Matrix other)
        {
            if (Rows != other.Rows || Cols != other.Cols)
                throw new ArgumentException("Matrices must have the same dimensions");
            
            Matrix result = new Matrix(Rows, Cols);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    result[i, j] = this[i, j] + other[i, j];
                }
            }
            return result;
        }

        public Matrix Multiply(Matrix other)
        {
            if (Cols != other.Rows)
                throw new ArgumentException("Matrix multiplication is not possible. The number of columns of the first matrix must equal the number of rows of the second matrix.");
            
            Matrix result = new Matrix(Rows, other.Cols);
            for (int i = 0; i < Rows; i++)  // Iterate over rows of the first matrix
            {
                for (int j = 0; j < other.Cols; j++)  // Iterate over columns of the second matrix
                {
                    int sum = 0;
                    for (int k = 0; k < Cols; k++)  // Iterate over the shared dimension
                    {
                        sum += this[i, k] * other[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }

        public Matrix Transpose()
        {
            Matrix result = new Matrix(Cols, Rows);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    result[j, i] = this[i, j];
                }
            }
            return result;
        }

        public void Display()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Console.Write($"{data[i, j],4}");
                }
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Matrices for Addition (2x3)
            Matrix m1Add = new Matrix(new int[,] {
                { 1, 2, 3 },
                { 4, 5, 6 }
            });

            Matrix m2Add = new Matrix(new int[,] {
                { 7, 8, 9 },
                { 10, 11, 12 }
            });

            Console.WriteLine("Matrix 1 for Addition:");
            m1Add.Display();

            Console.WriteLine("\nMatrix 2 for Addition:");
            m2Add.Display();

            Console.WriteLine("\nSum:");
            Matrix sum = m1Add.Add(m2Add);
            sum.Display();


            // Matrices for Multiplication (2x3) * (3x2)
            Matrix m1Mul = new Matrix(new int[,] {
                { 1, 2, 3 },
                { 4, 5, 6 }
            });

            Matrix m2Mul = new Matrix(new int[,] {
                { 7, 8 },
                { 9, 10 },
                { 11, 12 }
            });

            Console.WriteLine("\nMatrix 1 for Multiplication:");
            m1Mul.Display();

            Console.WriteLine("\nMatrix 2 for Multiplication:");
            m2Mul.Display();

            Console.WriteLine("\nProduct of Matrix 1 and Matrix 2:");
            Matrix product = m1Mul.Multiply(m2Mul);
            product.Display();


            // Matrix for Transpose (2x3)
            Matrix m1Transpose = new Matrix(new int[,] {
                { 1, 2, 3 },
                { 4, 5, 6 }
            });

            Console.WriteLine("\nMatrix 1 for Transpose:");
            m1Transpose.Display();

            Console.WriteLine("\nTranspose of Matrix 1:");
            Matrix transpose = m1Transpose.Transpose();
            transpose.Display();
        }
    }
}