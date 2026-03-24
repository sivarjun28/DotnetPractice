using System;
namespace Exercise04
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class ProductStats
    {
        public int Count { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AvgPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }

    //Good Queries
    public class GoodQueries
    {
        private readonly List<Product> products;
        public GoodQueries (List<Product> products)
        {
            this.products = products;
        }

        public ProductStats GetStats()
        {
            //Optimize to single enumeration
            var result = products
     .Where(p => p.IsActive)
     .Aggregate(
         new { Count = 0, Total = 0m, Max = 0m },
         (acc, p) => new
         {
             Count = acc.Count + 1,
             Total = acc.Total + p.Price,
             Max = acc.Count == 0 ? p.Price : Math.Max(acc.Max, p.Price)
         }
     );

            return new ProductStats
            {
                Count = result.Count,
                TotalValue = result.Total,
                AvgPrice = result.Count == 0 ? 0 : result.Total / result.Count,
                MaxPrice = result.Max
            };
        }

        public List<Product> GetExpensiveElectronics()
        {
            // Filter first, then order, then transform
            return products
                .Where(p => p.Price > 500 && p.Category == "Electronics")
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name.ToUpper(),
                    Price = p.Price,
                    Category = p.Category,
                    IsActive = p.IsActive
                }).ToList();
        }
        public List<string> GetCategoryNames()
        {
            // Simplify the query
            return products
                 .Where(p => p.IsActive)
                 .Select(p => p.Category)
                 .Distinct()
                 .OrderBy(c => c)
                 .ToList();
        }

        public Dictionary<string, int> CountByCategory()
        {
            // Use GroupBy
            return products
                     .GroupBy(p => p.Category)
                     .ToDictionary(g => g.Key, g => g.Count());
        }

        public bool HasProducts()
        {
            // Use Any()
            return products.Any();
        }

        public Product? GetFirst()
        {
            // Use FirstOrDefault
            return products.FirstOrDefault(p => p.IsActive);
        }
    }

    class Program
    {
        static void Main()
        {
            // Sample products
            var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1200, Category = "Electronics", IsActive = true },
            new Product { Id = 2, Name = "Phone", Price = 800, Category = "Electronics", IsActive = true },
            new Product { Id = 3, Name = "Desk", Price = 300, Category = "Furniture", IsActive = false },
            new Product { Id = 4, Name = "Monitor", Price = 400, Category = "Electronics", IsActive = true },
            new Product { Id = 5, Name = "Chair", Price = 150, Category = "Furniture", IsActive = true },
        };

            // Initialize GoodQueries
            var queries = new GoodQueries(products);

            // Get stats
            var stats = queries.GetStats();
            Console.WriteLine("Product Stats:");
            Console.WriteLine($"Count: {stats.Count}");
            Console.WriteLine($"Total Value: {stats.TotalValue}");
            Console.WriteLine($"Average Price: {stats.AvgPrice}");
            Console.WriteLine($"Max Price: {stats.MaxPrice}");
            Console.WriteLine();

            // Get expensive electronics
            var expensiveElectronics = queries.GetExpensiveElectronics();
            Console.WriteLine("Expensive Electronics:");
            foreach (var p in expensiveElectronics)
            {
                Console.WriteLine($"{p.Name} - {p.Price:C}");
            }
            Console.WriteLine();

            // Get category names
            var categories = queries.GetCategoryNames();
            Console.WriteLine("Active Categories:");
            foreach (var c in categories)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine();

            // Count by category
            var countByCategory = queries.CountByCategory();
            Console.WriteLine("Count By Category:");
            foreach (var kv in countByCategory)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }
            Console.WriteLine();

            // Check if any products
            Console.WriteLine($"Has Products? {queries.HasProducts()}");

            // Get first active product
            var firstActive = queries.GetFirst();
            Console.WriteLine($"First Active Product: {(firstActive != null ? firstActive.Name : "None")}");
        }
    }
}
