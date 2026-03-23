using System;
using System.Drawing;
namespace Exercise03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var numbers = Enumerable.Range(1, 20);
            var everyThird = numbers.TakeEveryNth(3);
            System.Console.WriteLine("Take every Nth");
            foreach (var item in everyThird)
            {
                System.Console.WriteLine(item);
            }
            // Expected: [1, 4, 7, 10, 13, 16, 19]

            // Test WhereNotNull
            List<string?> words = new() { "hello", null, "world", null, "test" };
            var nonNull = words.WhereNotNull();
            // Expected: ["hello", "world", "test"]
            System.Console.WriteLine("Where not null");
            foreach (var item in words)
            {
                System.Console.WriteLine(item);
            }

            // Test Partition
            var (evens, odds) = numbers.Partition(n => n % 2 == 0);
            // evens: [2, 4, 6, 8, 10, 12, 14, 16, 18, 20]
            // odds:  [1, 3, 5, 7, 9, 11, 13, 15, 17, 19]
            System.Console.WriteLine("Partition ");
            foreach (var item in evens)
            {
                System.Console.WriteLine(item);
            }
            foreach (var item in odds)
            {
                System.Console.WriteLine(item);
            }
            System.Console.WriteLine("Chunks");
            // Test Chunk
            var chunks = numbers.Chunk(5);
            // Expected: [[1,2,3,4,5], [6,7,8,9,10], [11,12,13,14,15], [16,17,18,19,20]]
            foreach (var item in chunks)
            {
                System.Console.WriteLine(item);
            }
            // Test RandomSample
            var sample = numbers.RandomSample(5);
            // Expected: 5 random numbers from 1-20
            System.Console.WriteLine("Random sample");
            foreach (var item in sample)
            {
                System.Console.WriteLine(item);
            }

            List<double> price = new() {22.3,43,78.9,987.9};
            var maxBy =  price.MaxBy(p => p);
            System.Console.WriteLine(maxBy);
        }
    }

    public static class LinqExtensios
    {
        //Yield every Nth element (1st, (n+1)th, (2n+1)th, etc.)
        public static IEnumerable<T> TakeEveryNth<T>(this IEnumerable<T> source, int n)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (n <= 0)
                throw new ArgumentOutOfRangeException(nameof(n), "N must be greater than 0");
            int index = 0;
            foreach (var item in source)
            {
                if (index % n == 0)
                    yield return item;
                index++;
            }
        }
        // Filter nulls and return non-nullable enumerable
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source) where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (var item in source)
            {
                if (item != null)
                {
                    yield return item;
                }
            }
        }
        //Split into two groups without enumerating twice

        public static (IEnumerable<T> Matching, IEnumerable<T> NonMatching) Partition<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            var matching = new List<T>();
            var nonMatching = new List<T>();
            foreach (var item in source)
            {
                if (predicate(item))
                    matching.Add(item);
                else
                {
                    nonMatching.Add(item);
                }
            }
            return new(matching, nonMatching);
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(
       this IEnumerable<T> source,
       int size)
        {
            //  Split into chunks of specified size
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            var chunck = new List<T>(size);
            foreach (var item in chunck)
            {
                if (chunck.Count == size)
                {
                    yield return chunck;
                    chunck = new List<T>(size);
                }
            }
            if (chunck.Count > 0)
                yield return chunck;
        }

        public static IEnumerable<T> RandomSample<T>(
    this IEnumerable<T> source,
    int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            var reservoir = new List<T>(count);
            var random = new Random();

            int i = 0;

            foreach (var item in source)
            {
                if (i < count)
                {
                    reservoir.Add(item);
                }
                else
                {
                    int j = random.Next(i + 1);
                    if (j < count)
                    {
                        reservoir[j] = item;
                    }
                }
                i++;
            }

            return reservoir;
        }

        public static T? MaxBy<T, TKey>(
    this IEnumerable<T> source,
    Func<T, TKey> selector)
    where TKey : IComparable<TKey>
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            using var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
                return default;

            var maxItem = enumerator.Current;
            var maxKey = selector(maxItem);

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                var currentKey = selector(current);

                if (currentKey.CompareTo(maxKey) > 0)
                {
                    maxKey = currentKey;
                    maxItem = current;
                }
            }

            return maxItem;
        }

    }
}
