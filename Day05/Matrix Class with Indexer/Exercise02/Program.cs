using System;

namespace Exercise02
{
    public class Matrix
    {
        private double[,] data;
        
        public int Rows => data.GetLength(0);
        public int Cols => data.GetLength(1);
        
        // Constructor from dimensions
        public Matrix(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Dimensions must be positive");
            data = new double[rows, cols];
        }
        
        // Constructor from 2D array
        public Matrix(double[,] array)
        {
            data = (double[,])array.Clone();
        }
        
        // Indexer
        public double this[int row, int col]
        {
            get
            {
                if (row < 0 || row >= Rows || col < 0 || col >= Cols)
                    throw new IndexOutOfRangeException("Index out of bounds");
                return data[row, col];
            }
            set
            {
                if (row < 0 || row >= Rows || col < 0 || col >= Cols)
                    throw new IndexOutOfRangeException("Index out of bounds");
                data[row, col] = value;
            }
        }
        
        // Fill method to set all values
        public void Fill(double value)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    data[i, j] = value;
                }
            }
        }
        
        // Add method: Adds another matrix to the current one
        public Matrix Add(Matrix other)
        {
            if (this.Rows != other.Rows || this.Cols != other.Cols)
                throw new InvalidOperationException("Matrix dimensions must match for addition.");

            Matrix result = new Matrix(this.Rows, this.Cols);
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Cols; j++)
                {
                    result[i, j] = this[i, j] + other[i, j];
                }
            }
            return result;
        }
        
        // Multiply method: Multiplies another matrix with the current one
        public Matrix Multiply(Matrix other)
        {
            if (this.Cols != other.Rows)
                throw new InvalidOperationException("Matrix dimensions must match for multiplication.");

            Matrix result = new Matrix(this.Rows, other.Cols);
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < other.Cols; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < this.Cols; k++)
                    {
                        result[i, j] += this[i, k] * other[k, j];
                    }
                }
            }
            return result;
        }
        
        // Transpose method: Returns the transpose of the matrix
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
        
        // Display method: Prints the matrix with formatting
        public void Display()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Console.Write($"{data[i, j],8:F2}");
                }
                Console.WriteLine();
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Matrix m1 = new Matrix(new double[,] {
                { 1, 2, 3 },
                { 4, 5, 6 }
            });
            
            Matrix m2 = new Matrix(new double[,] {
                { 7, 8, 9 },
                { 10, 11, 12 }
            });

            Console.WriteLine("Matrix 1:");
            m1.Display();
            
            Console.WriteLine("\nMatrix 2:");
            m2.Display();
            
            // Using indexer
            Console.WriteLine($"\nElement at [0,0] in Matrix 1: {m1[0, 0]}");
            m1[0, 0] = 10;
            
            Console.WriteLine("\nAfter modification of Matrix 1:");
            m1.Display();
            
            // Adding matrices
            try
            {
                Console.WriteLine("\nMatrix 1 + Matrix 2:");
                Matrix sum = m1.Add(m2);
                sum.Display();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            // Transposing a matrix
            Console.WriteLine("\nTranspose of Matrix 1:");
            Matrix transpose = m1.Transpose();
            transpose.Display();
            
            // Multiplying matrices
            try
            {
                Console.WriteLine("\nMatrix 1 * Matrix 2:");
                Matrix product = m1.Multiply(m2);
                product.Display();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}