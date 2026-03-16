using System;
using System.Security.Cryptography;
namespace masteryLinq
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //without Linq
            List<int> numbers = new() { 1, 2, 3, 4, 5, 6, 7, 8 };
            List<int> eventNums = new();
            foreach (var num in eventNums)
            {
                if (num % 2 == 0)
                {
                    eventNums.Add(num);
                }
            }

            // with Linq
            var evenNumbers = numbers.Where(n => n % 2 == 0);
            //QuerySyntax
            var query1 = from n in numbers
                         where n % 2 == 0
                         select n;
            foreach (var item in query1)
            {
                System.Console.WriteLine(item);
            }

            //Basic Linq queries
            //multiple conditions
            var filtered = numbers.Where(n => n > 3 && n < 10);
            List<Person> people = new()
            {
                new Person {Age = 21, Name = "Arjun"},
                new Person {Age = 22, Name = "Shiva"}
            };

            var adults = people.Where(p => p.Age >= 18);


            //selecting with select
            var doubled = numbers.Select(n => n * 2);

            // Transform to different type
            var doubled1 = numbers.Select(n => n * 2);
            // Result: 2, 4, 6, 8, 10

            var strings = numbers.Select(n => $"Number: {n}");
            // Result: "Number: 1", "Number: 2", ...

            // Project to anonymous type
            // List<Person> people1 = GetPeople();

            var nameAndAge = people.Select(p => new
            {
                p.Name,
                p.Age,
                AgeGroup = p.Age < 30 ? "Young" : "Adult"
            });

            // Project to specific properties
            var names = people.Select(p => p.Name);
            // Result: "Alice", "Bob", "Charlie"

            // Query syntax
            var doubled2 = from n in numbers
                           select n * 2;
            //order with orderBy
            List<int> numbers1 = new() { 5, 2, 8, 1, 9, 3 };
            var ascending = numbers1.OrderBy(n => n);
            foreach (var values in ascending)
            {
                System.Console.WriteLine(values);
            }
            // Descending
            var descending = numbers1.OrderByDescending(n => n);
            // Result: 9, 8, 5, 3, 2, 1\

            // Order by property
            var byAge = people.OrderBy(p => p.Age);

            //multiple ordering 
            var ordered = people.
                        OrderBy(p => p.Age)
                        .ThenBy(p => p.Name);
            //Query Syntax 
            var ordered2 = from p in people
                           orderby p.Age, p.Name
                           select p;

            var ordered3 = from p in people
                           orderby p.Age descending
                           select p;
            //Deferred Execution

            List<int> numbers3 = new() { 1, 2, 3, 4, 5 };

            // Query is DEFINED but NOT EXECUTED
            var query = numbers.Where(n => n > 2);
            Console.WriteLine("Query defined");

            // Query EXECUTES when you iterate
            foreach (int n in query)
            {
                Console.WriteLine(n);  // NOW it executes
            }

            // Modifying source affects query
            numbers.Add(6);
            numbers.Add(7);

            // Query executes AGAIN with new data
            foreach (int n in query)
            {
                Console.WriteLine(n);  // Includes 6 and 7!
            }

            //mmediate Execution with ToList/ToArray
            List<int> numbers4 = new() { 1, 2, 3, 4, 5 };

            // DEFERRED - not executed yet
            var query3 = numbers4.Where(n => n > 2);

            // IMMEDIATE - executes NOW and stores result
            var list = numbers4.Where(n => n > 2).ToList();
            var array = numbers4.Where(n => n > 2).ToArray();

            // Modifying source doesn't affect materialized results
            numbers4.Add(6);

            // query includes 6 (deferred)
            // list does NOT include 6 (already executed)

            Console.WriteLine(query3.Count());  // 4 (includes 6)
            Console.WriteLine(list.Count);     // 3 (snapshot before 6 was added)

            // These trigger immediate execution:
            var list1 = query3.ToList();           // To List
            var array1 = query3.ToArray();         // To Array
            var dict = query3.ToDictionary(n => n); // To Dictionary
            int count = query3.Count();           // Count
            int sum = query3.Sum();               // Sum
            int max = query3.Max();               // Max/Min
            int first = query3.First();           // First/Last
            bool any = query3.Any();              // Any/All

            //Common LINQ Operators 

            //Aggregation

            List<int> nums = new() { 1, 2, 3, 4, 5, 6 };
            int count1 = nums.Count();
            System.Console.WriteLine(count1);
            int sum1 = nums.Sum();
            System.Console.WriteLine(sum1);
            double avg = (int)nums.Average();
            System.Console.WriteLine(avg);
            int min = nums.Min();
            int maxs = nums.Max();

            //             // Count with condition
            // int expensiveCount = products.Count(p => p.Price > 100);

            // // Sum of property
            // decimal totalValue = products.Sum(p => p.Price);

            // // Average of property
            // double avgPrice = products.Average(p => (double)p.Price);

            // // Max/Min of property
            // decimal maxPrice = products.Max(p => p.Price);
            // Product mostExpensive = products.OrderByDescending(p => p.Price).First();

            //FIRST LAST SINGLE

            int first1 = nums.First();
            System.Console.WriteLine(first1);

            int firstEven = nums.First(n => n % 2 == 0);
            System.Console.WriteLine(firstEven);

            // FirstOrDefault - returns default if empty
            int firstOrDefault = numbers.FirstOrDefault(n => n > 10);  // 0
            System.Console.WriteLine(firstOrDefault);

            // Single - throws if 0 or more than 1 element
            List<int> single = new() { 42 };
            int value = single.Single();          // 42

            // SingleOrDefault
            int value2 = numbers.SingleOrDefault(n => n == 3);  // 3

            // ElementAt
            int third = nums.ElementAt(2);     // 3 (zero-based)

            System.Console.WriteLine(third);

            // Any - check if any element matches
            bool hasEvens = numbers.Any(n => n % 2 == 0);     // true
            bool hasNegative = numbers.Any(n => n < 0);       // false
            bool hasAny = numbers.Any();                      // true (has elements)

            // All - check if all elements match
            bool allPositive = numbers.All(n => n > 0);       // true
            bool allEven = numbers.All(n => n % 2 == 0);      // false

            // Contains - check if contains specific value
            bool hasThree = numbers.Contains(3);              // true
            bool hasTen = numbers.Contains(10);               // false

            // List<Product> products = GetProducts();

            // // Check if any products in Electronics
            // bool hasElectronics = products.Any(p => p.Category == "Electronics");

            // // Check if all products are in stock
            // bool allInStock = products.All(p => p.Stock > 0);

            //Distinct, Skip, Take
            // Distinct - remove duplicates
            var unique = numbers.Distinct();
            // Result: 1, 2, 3, 4, 5

            // Skip - skip first N elements
            var skipFirst3 = numbers.Skip(3);
            // Result: 3, 3, 3, 4, 5, 5

            // Take - take first N elements
            var takeFirst3 = numbers.Take(3);
            // Result: 1, 2, 2

            // SkipWhile, TakeWhile
            var skipWhileLessThan3 = numbers.SkipWhile(n => n < 3);
            // Result: 3, 3, 3, 4, 5, 5

            var takeWhileLessThan4 = numbers.TakeWhile(n => n < 4);
            // Result: 1, 2, 2, 3, 3, 3

            //Chaining Operators

            List<Product> products = new()
{
    new Product { Name = "Laptop", Price = 999.99m, Category = "Electronics", Stock = 5 },
    new Product { Name = "Mouse", Price = 25.50m, Category = "Electronics", Stock = 50 },
    new Product { Name = "Desk", Price = 299.99m, Category = "Furniture", Stock = 10 },
    new Product { Name = "Chair", Price = 199.99m, Category = "Furniture", Stock = 15 },
    new Product { Name = "Monitor", Price = 349.99m, Category = "Electronics", Stock = 0 }
};
            var res = products
                    .Where(p => p.Category == "Electronics")
                    .Where(p => p.Stock > 0)
                    .OrderByDescending(p => p.Price)
                    .Select(p =>
                        new
                        {
                            p.Name,
                            p.Price,
                            Discount = p.Price * 0.1m

                        })
                        .Take(3);

            foreach (var item in res)
            {
                System.Console.WriteLine($"{item.Name}: {item.Price} (Save {item.Discount:C})");
            }

            //Query Syntax
            var res2 = (from p in products
                        where p.Category == "Electronics" && p.Stock > 0
                        orderby p.Price descending
                        select new
                        {
                            p.Name,
                            p.Price,
                            Discount = p.Price * 0.1m
                        });

            foreach (var item in res2)
            {
                System.Console.WriteLine($"{item.Name}: {item.Price} (Save {item.Discount:C})");
            }
            {

            }
        }


    }
    public class Person
    {
        public int Age { set; get; }
        public string Name { set; get; }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int Stock { get; set; }


    }
}
