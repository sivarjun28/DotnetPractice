using System;
namespace Exercise06
{

public class SparseMatrix<T>
{
    private readonly Dictionary<(int, int), T> _matrix;
    private readonly int _rows;
    private readonly int _cols;
    
    // Constructor for SparseMatrix
    public SparseMatrix(int rows, int cols)
    {
        _matrix = new Dictionary<(int, int), T>();
        _rows = rows;
        _cols = cols;
    }

    // Indexer to access matrix elements
    public T this[int row, int col]
    {
        get
        {
            if (_matrix.ContainsKey((row, col)))
                return _matrix[(row, col)];
            else
                return default(T); // return default for zero elements
        }
        set
        {
            if (!EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                _matrix[(row, col)] = value;
            }
            else
            {
                _matrix.Remove((row, col)); // Remove zero elements from the sparse matrix
            }
        }
    }

    // Get the entire row as a list
    public List<T> GetRow(int row)
    {
        List<T> result = new List<T>();
        for (int col = 0; col < _cols; col++)
        {
            result.Add(this[row, col]);
        }
        return result;
    }

    // Get the entire column as a list
    public List<T> GetColumn(int col)
    {
        List<T> result = new List<T>();
        for (int row = 0; row < _rows; row++)
        {
            result.Add(this[row, col]);
        }
        return result;
    }

    // Add two sparse matrices
    public SparseMatrix<T> Add(SparseMatrix<T> other)
    {
        if (_rows != other._rows || _cols != other._cols)
            throw new ArgumentException("Matrices must have the same dimensions");

        SparseMatrix<T> result = new SparseMatrix<T>(_rows, _cols);

        // Add this matrix
        foreach (var entry in _matrix)
        {
            result[entry.Key.Item1, entry.Key.Item2] = entry.Value;
        }

        // Add other matrix
        foreach (var entry in other._matrix)
        {
            if (result._matrix.ContainsKey(entry.Key))
            {
                // Assuming T supports addition
                dynamic existingValue = result[entry.Key.Item1, entry.Key.Item2];
                result[entry.Key.Item1, entry.Key.Item2] = existingValue + entry.Value;
            }
            else
            {
                result[entry.Key.Item1, entry.Key.Item2] = entry.Value;
            }
        }

        return result;
    }

    // Multiply two sparse matrices
    public SparseMatrix<T> Multiply(SparseMatrix<T> other)
    {
        if (_cols != other._rows)
            throw new ArgumentException("Matrix dimensions must match for multiplication");

        SparseMatrix<T> result = new SparseMatrix<T>(_rows, other._cols);

        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < other._cols; j++)
            {
                dynamic sum = 0;
                for (int k = 0; k < _cols; k++)
                {
                    dynamic a = this[i, k];
                    dynamic b = other[k, j];
                    sum += a * b;
                }
                if (!EqualityComparer<T>.Default.Equals(sum, default(T)))
                {
                    result[i, j] = sum;
                }
            }
        }

        return result;
    }

    // Display the matrix
    public void Display()
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                Console.Write($"{this[i, j]} ");
            }
            Console.WriteLine();
        }
    }

    // Compare memory usage between sparse and dense matrices
    public static void CompareMemoryUsage(int rows, int cols)
    {
        // Dense Matrix would be stored fully in memory
        long denseMatrixSize = (long)rows * cols * sizeof(int);  // Assuming int type
        SparseMatrix<int> sparseMatrix = new SparseMatrix<int>(rows, cols);
        long sparseMatrixSize = sparseMatrix._matrix.Count * (sizeof(int) + sizeof(int) * 2);  // Key size (2 ints) + value size (int)
        
        Console.WriteLine($"Memory usage comparison:");
        Console.WriteLine($"Dense Matrix: {denseMatrixSize / (1024.0 * 1024.0)} MB");
        Console.WriteLine($"Sparse Matrix: {sparseMatrixSize / (1024.0 * 1024.0)} MB");
    }
}

public class Program
{
    public static void Main()
    {
        SparseMatrix<int> matrix = new SparseMatrix<int>(5, 5);
        
        // Set some values
        matrix[0, 0] = 5;
        matrix[1, 2] = 8;
        matrix[2, 3] = 3;
        matrix[4, 4] = 1;
        
        Console.WriteLine("Sparse Matrix:");
        matrix.Display();
        
        // Get a row
        var row = matrix.GetRow(2);
        Console.WriteLine("Row 2: " + string.Join(", ", row));
        
        // Get a column
        var col = matrix.GetColumn(2);
        Console.WriteLine("Column 2: " + string.Join(", ", col));
        
        // Memory comparison with dense matrix
        SparseMatrix<int>.CompareMemoryUsage(5, 5);
        
        // Matrix addition
        SparseMatrix<int> matrix2 = new SparseMatrix<int>(5, 5);
        matrix2[0, 0] = 10;
        matrix2[1, 1] = 6;
        SparseMatrix<int> sumMatrix = matrix.Add(matrix2);
        Console.WriteLine("Sum of matrices:");
        sumMatrix.Display();
        
        // Matrix multiplication (example)
        SparseMatrix<int> mulMatrix = matrix.Multiply(matrix2);
        Console.WriteLine("Product of matrices:");
        mulMatrix.Display();
    }
}
}
