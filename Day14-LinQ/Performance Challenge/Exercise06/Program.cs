using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace Exercise06
{

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Generating dataset...");
            var products = GenerateLargeDataset(1_000_000);

            var categories = new List<string> { "Electronics", "Clothing", "Food" };

            Console.WriteLine("Running benchmarks...\n");

            PerformanceTester.BenchmarkQuery("TopProducts - Naive", () =>
            {
                GetTopProductsByCategory_Naive(products);
            });

            PerformanceTester.BenchmarkQuery("TopProducts - Optimized", () =>
            {
                GetTopProductsByCategory_Optimized(products);
            });

            PerformanceTester.BenchmarkQuery("CategoryStats - Naive", () =>
            {
                GetCategoryStatistics_Naive(products, categories);
            });

            PerformanceTester.BenchmarkQuery("CategoryStats - Optimized", () =>
            {
                GetCategoryStatistics_Optimized(products, categories);
            });

            PerformanceTester.BenchmarkQuery("Search - Naive", () =>
            {
                SearchProducts_Naive(products, 100, 500, categories, 3);
            });

            PerformanceTester.BenchmarkQuery("Search - Optimized", () =>
            {
                SearchProducts_Optimized(products, 100, 500, categories, 3);
            });

            PerformanceTester.BenchmarkQuery("Analysis - Sequential", () =>
            {
                AnalyzeProducts_Sequential(products);
            });

            PerformanceTester.BenchmarkQuery("Analysis - Parallel", () =>
            {
                AnalyzeProducts_Parallel(products);
            });
        }

        // -------------------------------
        // 1. Top 100 by category
        // -------------------------------

        public static Dictionary<string, List<Product>> GetTopProductsByCategory_Naive(List<Product> products)
        {
            return products
                .GroupBy(p => p.Category)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(p => p.Price).Take(100).ToList()
                );
        }

        public static Dictionary<string, List<Product>> GetTopProductsByCategory_Optimized(List<Product> products)
        {
            var dict = new Dictionary<string, PriorityQueue<Product, decimal>>();

            foreach (var p in products)
            {
                if (!dict.TryGetValue(p.Category, out var pq))
                {
                    pq = new PriorityQueue<Product, decimal>();
                    dict[p.Category] = pq;
                }

                pq.Enqueue(p, p.Price);

                if (pq.Count > 100)
                    pq.Dequeue();
            }

            return dict.ToDictionary(
                kv => kv.Key,
                kv => kv.Value.UnorderedItems
                             .Select(x => x.Element)
                             .OrderByDescending(p => p.Price)
                             .ToList()
            );
        }

        // -------------------------------
        // 2. Category Statistics
        // -------------------------------

        public static Dictionary<string, CategoryStats> GetCategoryStatistics_Naive(
            List<Product> products,
            List<string> categories)
        {
            var result = new Dictionary<string, CategoryStats>();

            foreach (var category in categories)
            {
                var filtered = products.Where(p => p.Category == category).ToList();

                result[category] = new CategoryStats
                {
                    Count = filtered.Count,
                    AveragePrice = filtered.Any() ? filtered.Average(p => p.Price) : 0,
                    TotalValue = filtered.Sum(p => p.Price * p.Stock),
                    LowStockCount = filtered.Count(p => p.Stock < 10)
                };
            }

            return result;
        }

        public static Dictionary<string, CategoryStats> GetCategoryStatistics_Optimized(
            List<Product> products,
            List<string> categories)
        {
            var categorySet = new HashSet<string>(categories);

            return products
                .Where(p => categorySet.Contains(p.Category))
                .GroupBy(p => p.Category)
                .ToDictionary(
                    g => g.Key,
                    g => new CategoryStats
                    {
                        Count = g.Count(),
                        AveragePrice = g.Average(p => p.Price),
                        TotalValue = g.Sum(p => p.Price * p.Stock),
                        LowStockCount = g.Count(p => p.Stock < 10)
                    });
        }

        // -------------------------------
        // 3. Search Products
        // -------------------------------

        public static List<Product> SearchProducts_Naive(
            List<Product> products,
            decimal? minPrice,
            decimal? maxPrice,
            List<string>? categories,
            int? minRating)
        {
            return products
                .Where(p =>
                    (!minPrice.HasValue || p.Price >= minPrice) &&
                    (!maxPrice.HasValue || p.Price <= maxPrice) &&
                    (categories == null || categories.Contains(p.Category)) &&
                    (!minRating.HasValue || p.Rating >= minRating))
                .ToList();
        }

        public static List<Product> SearchProducts_Optimized(
            List<Product> products,
            decimal? minPrice,
            decimal? maxPrice,
            List<string>? categories,
            int? minRating)
        {
            var query = products.AsEnumerable();

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (categories != null && categories.Count > 0)
            {
                var set = new HashSet<string>(categories);
                query = query.Where(p => set.Contains(p.Category));
            }

            if (minRating.HasValue)
                query = query.Where(p => p.Rating >= minRating.Value);

            return query.ToList();
        }

        // -------------------------------
        // 4. Product Analysis
        // -------------------------------

        public static List<ProductAnalysis> AnalyzeProducts_Sequential(List<Product> products)
        {
            return products.Select(p => new ProductAnalysis
            {
                ProductId = p.Id,
                Score = (p.Rating * 20) + (p.Stock / 10m)
            }).ToList();
        }

        public static List<ProductAnalysis> AnalyzeProducts_Parallel(List<Product> products)
        {
            return products
                .AsParallel()
                .Select(p => new ProductAnalysis
                {
                    ProductId = p.Id,
                    Score = (p.Rating * 20) + (p.Stock / 10m)
                })
                .ToList();
        }

        // -------------------------------
        // Models
        // -------------------------------

        public class CategoryStats
        {
            public int Count { get; set; }
            public decimal AveragePrice { get; set; }
            public decimal TotalValue { get; set; }
            public int LowStockCount { get; set; }
        }

        public class ProductAnalysis
        {
            public int ProductId { get; set; }
            public decimal Score { get; set; }
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string Category { get; set; } = string.Empty;
            public int Stock { get; set; }
            public int Rating { get; set; }
        }

        // -------------------------------
        // Data Generator
        // -------------------------------

        public static List<Product> GenerateLargeDataset(int count)
        {
            var random = new Random(42);
            var categories = new[] { "Electronics", "Clothing", "Food", "Books", "Toys" };

            return Enumerable.Range(1, count)
                .Select(i => new Product
                {
                    Id = i,
                    Name = $"Product {i}",
                    Price = (decimal)(random.NextDouble() * 1000),
                    Category = categories[random.Next(categories.Length)],
                    Stock = random.Next(0, 1000),
                    Rating = random.Next(1, 6)
                })
                .ToList();
        }

        // -------------------------------
        // Benchmark Helper
        // -------------------------------

        public class PerformanceTester
        {
            public static void BenchmarkQuery(string name, Action query, int iterations = 3)
            {
                var times = new List<long>();

                for (int i = 0; i < iterations; i++)
                {
                    var sw = Stopwatch.StartNew();
                    query();
                    sw.Stop();
                    times.Add(sw.ElapsedMilliseconds);
                }

                Console.WriteLine($"{name}:");
                Console.WriteLine($"  Avg: {times.Average():F2} ms");
                Console.WriteLine($"  Min: {times.Min()} ms");
                Console.WriteLine($"  Max: {times.Max()} ms\n");
            }
        }
    }
}