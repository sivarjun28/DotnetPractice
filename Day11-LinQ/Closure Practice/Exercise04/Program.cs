// See https://aka.ms/new-console-template for more information
/*
Create functions that demonstrate closures:
1. Counter factory - creates independent counters
2. Filter builder - builds composite predicates
3. Caching function - memoization
4. Rate limiter - limits function calls
*/

using System;
using System.Diagnostics.Metrics;
using Microsoft.VisualBasic;
namespace Exercise04
{
    public static class ClosureExamples
    {
        public static Func<int> CreateCounter(int start = 0)
        {
            int count = start;
            return () => ++count;
        }

        // TODO: Multiplier factory
        public static Func<int, int> CreateMultiplier(int factor)
        {
            return x => x * factor;// closure is factor
        }

        // Filter builder
        public static Func<T, bool> And<T>(Func<T, bool> predicate1,
                   Func<T, bool> predicate2)
        {
            return item => predicate1(item) && predicate2(item);
        }

        public static Func<T, bool> Or<T>(Func<T, bool> predicate1, Func<T, bool> predicate2)
        {

            return item => predicate1(item) || predicate2(item);
        }


        // Implement Caching/Memoization
        public static Func<T, R> Memoize<T, R>(Func<T, R> func) where T : notnull
        {
            Dictionary<T, R> cache = new();

            return arg =>
                {
                    if (cache.TryGetValue(arg, out R? result))

                    {
                        System.Console.WriteLine($"Cache hit for {arg}");
                        return result;
                    }

                    System.Console.WriteLine($"computing for {arg}");
                    result = func(arg);
                    cache[arg] = result;
                    return result;
                };
        }

        public static Func<T> RateLimit<T>(Func<T> func, TimeSpan interval)
        {
            DateTime lastCall = DateTime.MinValue;

            T? lastResult = default;

            return() =>
            {
                var now = DateTime.UtcNow;

                if(now - lastCall >= interval)
                {
                    System.Console.WriteLine("Executing function");
                    lastResult = func(); // call original function
                    lastCall = now;  // update last execution time
                }
                else
                {
                    System.Console.WriteLine("Rate limit hit - returning cached element ");
                }

                return lastResult!;
            };
                    
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var counter1 = ClosureExamples.CreateCounter();
            var counter2 = ClosureExamples.CreateCounter(100);

            System.Console.WriteLine("Counter1: ");
            System.Console.WriteLine(counter1());
            System.Console.WriteLine(counter1());
            System.Console.WriteLine(counter1());

            System.Console.WriteLine("Counter2: ");
            System.Console.WriteLine(counter2());
            System.Console.WriteLine(counter2());

            var double1 = ClosureExamples.CreateMultiplier(2);
            var triple = ClosureExamples.CreateMultiplier(7);

             Console.WriteLine($"\nDouble 5: {double1(5)}");
            Console.WriteLine($"Triple 5: {triple(5)}");

            Func<int, bool> isEven = n => n % 2 == 0;
            Func<int, bool> isPositive = n => n > 0;
            Func<int, bool> isLarge = n => n > 10;

            var positiveEvens = ClosureExamples.And(isEven, isPositive);
            var largeOrEven = ClosureExamples.Or(isLarge, isEven);

            System.Console.WriteLine($"\n5 is positive Even: {positiveEvens(5)}");
            System.Console.WriteLine($"12 is Large or Even : {largeOrEven(12)}");


            Func<int, int> expensiveCalculation = n =>
            {
                System.Threading.Thread.Sleep(1000);
                return n * n;
            };
            var memoized = ClosureExamples.Memoize(expensiveCalculation);

            System.Console.WriteLine("\nmemorization test");
            System.Console.WriteLine(memoized(5));
            System.Console.WriteLine(memoized(5));
            System.Console.WriteLine(memoized(6));

            Func<int> getRandom = () =>
            {
                System.Console.WriteLine("Generating random......");
                return new Random().Next(1,100);

            };
            var limited = ClosureExamples.RateLimit(getRandom, TimeSpan.FromSeconds(3));
            System.Console.WriteLine(limited());
            System.Console.WriteLine(limited());
            Thread.Sleep(3000);
            System.Console.WriteLine(limited());
        }
    }
}
