using System;
namespace Exercise03
{
 /*
Implement these generic utilities:
1. FindMax<T>(list) where T : IComparable<T>
2. FindMin<T>(list) where T : IComparable<T>
3. Swap<T>(ref a, ref b)
4. Clone<T>(obj) where T : ICloneable
5. AreEqual<T>(a, b) where T : IEquatable<T>
6. ConvertAll<TInput, TOutput>(list, converter)  
*/

    public static class GenericUtilities
    {
        // Find maximum value
        public static T FindMax<T>(List<T> list) where T : IComparable<T>
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("List cannot be null or empty");

            T max = list[0];
            foreach (T item in list)
            {
                if (item.CompareTo(max) > 0)
                    max = item;
            }
            return max;
        }

        // Find minimum value
        public static T FindMin<T>(List<T> list) where T : IComparable<T>
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("List cannot be null or empty");

            T min = list[0];
            foreach (T item in list)
            {
                if (item.CompareTo(min) < 0)
                    min = item;
            }
            return min;
        }

        // Swap two values
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        // Check if two values are equal
        public static bool AreEqual<T>(T a, T b) where T : IEquatable<T>
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            return a.Equals(b);
        }

        // Convert list from one type to another
        public static List<TOutput> ConvertAll<TInput, TOutput>(
            List<TInput> input,
            Func<TInput, TOutput> converter)
        {
            List<TOutput> result = new List<TOutput>();
            foreach (TInput item in input)
            {
                result.Add(converter(item));
            }
            return result;
        }

        // Filter list based on a predicate
        public static List<T> Filter<T>(List<T> list, Predicate<T> predicate)
        {
            List<T> filteredList = new List<T>();
            foreach (T item in list)
            {
                if (predicate(item))
                    filteredList.Add(item);
            }
            return filteredList;
        }

        // Partition list into two groups based on a predicate
        public static (List<T> matching, List<T> notMatching) Partition<T>(
            List<T> list,
            Predicate<T> predicate)
        {
            List<T> matching = new List<T>();
            List<T> notMatching = new List<T>();

            foreach (T item in list)
            {
                if (predicate(item))
                    matching.Add(item);
                else
                    notMatching.Add(item);
            }

            return (matching, notMatching);
        }
    }

    // Test Code
    public class Program
    {
        public static void Main()
        {
            // Test FindMax and FindMin
            List<int> numbers = new() { 5, 2, 8, 1, 9 };
            int max = GenericUtilities.FindMax(numbers);  // 9
            int min = GenericUtilities.FindMin(numbers);  // 1
            Console.WriteLine($"Max: {max}, Min: {min}");

            // Test Swap
            int x = 10, y = 20;
            GenericUtilities.Swap(ref x, ref y);  // x=20, y=10
            Console.WriteLine($"x: {x}, y: {y}");

            // Test ConvertAll
            List<string> names = new() { "Alice", "Bob", "Charlie" };
            List<int> lengths = GenericUtilities.ConvertAll(names, n => n.Length);  // [5, 3, 7]
            Console.WriteLine("Lengths: " + string.Join(", ", lengths));

            // Test Filter
            List<int> evenNumbers = GenericUtilities.Filter(numbers, n => n % 2 == 0);  // [2, 8]
            Console.WriteLine("Even Numbers: " + string.Join(", ", evenNumbers));

            // Test Partition
            var (even, odd) = GenericUtilities.Partition(numbers, n => n % 2 == 0);
            Console.WriteLine("Even: " + string.Join(", ", even));
            Console.WriteLine("Odd: " + string.Join(", ", odd));
        }
    }

}
