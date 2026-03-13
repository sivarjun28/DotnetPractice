using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using System.Linq.Expressions;

namespace LambdaAndDelegates
{
    internal class Program
    {
        static void Main(string[] args)
        {



            // Create an instance of Delegates
            Delegates delegates = new Delegates();

            // Use the delegate
            Delegates.MathOperation operation = delegates.Add;
            int result = operation(5, 3);  // 8
            Console.WriteLine($"Add result: {result}");

            operation = delegates.Multiply;
            result = operation(5, 3);  // 15
            Console.WriteLine($"Multiply result: {result}");

            // Use the Calculate method and pass the delegate
            delegates.Calculate(5, 3, delegates.Add);  // Result: 8
            delegates.Calculate(5, 3, delegates.Multiply);  // Result: 15

            // Built-in Delegates: Func, Action, Predicate
            Func<int, int, int> add = (a, b) => a + b;
            Func<string, int> getLength = s => s.Length;
            Func<int, bool> isEven = n => n % 2 == 0;

            int sum = add(7, 7);
            int length = getLength("Arjun");
            bool isEven1 = isEven(5678);

            // Action examples
            Action<string> print = message => Console.WriteLine(message);
            Action<int, int> addition = (a, b) => Console.WriteLine(a + b);

            print("Arjun");
            addition(6, 7);

            // Predicate example
            Predicate<int> isPositive = num => num > 0;
            bool result1 = isPositive(5);
            Console.WriteLine(result1);

            // BASIC SYNTAX: (parameters) => expression

            // No parameters
            Func<int> getRandom = () => new Random().Next();

            // One parameter (parentheses optional)
            Func<int, int> square = x => x * x;

            // Multiple parameters
            Func<int, int, int> add1 = (a, b) => a + b;

            // Factorial using lambda
            Func<int, int> factorial = n =>
            {
                int resultFactorial = 1;
                for (int i = 1; i <= n; i++)
                {
                    resultFactorial *= i;
                }
                return resultFactorial;
            };

            Console.WriteLine(factorial(5));  // Output: 120

            Action<string> ignoreParam = _ => Console.WriteLine("Ignored");

            // Filter and Select examples
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
            List<int> evens = numbers.Where(n => n % 2 == 0).ToList();  // Filtering even numbers
            List<int> doubled = numbers.Select(n => n * 2).ToList();    // Doubling each number

            // Display filtered and doubled lists
            Console.WriteLine("Even numbers: " + string.Join(", ", evens));
            Console.WriteLine("Doubled numbers: " + string.Join(", ", doubled));

            // For static methods
            double[] numberArray = { 1.5, 2.3, 3.7 };
            int[] roundedNumbers = numberArray.Select(n => (int)Math.Round(n)).ToArray();  // Type cast after rounding

            // Display rounded numbers
            Console.WriteLine("Rounded numbers: " + string.Join(", ", roundedNumbers));

            // Using Method Group (Method Reference)
            Action<string> print1 = Console.WriteLine;  // Method group
            print1("Hello using method group");


            //Closures & Variable Capture 
            int multiplier = 5;
            Func<int, int> multiplyBy = x => x * multiplier;
            System.Console.WriteLine(multiplyBy(3));
            multiplier = 10;
            System.Console.WriteLine(multiplyBy(4));

            // Closure in loops (GOTCHA!)

            List<Func<int>> functions = new List<Func<int>>();

            for (int i = 0; i < 3; i++)
            {
                functions.Add(() => i + 2);
            }

            foreach (var fun in functions)
            {
                System.Console.WriteLine(fun());
            }

            // FIX: Capture loop variable locally

            for (int i = 0; i < 3; i++)
            {
                int captured = i;
                functions.Add(() => captured);
                functions.Add(() => captured + 3);
            }

            foreach (var func in functions)
            {
                System.Console.WriteLine(func());
            }

            FactoryPatterns patterns = new FactoryPatterns();
            var doubl = patterns.CreateMultiplier(2);
            var triple = patterns.CreateMultiplier(3);
            System.Console.WriteLine(doubl(5));
            System.Console.WriteLine(triple(5));

            var counter = patterns.CreateCounter();
            System.Console.WriteLine(counter());
            System.Console.WriteLine(counter());
            System.Console.WriteLine(counter());

            //Expression Trees
            Func<int, bool> isEven3 = n => n % 2 == 0;
            bool res = isEven3(4);
            System.Console.WriteLine(res);


            // EXPRESSION<FUNC<T>> - Expression tree (data structure representing code)
            Expression<Func<int, bool>> isEvenExpr = n => n % 2 == 0;
            // Can analyze, modify, or translate to other languages (e.g., SQL)

            // Compile expression to get executable function
            Func<int, bool> compiled = isEvenExpr.Compile();
            bool res2 = compiled(4);
            System.Console.WriteLine(res2);


            // Why use expressions?
            // 1. LINQ to SQL/EF Core translates expressions to SQL
            // 2. Can inspect/modify code at runtime
            // 3. Build dynamic queries
            System.Console.WriteLine(isEvenExpr.Body);
            Console.WriteLine(isEvenExpr.Parameters[0].Name);

            //LINQ and Expressions
            // LINQ to Objects uses Func<T> (executes in memory)
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
            IEnumerable<int> evens = numbers.Where(n => n % 2 == 0);  // Func<int, bool>

            // LINQ to Entities (EF Core) uses Expression<Func<T>> (translates to SQL)
            // DbSet<Product> products = context.Products;
            // IQueryable<Product> expensive = products.Where(p => p.Price > 100);
            // SQL: SELECT * FROM Products WHERE Price > 100

            // Expression allows EF to translate lambda to SQL

            

        }



        public List<T> Filter<T>(List<T> items, Func<T, bool> predicate)
        {
            List<T> result = new List<T>();
            foreach (T item in items)
            {
                if (predicate(item))
                    result.Add(item);
            }
            return result;
        }




    }

    public class Delegates
    {
        // Define a delegate type (function signature)
        public delegate int MathOperation(int a, int b);

        // Methods that match the signature
        public int Add(int a, int b) => a + b;
        public int Multiply(int a, int b) => a * b;

        // Pass delegate as parameter (callback pattern)
        public void Calculate(int x, int y, MathOperation op)
        {
            int result = op(x, y);
            Console.WriteLine($"Result: {result}");
        }
    }

    public class FactoryPatterns
    {
        public Func<int, int> CreateMultiplier(int factor)
        {
            return x => x * factor;
        }

        public Func<int> CreateCounter()
        {
            int count = 0;
            return () => ++count;
        }


    }
}