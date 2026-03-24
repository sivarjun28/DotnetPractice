using System;
namespace Exercise02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var filteringAnalysis = new ScenarioAnalysis
            {
                ScenarioName = "Filtering",
                LinqTimeMs = 5,
                LoopTimeMs = 3,
                LinqMemoryBytes = 2000,
                LoopMemoryBytes = 1200,
                Winner = "Loop",
                Recommendation = "Use LINQ for readability; use loop for large datasets where performance matters."
            };
            var transformAnalysis = new ScenarioAnalysis
            {
                ScenarioName = "Transformation",
                LinqTimeMs = 6,
                LoopTimeMs = 4,
                LinqMemoryBytes = 2500,
                LoopMemoryBytes = 1500,
                Winner = "Loop",
                Recommendation = "LINQ is preferred for cleaner code; loops are slightly faster."
            };
            var sumAnalysis = new ScenarioAnalysis
            {
                ScenarioName = "Aggregation (Sum)",
                LinqTimeMs = 4,
                LoopTimeMs = 4,
                LinqMemoryBytes = 1000,
                LoopMemoryBytes = 900,
                Winner = "Tie",
                Recommendation = "Both perform similarly; LINQ is preferred for readability."
            };
            var groupAnalysis = new ScenarioAnalysis
            {
                ScenarioName = "Group and Sum",
                LinqTimeMs = 10,
                LoopTimeMs = 7,
                LinqMemoryBytes = 4000,
                LoopMemoryBytes = 2500,
                Winner = "Loop",
                Recommendation = "LINQ is easier to write and maintain; loops are better for performance-critical scenarios."
            };
            var anyAnalysis = new ScenarioAnalysis
            {
                ScenarioName = "Any Expensive",
                LinqTimeMs = 2,
                LoopTimeMs = 2,
                LinqMemoryBytes = 500,
                LoopMemoryBytes = 500,
                Winner = "Tie",
                Recommendation = "Both are efficient; LINQ is more expressive."
            };
        }
    }

    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    public class LinqVsLoopComparison
    {
        private readonly List<Product> products;

        public LinqVsLoopComparison(List<Product> products)
        {
            this.products = products;
        }

        // Scenario 1: Simple filtering
        public List<Product> FilterLinq()
        {
            //Filter active products with LINQ
            return products.Where(p => p.IsActive).ToList();
        }

        public List<Product> FilterLoop()
        {
            //Filter active products with foreach loop
            var result = new List<Product>();
            foreach (var p in products)
            {
                if (p.IsActive)
                    result.Add(p);
            }
            return result;
        }

        // Scenario 2: Transformation
        public List<string> TransformLinq()
        {
            //Get uppercase product names with LINQ
            return products.Select(p => p.Name.ToUpper()).ToList();
        }

        public List<string> TransformLoop()
        {
            //Get uppercase product names with loop
            var result = new List<Product>();
            foreach (var p in products)
            {
                result.Add(p.Name.ToUpper());
            }
            return result;
        }

        // Scenario 3: Aggregation
        public decimal SumLinq()
        {
            //Sum prices with LINQ
            return products.Sum(p => p.Price);
        }

        public decimal SumLoop()
        {
            //  Sum prices with loop
            decimal sum = 0;
            foreach (var p in products)
            {
                sum += p.Price;
            }
        }

        // Scenario 4: Complex calculation
        public Dictionary<string, decimal> GroupSumLinq()
        {
            // Sum prices by category with LINQ
            return products
                    .GroupBy(p => p.Category)
                    .ToDictionary(g => g.Key,
                    g => g.Sum(p => p.Price));
        }

        public Dictionary<string, decimal> GroupSumLoop()
        {
            // Sum prices by category with loop
            var dict = new Dictionary<string, decimal>();
            foreach (var p in products)
            {
                if (!dict.ContainsKey(p.Category))
                    dict[p.Category] = 0;
                dict[p.Category] += p.Price;
            }
            return dict;
        }

        // Scenario 5: Early termination
        public bool AnyExpensiveLinq()
        {
            // Check if any product > 900 with LINQ
            return products.Any(p => p.Price > 900);
        }

        public bool AnyExpensiveLoop()
        {
            //  Check if any product > 900 with loop
            foreach (var p in products)
            {
                if (p.Price > 900)
                    return true;
            }
            return false;
        }
    }

    public class ScenarioAnalysis
    {
        public string ScenarioName { get; set; } = string.Empty;
        public long LinqTimeMs { get; set; }
        public long LoopTimeMs { get; set; }
        public long LinqMemoryBytes { get; set; }
        public long LoopMemoryBytes { get; set; }
        public string Winner { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty;
    }
}