using System;
using System.Diagnostics;
namespace Exercise01
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Stock { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Rating { get; set; }
        // Generate large dataset
        public static List<Product> GenerateProducts(int count)
        {
            var random = new Random(42);
            var categories = new[] { "Electronics", "Clothing", "Food", "Books", "Toys", "Sports" };

            return Enumerable.Range(1, count)
                .Select(i => new Product
                {
                    Id = i,
                    Name = $"Product {i}",
                    Price = (decimal)(random.NextDouble() * 1000),
                    Category = categories[random.Next(categories.Length)],
                    Stock = random.Next(0, 1000),
                    IsActive = random.Next(2) == 0,
                    CreatedDate = DateTime.Now.AddDays(-random.Next(365)),
                    Rating = random.Next(1, 6)
                })
                .ToList();
        }
    }

    //Task 1: Complex Filtering
    // Find products matching multiple criteria
    public class ProductSearcher
    {
        private readonly List<Product> products;

        public ProductSearcher(List<Product> products)
        {
            this.products = products;
        }

        // Version 1: Naive implementation
        public List<Product> SearchNaive(
            decimal? minPrice,
            decimal? maxPrice,
            List<string>? categories,
            int? minRating,
            bool? isActive)
        {
            // Implement with separate Where clauses
            // Apply each filter separately

            IEnumerable<Product> query = products;

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);
            if (categories != null && categories.Any())
                query = query.Where(p => categories.Contains(p.Category));
            if (minRating.HasValue)
                query = query.Where(p => p.Rating >= minRating);
            if (isActive.HasValue)
                query = query.Where(p => p.IsActive == isActive.Value);
            return query.ToList();
        }

        // Version 2: Optimized implementation
        public List<Product> SearchOptimized(
            decimal? minPrice,
            decimal? maxPrice,
            List<string>? categories,
            int? minRating,
            bool? isActive)
        {
            // Implement with combined predicate
            // Build single where clause
            return
                products.
                Where(p => (!minPrice.HasValue || p.Price >= minPrice.Value) &&
                (!maxPrice.HasValue || p.Price <= maxPrice.Value) &&
                (categories == null || categories.Count == 0 || categories.Contains(p.Category)) &&
                (!minRating.HasValue || p.Rating >= minRating.Value) &&
                (!isActive.HasValue || p.IsActive == isActive.Value)
                ).ToList();
        }
    }
    // Task 2: Category Statistics
    public class CategoryStats
    {
        public string Category { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
        public int ActiveCount { get; set; }
        public int LowStockCount { get; set; }  // Stock < 50
    }
    public class CategoryAnalyzer
    {
        private readonly List<Product> products;

        public CategoryAnalyzer(List<Product> products)
        {
            this.products = products;
        }
        // Version 1: Multiple queries
        public Dictionary<string, CategoryStats> GetStatsNaive()
        {
            var categories = products.Select(p => p.Category).Distinct();
            var result = new Dictionary<string, CategoryStats>();

            foreach (var category in categories)
            {
                var categoryProducts = products.Where(p => p.Category == category);

                var stats = new CategoryStats
                {
                    Category = category,
                    ProductCount = categoryProducts.Count(),
                    TotalValue = categoryProducts.Sum(p => p.Price),
                    AveragePrice = categoryProducts.Average(p => p.Price),
                    ActiveCount = categoryProducts.Count(p => p.IsActive),
                    LowStockCount = categoryProducts.Count(p => p.Stock < 50)
                };

                result[category] = stats;
            }

            return result;
        }

        // Version 2: Single pass
        public Dictionary<string, CategoryStats> GetStatsOptimized()
        {
            return products
                .GroupBy(p => p.Category)
                .ToDictionary(
                    g => g.Key,
                    g => new CategoryStats
                    {
                        Category = g.Key,
                        ProductCount = g.Count(),
                        TotalValue = g.Sum(p => p.Price),
                        AveragePrice = g.Average(p => p.Price),
                        ActiveCount = g.Count(p => p.IsActive),
                        LowStockCount = g.Count(p => p.Stock < 50)
                    });
        }
    }

    // Top N Per Category
    public class TopProductsFinder
    {
        private readonly List<Product> products;

        public TopProductsFinder(List<Product> products)
        {
            this.products = products;
        }

        // Version 1: Separate queries
        public Dictionary<string, List<Product>> GetTopNNaive(int n)
        {
            var categories = products.Select(p => p.Category).Distinct();
            var result = new Dictionary<string, List<Product>>();

            foreach (var category in categories)
            {
                //: Query products for each category separately

                var topProducts = products
                            .Where(p => p.Category == category)
                            .OrderByDescending(p => p.Price)
                            .Take(n)
                            .ToList();
                result[category] = topProducts;

            }

            return result;
        }

        // Version 2: Single query with grouping
        public Dictionary<string, List<Product>> GetTopNOptimized(int n)
        {
            // Group and take in single pass
            return products
                 .GroupBy(p => p.Category)
                 .ToDictionary(
                     g => g.Key,
                     g => g.OrderByDescending(p => p.Price)
                     .Take(n)
                     .ToList()
                 );
        }


    }
    // Parallel Processing
    public class ProductScore
    {
        public int ProductId { get; set; }
        public double Score { get; set; }
    }
    public class ProductProcessor
    {
        // Simulate expensive computation
        private static double ExpensiveCalculation(Product product)
        {
            double result = 0;
            for (int i = 0; i < 10000; i++)
            {
                result += Math.Sqrt((double)product.Price) * Math.Log(product.Stock + 1);
            }
            return result;
        }

        // Version 1: Sequential
        public List<ProductScore> ProcessSequential(List<Product> products)
        {
            //Process each product sequentially
            var result = new List<ProductScore>();
            foreach (var product in products)
            {
                var score = ExpensiveCalculation(product);
                result.Add(new ProductScore
                {
                    ProductId = product.Id,
                    Score = score
                });
            }
            return result;

        }

        // Version 2: Parallel (PLINQ)
        public List<ProductScore> ProcessParallel(List<Product> products)
        {
            //  Use AsParallel() for parallel processing
            return
                products.
                    AsParallel()
                    .Select(product =>
                    new ProductScore
                    {
                        ProductId = product.Id,
                        Score = ExpensiveCalculation(product)
                    }).ToList();
        }

        // Version 3: Parallel with optimal degree
        public List<ProductScore> ProcessParallelOptimized(List<Product> products)
        {
            //  Use WithDegreeOfParallelism
            return products
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .Select(product => new ProductScore
                {
                    ProductId = product.Id,
                    Score = ExpensiveCalculation(product)
                }).ToList();
        }
    }
    public class PerformanceBenchmark
    {
        public static BenchmarkResult Measure(string name, Action action, int iterations = 5)
        {
            // Warmup
            action();

            var times = new List<long>();
            var memoryBefore = GC.GetTotalMemory(true);

            for (int i = 0; i < iterations; i++)
            {
                var sw = Stopwatch.StartNew();
                action();
                sw.Stop();
                times.Add(sw.ElapsedMilliseconds);
            }

            var memoryAfter = GC.GetTotalMemory(false);
            var memoryUsed = memoryAfter - memoryBefore;

            return new BenchmarkResult
            {
                Name = name,
                AverageMs = times.Average(),
                MinMs = times.Min(),
                MaxMs = times.Max(),
                MemoryUsedBytes = memoryUsed
            };
        }

        public static void CompareBenchmarks(BenchmarkResult baseline, BenchmarkResult optimized)
        {
            var speedup = baseline.AverageMs / optimized.AverageMs;
            var memoryReduction =
                (baseline.MemoryUsedBytes - optimized.MemoryUsedBytes) /
                (double)baseline.MemoryUsedBytes * 100;

            Console.WriteLine($"\n=== Performance Comparison ===");
            Console.WriteLine($"Baseline:  {baseline.AverageMs:F2}ms");
            Console.WriteLine($"Optimized: {optimized.AverageMs:F2}ms");
            Console.WriteLine($"Speedup:   {speedup:F2}x");
            Console.WriteLine($"Memory saved: {memoryReduction:F1}%");
        }
    }

    public class BenchmarkResult
    {
        public string Name { get; set; } = string.Empty;
        public double AverageMs { get; set; }
        public long MinMs { get; set; }
        public long MaxMs { get; set; }
        public long MemoryUsedBytes { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            // Generate datasets
            var products1K = Product.GenerateProducts(1_000);
            var products10K = Product.GenerateProducts(10_000);
            var products100K = Product.GenerateProducts(100_000);

            // ----------------------------
            // Task 1: Complex Filtering
            var searcher = new ProductSearcher(products100K);

            var result1 = PerformanceBenchmark.Measure("Search Naive", () =>
            {
                searcher.SearchNaive(100, 500, new List<string> { "Electronics" }, 3, true);
            });

            var result2 = PerformanceBenchmark.Measure("Search Optimized", () =>
            {
                searcher.SearchOptimized(100, 500, new List<string> { "Electronics" }, 3, true);
            });

            PerformanceBenchmark.CompareBenchmarks(result1, result2);

            // ----------------------------
            // Task 2: Category Statistics
            var analyzer = new CategoryAnalyzer(products100K);

            var statsNaive = PerformanceBenchmark.Measure("Category Stats Naive", () =>
            {
                analyzer.GetStatsNaive();
            });

            var statsOptimized = PerformanceBenchmark.Measure("Category Stats Optimized", () =>
            {
                analyzer.GetStatsOptimized();
            });

            PerformanceBenchmark.CompareBenchmarks(statsNaive, statsOptimized);

            // ----------------------------
            // Task 3: Top N Products per Category
            var topFinder = new TopProductsFinder(products100K);
            int topN = 5;

            var topNaive = PerformanceBenchmark.Measure("Top N Naive", () =>
            {
                topFinder.GetTopNNaive(topN);
            });

            var topOptimized = PerformanceBenchmark.Measure("Top N Optimized", () =>
            {
                topFinder.GetTopNOptimized(topN);
            });

            PerformanceBenchmark.CompareBenchmarks(topNaive, topOptimized);

            // ----------------------------
            // Task 4: Parallel Processing
            var processor = new ProductProcessor();

            var sequential = PerformanceBenchmark.Measure("Sequential Processing", () =>
            {
                processor.ProcessSequential(products10K);
            });

            var parallel = PerformanceBenchmark.Measure("Parallel Processing", () =>
            {
                processor.ProcessParallel(products10K);
            });

            var parallelOptimized = PerformanceBenchmark.Measure("Parallel Optimized", () =>
            {
                processor.ProcessParallelOptimized(products10K);
            });

            PerformanceBenchmark.CompareBenchmarks(sequential, parallel);
            PerformanceBenchmark.CompareBenchmarks(sequential, parallelOptimized);
        }
    }


}