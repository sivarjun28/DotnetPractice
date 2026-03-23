using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Exercise04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var processor = new CsvProcessor<Product>("products.csv");

            var products = processor.ReadAll();

            Console.WriteLine("All Products:");
            foreach (var p in products)
            {
                Console.WriteLine($"{p.Id} - {p.Name} - {p.Price} - {p.Category} - {p.Stock}");
            }

            var validProducts = processor.ReadWithValidation(p => p.Price > 0 && p.Stock >= 0);

            Console.WriteLine("\nValid Products Count: " + validProducts.Count);

            processor.WriteCsv(validProducts, "output.csv");

            var summary = processor.GetSummary();

            Console.WriteLine("\nSummary:");
            foreach (var kv in summary)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }
        }
    }

    public class CsvProcessor<T> where T : new()
    {
        private readonly string filePath;
        private readonly char delimiter;

        public CsvProcessor(string filePath, char delimiter = ',')
        {
            this.filePath = filePath;
            this.delimiter = delimiter;
        }

        // ✅ READ ALL
        public List<T> ReadAll()
        {
            var lines = File.ReadAllLines(filePath);
            if (lines.Length <= 1) return new List<T>();

            var headers = lines[0].Split(delimiter);
            var properties = typeof(T).GetProperties();

            var result = new List<T>();

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(delimiter);
                var obj = new T();

                for (int i = 0; i < headers.Length; i++)
                {
                    var prop = properties.FirstOrDefault(p =>
                        string.Equals(p.Name, headers[i], StringComparison.OrdinalIgnoreCase));

                    if (prop != null && prop.CanWrite)
                    {
                        object val = ConvertValue(values[i], prop.PropertyType);
                        prop.SetValue(obj, val);
                    }
                }

                result.Add(obj);
            }

            return result;
        }

        // ✅ READ WITH VALIDATION
        public List<T> ReadWithValidation(Func<T, bool> validator)
        {
            return ReadAll()
                    .Where(validator)
                    .ToList();
        }

        // ✅ WRITE CSV
        public void WriteCsv(List<T> data, string outputPath)
        {
            var props = typeof(T).GetProperties();

            using var writer = new StreamWriter(outputPath);

            // Header
            writer.WriteLine(string.Join(delimiter, props.Select(p => p.Name)));

            // Rows
            foreach (var item in data)
            {
                var values = props.Select(p => p.GetValue(item)?.ToString());
                writer.WriteLine(string.Join(delimiter, values));
            }
        }

        // ✅ SUMMARY
        public Dictionary<string, object> GetSummary()
        {
            var data = ReadAll();

            var summary = new Dictionary<string, object>();

            summary["TotalRecords"] = data.Count;

            var priceProp = typeof(T).GetProperty("Price");
            var stockProp = typeof(T).GetProperty("Stock");

            if (priceProp != null)
            {
                var prices = data.Select(x => Convert.ToDecimal(priceProp.GetValue(x)));
                summary["TotalPrice"] = prices.Sum();
                summary["AveragePrice"] = prices.Average();
            }

            if (stockProp != null)
            {
                var stocks = data.Select(x => Convert.ToInt32(stockProp.GetValue(x)));
                summary["TotalStock"] = stocks.Sum();
            }

            return summary;
        }

        // ✅ Helper
        private object ConvertValue(string value, Type type)
        {
            if (type == typeof(int)) return int.Parse(value);
            if (type == typeof(decimal)) return decimal.Parse(value);
            if (type == typeof(double)) return double.Parse(value);
            if (type == typeof(bool)) return bool.Parse(value);

            return value;
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Stock { get; set; }
    }

 
}