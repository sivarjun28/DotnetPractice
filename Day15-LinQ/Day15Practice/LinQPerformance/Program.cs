using System;
using System.Security.Cryptography.X509Certificates;
namespace LINQPerformance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 12, 3, 4, 456 };
            // DEFERRED - Creates query (no execution)
            IEnumerable<int> query = numbers
                .Where(n => n > 5)
                .Select(n => n * 2);
            // Executes on enumeration
            foreach (var n in query) { }  // Executes
            var list = query.ToList();    // Executes
            var arr = query.ToArray();    // Executes
            var first = query.First();    // Executes (partial)

            // IMMEDIATE - Executes immediately
            var count = numbers.Count(n => n > 5);           // Executes
            var max = numbers.Max();                         // Executes
            var any = numbers.Any(n => n > 100);            // Executes
            var single = numbers.Single(n => n == 5);       // Executes
            var list2 = numbers.Where(n => n > 5).ToList(); // Executes
            List<Product> products = Product.GetProducts();

            // ❌ BAD - Multiple enumerations
            IEnumerable<Product> ExpensiveQuery()
            {
                return products
                    // .Where(p => ComplexFilter(p))  // Expensive operation
                    .OrderBy(p => p.Price);
            }

            var results = ExpensiveQuery();
            // Each line enumerates and calls ComplexFilter again!
            int count1 = results.Count();        // 1st enumeration
            decimal sum = results.Sum(p => p.Price);   // 2nd enumeration
            Product? first1 = results.FirstOrDefault(); // 3rd enumeration

            //Good Single Enumeration
            var results1 = ExpensiveQuery().ToList();//Enumerate once
            int count2 = results1.Count; // no Enumerataion
            decimal sum1 = results1.Sum(p => p.Price); // no Enumerataion
            Product? first2 = results1.FirstOrDefault(); // No enumeration

            //Filter Early, Transform Late
            // ❌ BAD - Filtering after expensive operations
            var result = products
                        .Select(p => new  // Creates many objects
                        {
                            p.Id,
                            p.Name,
                            p.Price,
                            FormattedPrice = $"${p.Price:F2}",
                            UpperName = p.Name.ToUpper()
                        })
    .Where(x => x.Price > 100)  // Filters after transformation
    .Take(10);
            // ✅ GOOD - Filter first

            var res = products
                    .Where(p => p.Price > 100)
                    .Take(10)
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Price,
                        FormatedPrice = $"${p.Price: F2}",
                        UpperName = p.Name.ToUpper()
                    });

            // When TO use ToList():
            // 1. When you need to enumerate multiple times
            var activeProducts = products.Where(p => p.IsActive).ToList();
            var count3 = activeProducts.Count;     // Fast
            var max3 = activeProducts.Max(p => p.Price);   // Fast

            // 2. When you need to avoid closure/context issues

            List<Product> results2 = new();
            // foreach (var category in categories)
            // {
            //     // ToList() captures current category value
            //     var items = products
            //         .Where(p => p.Category == category)
            //         .ToList();


            // }


            //Part 2: LINQ vs Traditional Loops

            // ✅ GOOD - LINQ is clearer
            var highPriceProducts = products
                .Where(p => p.Price > 100)
                .OrderBy(p => p.Name)
                .ToList();
            // vs Traditional loop (more verbose)
            var highPriceProducts_Trad = new List<Product>();
            foreach (var p in products)
            {
                if (p.Price > 100)
                {
                    highPriceProducts.Add(p);
                }
            }
            highPriceProducts_Trad.Sort((a, b) => a.Name.CompareTo(b.Name));

            // ✅ GOOD - LINQ for transformations
            var names = products
                .Select(p => p.Name)
                .ToList();

            // vs
            var names_Trad = new List<string>();
            foreach (var p in products)
            {
                names_Trad.Add(p.Name);
            }
            // Scenario: Sum of squares of even numbers

            // LINQ
            var sum3 = numbers
                .Where(n => n % 2 == 0)
                .Select(n => n * n)
                .Sum();

            // For loop
            int sum_L = 0;
            foreach (var n in numbers)
            {
                if (n % 2 == 0)
                {
                    sum += n * n;
                }
            }

            // Results for 1 million numbers:
            // LINQ:    ~45ms (more allocations)
            // For loop: ~25ms (no allocations)

            // Use LINQ when:
            // - Readability matters more
            // - Data size is small/medium
            // - Not in tight performance loop

            // Use loops when:
            // - Maximum performance critical
            // - Very large datasets
            // - Complex state management
            // - Early termination needed


            //Memory Mnaagement

            // ❌ BAD - Many intermediate lists
            var result4 = products
                .Where(p => p.IsActive).ToList()        // List 1
                .Where(p => p.Price > 100).ToList()     // List 2
                .OrderBy(p => p.Name).ToList()          // List 3
                .Take(10).ToList();                     // List 4

            // ✅ GOOD - Single materialization
            var result5 = products
                .Where(p => p.IsActive)
                .Where(p => p.Price > 100)
                .OrderBy(p => p.Name)
                .Take(10)
                .ToList();  // Single list
            //Good use Query Syntax
            var result6 = (
                from p in products
                where p.IsActive && p.Price > 100
                orderby p.Name
                select p
            ).Take(10).ToList();


            // ❌ BAD - Second OrderBy replaces first!
            var result7 = products
                .OrderBy(p => p.Category)
                .OrderBy(p => p.Price);  // Only sorted by Price!

            // ✅ GOOD - Use ThenBy for secondary sort
            var result8 = products
                .OrderBy(p => p.Category)
                .ThenBy(p => p.Price);


            //Best Practices Summary
            //Filter Early
            products.Where(p => p.IsActive).Select(p => p.Name);
            //Use appropriate operators
            products.Any(p => p.Price > 100); //Not: count() > 0
            products.FirstOrDefault();      //// Not: Where().First()

            // 4. Avoid multiple enumerations
            var list_l = query.ToList();
            var count_c = list.Count;  // Not: query.Count()
            var max_m = list.Max();    // Not: query.Max()

            // 5. Use ToDictionary for lookups
            var lookup = products.ToDictionary(p => p.Id);
            var product = lookup[1];  // O(1) lookup
                                      // 6. Consider parallel for CPU-bound work
                                      // var result = largeList
                                      //     .AsParallel()
                                      //     .Where(expensive condition)
                                      //     .ToList();
                                      // 1. Don't chain ToList/ToArray unnecessarily
            /* .Where().ToList().OrderBy().ToList()

            // 2. Don't use LINQ for side effects
            .Select(x => { x.Process(); return x; })

            // 3. Don't use Count() to check existence
            if (items.Count() > 0)  // Use: items.Any()

            // 4. Don't filter after ordering entire set
            .OrderBy().Where().Take(10)

            // 5. Don't ignore context in Entity Framework
            dbContext.Products.ToList().Where()  // Bad
            dbContext.Products.Where().ToList()  // Good */

            // ✅ GOOD - Clear single responsibility
            var activeProducts1 = products
                .Where(p => p.IsActive)
                .ToList();

            var sortedActive = activeProducts
                .OrderBy(p => p.Name)
                .ToList();

            // ❌ BAD - Too complex
            var result_r = products
                .Where(p => p.IsActive)
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Products = g.OrderBy(p => p.Price)
                                .Take(5)
                                .Select(p => new { p.Name, p.Price })
                })
                .OrderByDescending(x => x.Products.Sum(p => p.Price))
                .ToList();
            //// ✅ BETTER - Break down
            var activeByCategory = products
                                .Where(p => p.IsActive)
                                .GroupBy(p => p.Category);
            var topProductByCategory = activeByCategory
                                        .Select(g => new
                                        {
                                            Category = g.Key,
                                            // Products = GetTopPOroducts(g, 5)
                                        });
                                        

        }
        //When to Use Loops
        public bool HasExpensivaProducts(List<Product> products)
        {
            foreach (var item in products)
            {
                if (item.Price > 100)
                {
                    return true;//Early yTermination 
                }
            }
            return false;
        }
        // ❌ BAD - LINQ processes all elements
        public bool HasExpensiveProduct(List<Product> products)
        {
            return products
                .Select(p => p.Price > 1000)  // Processes all!
                .Any(result => result);
        }
        // ✅ BETTER - But still use Any() correctly
        public bool HasExpensivaProducts1(List<Product> products)
        {
            return products.Any(p => p.Price > 1000);
        }
        // ✅ GOOD - Loop for complex state
        public Dictionary<string, int> CountByCategory(List<Product> products)
        {
            var counts = new Dictionary<string, int>();
            foreach (var item in products)
            {
                if (!counts.ContainsKey(item.Category))
                {
                    counts[item.Category] = 0;
                }
                counts[item.Category]++;
            }
            return counts;
        }

        //     // ❌ WORSE - LINQ is less clear here
        // var counts = products
        //     .GroupBy(p => p.Category)
        //     .ToDictionary(g => g.Key, g => g.Count());
        // ✅ GOOD - Loop for side effects
        public void UpdatePrices(List<Product> products, decimal multiplier)
        {
            foreach (var p in products)
            {
                p.Price *= multiplier;
            }


        }

        // ❌ BAD - LINQ not designed for side effects
        public void UpdatePrices_LinQ(List<Product> products, decimal multiplier)
        {
            products.
                     Select(p => { p.Price *= multiplier; return p; })
                     .ToList();
        }

    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsActive = true;

        public static List<Product> GetProducts()
        {
            return new List<Product>
        {
            new Product { Id = 1, Name = "Milk", Price = 2, Category = "Dairy" },
            new Product { Id = 1, Name = "Apple", Price = 1, Category = "Fruits" },
            new Product { Id = 1, Name = "Bread", Price = 3, Category = "Bakery" }
        };
        }
    }
}