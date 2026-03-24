using System;
using System.Diagnostics;
namespace Name
{
    public class LargeDataProcessor
    {
        private readonly string inputPath;
        private readonly string outputPath;

        public LargeDataProcessor(string inputPath, string outputPath)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
        }
        // 1. Read all lines into memory
        // 2. Parse all products
        // 3. Process
        // 4. Write output
        public void ProcessLoadAll()
        {
            var lines = File.ReadAllLines(inputPath).Skip(1);

            var products = lines.Select(line =>
            {
                var parts = line.Split(',');
                return new Product
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    Price = int.Parse(parts[2]),
                    Category = parts[3],
                    Stock = int.Parse(parts[4]),
                    IsActive = bool.Parse(parts[5])
                };


            }).ToList();

            var processed = products.Where(p => p.IsActive).ToList();

            using var writer = new StreamWriter(outputPath);
            writer.WriteLine("Id,Name,Price,Category");
            foreach (var p in products)
            {
                writer.WriteLine($"{p.Id}, {p.Name}, {p.Price}, {p.Category}");
            }
        }
        // 1. Read line by line
        // 2. Parse on the fly
        // 3. Process incrementally
        // 4. Write incrementally
        public void ProcessStreaming()
        {
            using var reader = new StreamReader(inputPath);
            using var writer = new StreamWriter(outputPath);

            reader.ReadLine();
            writer.WriteLine("Id,Name,Price,Category");
            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');

                var product = new Product
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    Price = decimal.Parse(parts[2]),
                    Category = parts[3],
                    Stock = int.Parse(parts[4]),
                    IsActive = bool.Parse(parts[5])
                };

                // process on the fly
                if (product.IsActive)
                {
                    writer.WriteLine($"{product.Id},{product.Name},{product.Price},{product.Category}");
                }

            }

        }

        // 1. Read in batches
        // 2. Process each batch
        // 3. Aggregate results
        // 4. Write output

        public void ProcessBatches(int batchSize)
        {
            using var reader = new StreamReader(inputPath);
            using var writer = new StreamWriter(outputPath);

            reader.ReadLine();
            writer.WriteLine("Id,Name,Price,Category");
            var batch = new List<Product>();
            string? line;
             while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');
                batch.Add(new Product
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    Price = decimal.Parse(parts[2]),
                    Category = parts[3],
                    Stock = int.Parse(parts[4]),
                    IsActive = bool.Parse(parts[5])
                });
                if (batch.Count >= batchSize)
                {
                    ProcessBatch(batch, writer);
                    batch.Clear();
                }
            }
            if (batch.Count > 0)
                ProcessBatch(batch, writer);
        }
        private void ProcessBatch(List<Product> batch, StreamWriter writer)
        {
            foreach (var p in batch.Where(p => p.IsActive))
            {
                writer.WriteLine($"{p.Id},{p.Name},{p.Price},{p.Category}");
            }
        }

        public CategorySummary[] GetCategorySummaries()
        {
            var dict = new Dictionary<string, (int count, decimal total)>();

            using var reader = new StreamReader(inputPath);
            reader.ReadLine(); // skip header

            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');

                var category = parts[3];
                var price = decimal.Parse(parts[2]);

                if (!dict.ContainsKey(category))
                    dict[category] = (0, 0);

                var current = dict[category];
                dict[category] = (current.count + 1, current.total + price);
            }

            return dict.Select(kvp => new CategorySummary
            {
                Category = kvp.Key,
                Count = kvp.Value.count,
                TotalValue = kvp.Value.total,
                AveragePrice = kvp.Value.total / kvp.Value.count
            }).ToArray();
        }

        public class MemoryMonitor
        {
            public static void MonitorMemory(string name, Action action)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                var before = GC.GetTotalMemory(false);
                var sw = Stopwatch.StartNew();

                action();

                sw.Stop();
                var after = GC.GetTotalMemory(false);
                var used = (after - before) / (1024 * 1024); // MB

                Console.WriteLine($"{name}:");
                Console.WriteLine($"  Time: {sw.ElapsedMilliseconds}ms");
                Console.WriteLine($"  Memory: {used}MB");
            }
        }

    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string Category { get; set; } = "";
        public int Stock { get; set; }
        public bool IsActive { get; set; }
    }

    public class CategorySummary
    {
        public string Category { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }

        public static void GenerateLargeCsv(string path, int rows)
        {
            using var writer = new StreamWriter(path);
            writer.WriteLine("Id,Name,Price,Category,Stock,IsActive");

            var random = new Random();
            var categories = new[] { "Electronics", "Clothing", "Food", "Books" };

            for (int i = 1; i <= rows; i++)
            {
                writer.WriteLine(
                    $"{i},Product{i},{random.Next(10, 1000)}," +
                    $"{categories[random.Next(categories.Length)]}," +
                    $"{random.Next(0, 1000)},{random.Next(2) == 0}");
            }
        }
    }
    class Program
    {
        static void Main()
        {
            string input = "large.csv";
            string output = "output.csv";

            // Generate test data (example: 1 million rows)
            CategorySummary.GenerateLargeCsv(input, 1_000_000);

            var processor = new LargeDataProcessor(input, output);

            // Run all 3 versions with memory monitoring
            MemoryMonitor.MonitorMemory("Load All", () => processor.ProcessLoadAll());

            MemoryMonitor.MonitorMemory("Streaming", () => processor.ProcessStreaming());

            MemoryMonitor.MonitorMemory("Batch (10k)", () => processor.ProcessBatches(10000));
        }
    }

    public class MemoryMonitor
    {
        public static void MonitorMemory(string name, Action action)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var before = GC.GetTotalMemory(false);
            var sw = Stopwatch.StartNew();

            action();

            sw.Stop();
            var after = GC.GetTotalMemory(false);
            var used = (after - before) / (1024 * 1024); // MB

            Console.WriteLine($"{name}:");
            Console.WriteLine($"  Time: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"  Memory: {used}MB");
        }
    }
}

