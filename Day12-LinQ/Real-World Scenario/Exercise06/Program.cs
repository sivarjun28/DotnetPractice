using System;
using System.Collections.Immutable;
namespace Exercise06
{
    public class Sale
    {

        public int SaleId { get; set; }
        public string Product { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public string Salesperson { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }

        public static List<Sale> GenerateSales(int count)
        {
            var products = new[] { "Laptop", "Mouse", "Keyboard", "Monitor", "Phone" };
            var categories = new[] { "Electronics", "Accessories" };
            var salespeople = new[] { "Alice", "Bob", "Charlie", "David", "Eve" };
            var regions = new[] { "North", "South", "East", "West" };

            var random = new Random();

            return Enumerable.Range(1, count).Select(i => new Sale
            {
                SaleId = i,
                Product = products[random.Next(products.Length)],
                Category = categories[random.Next(categories.Length)],
                Amount = (decimal)(random.Next(50, 2000)),   // price per sale
                Quantity = random.Next(1, 10),
                Salesperson = salespeople[random.Next(salespeople.Length)],
                Region = regions[random.Next(regions.Length)],
                SaleDate = DateTime.Now.AddDays(-random.Next(0, 365)) // last 1 year
            }).ToList();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Sale> sales = Sale.GenerateSales(100);

            //Total sales by region

            var totalSales = sales
         .GroupBy(s => s.Region)
         .Select(g => new
         {
             Region = g.Key,
             Total = g.Sum(x => x.Amount * x.Quantity)
         });
            foreach (var item in totalSales)
            {
                System.Console.WriteLine($"{item.Region} : {item.Total}");
            }

            // Top 5 best-selling products
            var top5 = sales.
                        GroupBy(p => p.Product)
                        .Select(g => new
                        {
                            Product = g.Key,
                            TotalQuantity = g.Sum(x => x.Quantity),
                            TotalRevenue = g.Sum(x => x.Amount * x.Quantity)
                        })
                        .OrderByDescending(x => x.TotalQuantity)
                        .Take(5);
            Console.WriteLine("Top 5 Best-Selling Products:");

            foreach (var item in top5)
            {
                Console.WriteLine($"{item.Product} | Quantity Sold: {item.TotalQuantity} | Revenue: ${item.TotalRevenue:F2}");
            }

            //. Average sale amount by category

            var avg = sales.
                        GroupBy(s => s.Category)
                        .Select(g => new
                        {
                            Category = g.Key,
                            AverageSale = g.Average(x => x.Amount)
                        });
            foreach (var item in avg)
            {
                System.Console.WriteLine($"{item.Category} : {item.AverageSale:F2}");
            }

            //4. Salesperson performance (total and count)
            var performance = sales
     .GroupBy(s => s.Salesperson)
     .Select(g => new
     {
         Salesperson = g.Key,
         TotalSales = g.Sum(x => x.Amount * x.Quantity),
         TotalTransactions = g.Count()
     })
     .OrderByDescending(x => x.TotalSales);
            Console.WriteLine("Salesperson Performance:");

            foreach (var item in performance)
            {
                Console.WriteLine($"{item.Salesperson} | Total Sales: ${item.TotalSales:F2} | Transactions: {item.TotalTransactions}");
            }

            //5. Monthly sales trend

            var monthlyTrends = sales
    .GroupBy(s => new { s.SaleDate.Year, s.SaleDate.Month })
    .Select(g => new
    {
        Year = g.Key.Year,
        Month = g.Key.Month,
        TotalSales = g.Sum(x => x.Amount * x.Quantity),
        TotalOrders = g.Count()
    })
    .OrderBy(x => x.Year)
    .ThenBy(x => x.Month);

            Console.WriteLine("Monthly Sales Trends:");

            foreach (var item in monthlyTrends)
            {
                Console.WriteLine($"{item.Year}-{item.Month:D2} | Revenue: ${item.TotalSales:F2} | Orders: {item.TotalOrders}");
            }

            //Products sold in multiple regions

            var sold = sales
     .GroupBy(s => s.Product)
     .Select(g => new
     {
         Product = g.Key,
         Regions = g.Select(x => x.Region).Distinct().Count()
     })
     .Where(x => x.Regions > 1);

            Console.WriteLine("Products sold in multiple regions:");

            foreach (var item in sold)
            {
                Console.WriteLine($"{item.Product} sold in {item.Regions} regions");
            }

            // -------------------
            // 7. Sales above average
            var avgSales = sales.Average(s => s.Amount * s.Quantity);

            var aboveAvgSales = sales
                .Where(s => (s.Amount * s.Quantity) > avgSales);

            Console.WriteLine("\n7. Sales Above Average:");
            Console.WriteLine($"Average Sale Value: ${avgSales:F2}");

            foreach (var s in aboveAvgSales)
            {
                Console.WriteLine($"{s.Product} | ${s.Amount * s.Quantity:F2} | {s.Salesperson}");
            }


            // -------------------
            // 8. Year-over-Year comparison
            var yearlySales = sales
                .GroupBy(s => s.SaleDate.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    Total = g.Sum(x => x.Amount * x.Quantity)
                })
                .OrderBy(x => x.Year);

            Console.WriteLine("\n8. Year-over-Year Sales:");

            foreach (var y in yearlySales)
            {
                Console.WriteLine($"{y.Year}: ${y.Total:F2}");
            }


            // -------------------
            // 9. Category contribution percentage
            var totalRevenue = sales.Sum(s => s.Amount * s.Quantity);

            var categoryContribution = sales
                .GroupBy(s => s.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Revenue = g.Sum(x => x.Amount * x.Quantity),
                    Percentage = (g.Sum(x => x.Amount * x.Quantity) / totalRevenue) * 100
                });

            Console.WriteLine("\n9. Category Contribution (%):");

            foreach (var c in categoryContribution)
            {
                Console.WriteLine($"{c.Category}: ${c.Revenue:F2} ({c.Percentage:F2}%)");
            }


            // -------------------
            // 10. Underperforming products (< $500 total sales)
            var underperforming = sales
                .GroupBy(s => s.Product)
                .Select(g => new
                {
                    Product = g.Key,
                    TotalSales = g.Sum(x => x.Amount * x.Quantity)
                })
                .Where(x => x.TotalSales < 500);

            Console.WriteLine("\n10. Underperforming Products (< $500):");

            foreach (var p in underperforming)
            {
                Console.WriteLine($"{p.Product}: ${p.TotalSales:F2}");
            }


        }

    }
}

