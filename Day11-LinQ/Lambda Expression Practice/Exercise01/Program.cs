using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Exercise01
{
    /*
Create utility class with methods that accept lambdas:
1. Filter<T>(List<T> items, Predicate<T> predicate)
2. Transform<T, R>(List<T> items, Func<T, R> transformer)
3. ForEach<T>(List<T> items, Action<T> action)
4. FindFirst<T>(List<T> items, Predicate<T> predicate)
5. Aggregate<T, R>(List<T> items, R seed, Func<R, T, R> accumulator)
*/

    public class FunctionUtils
    {
        public static List<T> Filter<T>(List<T> items, Predicate<T> predicate)
        {
            List<T> result = new();

            foreach (T item in result)
            {
                if (predicate(item))
                    result.Add(item);
            }
            return result;
        }

        public static List<R> Trasform<T, R>(List<T> items, Func<T, R> transformer)
        {
            List<R> result = new List<R>();

            foreach (T data in items)
            {
                result.Add(transformer(data));
            }

            return result;
        }

        public static void ForEach<T>(List<T> items, Action<T> action)
        {

            foreach (T item in items)
            {
                action(item);
            }
        }
        public static T? FindFirst<T>(List<T> items, Predicate<T> predicate)
        {
            foreach (T item in items)          // Check each item in the list
            {
                if (predicate(item))           // If it matches the condition
                {
                    return item;               // Return the first matching item
                }
            }
            return default;                    // Return default if none match
        }

        public static R Aggregate<T, R>(List<T> items, R seed, Func<R, T, R> accumulator)
        {
            R result = seed;
            foreach (T item in items)
            {
                result = accumulator(result, item);
            }
            return result;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            List<int> evens = FunctionUtils.Filter(numbers, n => n % 2 == 0);
            System.Console.WriteLine($"Evens: " + string.Join(", ", evens));

            List<int> transform = FunctionUtils.Trasform(numbers, n => n * n);
            System.Console.WriteLine($"squares " + string.Join(", ", transform));

            System.Console.Write($"Numbers: ");
            FunctionUtils.ForEach(numbers, n => System.Console.Write($"{n + " "}"));

            System.Console.WriteLine();

            int? first = FunctionUtils.FindFirst(numbers, n => n > 5);
            System.Console.WriteLine($"First: > 5: {first}");

            int sum = FunctionUtils.Aggregate(numbers, 0, (acc, n) => acc + n);

            System.Console.WriteLine($"Sum: {sum}");

            // Filter words longer than 5 characters
            // Transform to uppercase
            // Find first word starting with 'c'

            List<string> words = new() { "apple", "banana", "cherry", "grapes" };
            List<string> filterWords = FunctionUtils.Filter(words, w => w.Length > 4);

            // Print each filtered word
            foreach (var word in filterWords)
            {
                System.Console.WriteLine($"The words longer than 4 characters: {word}");
            }
            List<string> upperCase = FunctionUtils.Trasform(words, w => w.ToUpper());

            foreach (var upper in upperCase)
            {
                System.Console.WriteLine($"the upper case words are {upper}");
            }
            string charC = FunctionUtils.FindFirst(words, w => w.Contains('c'));
            System.Console.WriteLine(charC);
        }
    }
}
