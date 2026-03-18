using System;
using System.Globalization;
namespace Name
{

    public class Transaction
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Region { get; set; } = string.Empty;


        public static List<Transaction> GenerateTransactions(int count)
        {
            var random = new Random();

            var customers = new[] { "Alice", "Bob", "Charlie", "David", "Eva" };
            var products = new[] { "Laptop", "Phone", "Shoes", "Watch", "Bag", "Headphones" };
            var categories = new[] { "Electronics", "Fashion", "Accessories" };
            var regions = new[] { "North", "South", "East", "West" };

            var transactions = new List<Transaction>();

            for (int i = 1; i <= count; i++)
            {
                var transaction = new Transaction
                {
                    Id = i,
                    CustomerName = customers[random.Next(customers.Length)],
                    Product = products[random.Next(products.Length)],
                    Category = categories[random.Next(categories.Length)],
                    Amount = Math.Round((decimal)(random.NextDouble() * 2000), 2), // 0 - 2000
                    Date = DateTime.Now.AddDays(-random.Next(0, 60)), // last 60 days
                    Region = regions[random.Next(regions.Length)]
                };

                transactions.Add(transaction);
            }

            return transactions;
        }
    }
    /*
    Create queries for:
    1. Group by category, show count and total
    2. Group by customer, show number of purchases and total spent
    3. Group by region, find average transaction amount
    4. Group by month, show monthly revenue
    5. Group by category and region (multi-key)
    6. Find categories with more than 10 transactions
    7. Group by customer, find customer who spent most
    8. Group by product, show products sold in each region
    9. Group by date range (weekly)
    10. Nested grouping: region -> category -> count
    */
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Transaction> transactions = Transaction.GenerateTransactions(100);

            //1. Group by category, show count and total
            var byCategory = transactions.
                                GroupBy(t => t.Category)
                                .Select(g => new
                                {
                                    Category = g.Key,
                                    Count = g.Count(),
                                    Total = g.Sum(x => x.Amount)
                                });
            foreach (var item in byCategory)
            {
                System.Console.WriteLine($"{item.Category} : {item.Count} transactions, {item.Total:C}");
            }
            // 2. Group by customer, show number of purchases and total spent
            var byCustomer = transactions
                                .GroupBy(t => t.CustomerName)
                                .Select(g => new
                                {
                                    Customer = g.Key,
                                    PurchaseCount = g.Count(),
                                    TotalSpent = g.Sum(t => t.Amount)

                                });
            foreach (var item in byCustomer)
            {
                System.Console.WriteLine($"{item.Customer} : {item.PurchaseCount} transactions, {item.TotalSpent:C}");
            }

            //3. Group by region, find average transaction amount
            var byRegion = transactions.
                                GroupBy(t => t.Region)
                                .Select(g => new
                                {
                                    Region = g.Key,
                                    Average = g.Average(x => x.Amount)
                                });
            foreach (var item in byRegion)
            {
                System.Console.WriteLine($" {item.Region}: {item.Average:C}");
            }

            //4. Group by month, show monthly revenue
            var byMonth = transactions
                            .GroupBy(t => new { t.Date.Year, t.Date.Month })
                            .Select(g => new
                            {
                                Year = g.Key.Year,
                                Month = g.Key.Month,
                                Revenue = g.Sum(t => t.Amount),
                                TransactionCount = g.Count()
                            });
            foreach (var item in byMonth)
            {
                System.Console.WriteLine($"{item.Year} {item.Month} : {item.Revenue} : {item.TransactionCount} transactions");
            }
            //5. Group by category and region (multi-key)
            var multiGroup = transactions
                            .GroupBy(t => new { t.Category, t.Region })
                            .Select(g => new
                            {
                                Category = g.Key.Category,
                                Region = g.Key.Region,
                                TransactionCount = g.Count(),
                                TotalAmount = g.Sum(t => t.Amount),
                                AverageAmount = g.Average(t => t.Amount)
                            });
            foreach (var item in multiGroup)
            {
                Console.WriteLine($"{item.Category} | {item.Region} | Count: {item.TransactionCount} | Total: {item.TotalAmount:C} | Average: {item.AverageAmount:C}");
            }

            //6. Find categories with more than 10 transactions
            var popularCategories = transactions
                            .GroupBy(t => t.Category)              // Group by category
                            .Where(g => g.Count() > 10)            // Filter groups with more than 10 transactions
                            .Select(g => new
                            {
                                Category = g.Key,
                                TransactionCount = g.Count()
                            });
            foreach (var item in popularCategories)
            {
                Console.WriteLine($"{item.Category} | Transactions: {item.TransactionCount}");
            }

            //7. Group by customer, find customer who spent most

            var spentByCustomer = transactions
                    .GroupBy(t => t.CustomerName)         // Group by customer
                    .Select(g => new
                    {
                        Customer = g.Key,
                        TotalSpent = g.Sum(t => t.Amount) // Sum of all their transactions
                    })
                    .OrderByDescending(x => x.TotalSpent) // Sort by total spent
                    .FirstOrDefault();                    // Take the top spender
            if (spentByCustomer != null)
            {
                Console.WriteLine($"{spentByCustomer.Customer} spent the most: {spentByCustomer.TotalSpent:C}");
            }

            //8. Group by product, show products sold in each region
            var sold = transactions
                            .GroupBy(t => t.Product)
                            .Select(g => new
                            {
                                Product = g.Key,
                                Regions = g.Select(t => t.Region)
                                            .Distinct()
                                            .ToList()
                            });
            foreach (var item in sold)
            {
                Console.WriteLine($"{item.Product} sold in regions: {string.Join(", ", item.Regions)}");
            }

            //9. Group by date range (weekly)


            var weekly = transactions
                .GroupBy(t => new
                {
                    Year = t.Date.Year,
                    Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                        t.Date,
                        CalendarWeekRule.FirstDay,
                        DayOfWeek.Monday)  // Week starts on Monday
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Week = g.Key.Week,
                    TransactionCount = g.Count(),
                    TotalAmount = g.Sum(t => t.Amount)
                });
            foreach (var item in weekly)
            {
                System.Console.WriteLine($"{item.Year} -{item.Week} :{item.TransactionCount:C} transactions, Total: {item.TotalAmount}");
            }
            //10. Nested grouping: region -> category -> count
            var nested = transactions.
                        GroupBy(t => new
                        {
                            t.Region,
                            t.Category
                        }).Select(g => new
                        {
                            Region = g.Key.Region,
                            Category = g.Key.Category,
                            TotalCount = g.Count()
                        })
                        .OrderBy(x => x.Region)
                        .ThenBy(x => x.Category);
            foreach (var item in nested)
            {
                System.Console.WriteLine($"{item.Region} - {item.Category} : {item.TotalCount} transactions");
            }
        }
    }
}
