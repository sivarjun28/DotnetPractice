using System;
using System.Collections.Generic;
using System.Linq;

public static class LinqExtensions
{
    // 1. Batch: Group elements into batches of a specified size
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (batchSize <= 0) throw new ArgumentOutOfRangeException(nameof(batchSize));

        List<T> currentBatch = new List<T>();
        foreach (var item in source)
        {
            currentBatch.Add(item);
            if (currentBatch.Count == batchSize)
            {
                yield return currentBatch;
                currentBatch = new List<T>();
            }
        }

        // Yield the remaining elements as the last batch if it has any
        if (currentBatch.Count > 0)
        {
            yield return currentBatch;
        }
    }

    // 2. Window: Group elements into overlapping windows of a specified size
    public static IEnumerable<IEnumerable<T>> Window<T>(this IEnumerable<T> source, int windowSize)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (windowSize <= 0) throw new ArgumentOutOfRangeException(nameof(windowSize));

        var list = source.ToList(); // Convert to list for easier access to overlapping windows
        for (int i = 0; i <= list.Count - windowSize; i++)
        {
            yield return list.Skip(i).Take(windowSize);
        }
    }

    // 3. TakeUntil: Take elements until a condition is met
    public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        foreach (var item in source)
        {
            yield return item;
            if (predicate(item))
                yield break;
        }
    }

    // 4. DistinctBy: Return distinct elements based on a specified key selector
    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (var item in source)
        {
            var key = keySelector(item);
            if (seenKeys.Add(key)) // Add returns false if the key already exists
            {
                yield return item;
            }
        }
    }

    class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class Program
    {
        static void Main()
        {
            // Test 1: Batch method
            var numbers = Enumerable.Range(1, 10);
            var batches = numbers.Batch(3);
            Console.WriteLine("Batch:");
            foreach (var batch in batches)
            {
                Console.WriteLine(string.Join(", ", batch));
            }

            // Test 2: Window method
            var windowed = numbers.Window(3);
            Console.WriteLine("\nWindow:");
            foreach (var window in windowed)
            {
                Console.WriteLine(string.Join(", ", window));
            }

            // Test 3: TakeUntil method
            var takenUntil = numbers.TakeUntil(n => n == 5);
            Console.WriteLine("\nTakeUntil:");
            foreach (var item in takenUntil)
            {
                Console.WriteLine(item);
            }

            // Test 4: DistinctBy method
            var people = new List<Person>
        {
            new Person { Id = 1, Name = "John" },
            new Person { Id = 2, Name = "Jane" },
            new Person { Id = 3, Name = "John" },
            new Person { Id = 4, Name = "Mike" }
        };
            var distinctPeople = people.DistinctBy(p => p.Name);
            Console.WriteLine("\nDistinctBy:");
            foreach (var person in distinctPeople)
            {
                Console.WriteLine($"{person.Id}: {person.Name}");
            }
        }
    }


}