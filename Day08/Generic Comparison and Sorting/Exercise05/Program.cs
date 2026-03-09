using System;
using System.Collections;
namespace Name
{
    /*
Implement:
1. Custom IComparer<T> implementations
2. Generic sorting algorithms
3. Multi-key sorting

Create comparers for:
- Product by price (ascending/descending)
- Person by age then name
- String by length
*/

    public class ProductPriceComparer : IComparer<Product>
    {
        private readonly bool ascending = true;
        public ProductPriceComparer(bool ascending = true)
        {
            this.ascending = ascending;
        }

        public int Compare(Product? x, Product? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int result = x.Price.CompareTo(y.Price);
            return ascending ? result : -result;
        }
    }

    public class PersonComparer : IComparer<Person>
    {
        public int Compare(Person? x, Person? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int ageComparsion = x.Age.CompareTo(y.Age);
            if (ageComparsion != 0)
            {
                return ageComparsion;
            }

            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);

        }
    }

    public class StringLengthComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            return x.Length.CompareTo(y.Length);

        }
    }

    public class Sorter<T>
    {
        public static void BubbleSort(List<T> list, IComparer<T> comparer)
        {
            int n = list.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (comparer.Compare(list[j], list[j + 1]) > 0)
                    {
                        // Swap the elements
                        T temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        public static void QuickSort(List<T> list, IComparer<T> comparer)
        {
            QuickSortHelper(list, 0, list.Count - 1, comparer);
        }

        private static void QuickSortHelper(List<T> list, int low, int high, IComparer<T> comparer)
        {
            if (low < high)
            {
                int pi = Partition(list, low, high, comparer);
                QuickSortHelper(list, low, pi - 1, comparer);
                QuickSortHelper(list, pi + 1, high, comparer);
            }
        }
        private static int Partition(List<T> list, int low, int high, IComparer<T> comparer)
        {
            T pivot = list[high];
            int i = (low - 1);  // Index of smaller element
            for (int j = low; j < high; j++)
            {
                if (comparer.Compare(list[j], pivot) <= 0)
                {
                    i++;
                    T temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
            T temp1 = list[i + 1];
            list[i + 1] = list[high];
            list[high] = temp1;
            return i + 1;
        }
    }

    public class Product
    {
        public string ? Name { get; set; }
        public decimal Price { get; set; }
    }

    public class Person
    {
        public string ? Name { get; set; }
        public int Age { get; set; }
    }

    public class Program
{
    public static void Main()
    {
        // Test Products
        List<Product> products = new()
        {
            new Product { Name = "Laptop", Price = 999.99m },
            new Product { Name = "Mouse", Price = 25.50m },
            new Product { Name = "Keyboard", Price = 75.00m }
        };

        // Sort by price ascending
        products.Sort(new ProductPriceComparer(true));
        Console.WriteLine("Sorted by price ascending:");
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Name}: {product.Price}");
        }

        // Sort by price descending
        products.Sort(new ProductPriceComparer(false));
        Console.WriteLine("\nSorted by price descending:");
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Name}: {product.Price}");
        }

        // Test People
        List<Person> people = new()
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 25 },
            new Person { Name = "Charlie", Age = 25 }
        };

        // Sort by age then name
        people.Sort(new PersonComparer());
        Console.WriteLine("\nSorted by age then name:");
        foreach (var person in people)
        {
            Console.WriteLine($"{person.Name}, Age: {person.Age}");
        }

        // Test Strings
        List<string> names = new() { "Alice", "Bob", "Charlie", "Daniel" };

        // Sort by string length
        names.Sort(new StringLengthComparer());
        Console.WriteLine("\nSorted by string length:");
        foreach (var name in names)
        {
            Console.WriteLine(name);
        }

        // Test Sorting Algorithms: BubbleSort and QuickSort
        List<int> numbers = new() { 5, 2, 9, 1, 5, 6 };

        // BubbleSort
        Sorter<int>.BubbleSort(numbers, Comparer<int>.Default);
        Console.WriteLine("\nSorted using BubbleSort:");
        foreach (var number in numbers)
        {
            Console.WriteLine(number);
        }

        // QuickSort
        List<int> numbers2 = new() { 5, 2, 9, 1, 5, 6 };
        Sorter<int>.QuickSort(numbers2, Comparer<int>.Default);
        Console.WriteLine("\nSorted using QuickSort:");
        foreach (var number in numbers2)
        {
            Console.WriteLine(number);
        }
    }
}
}