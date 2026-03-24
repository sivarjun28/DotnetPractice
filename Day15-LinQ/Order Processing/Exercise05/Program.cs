using System;
namespace Name
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public List<OrderItem> Items { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public decimal ShippingCost { get; set; }


        public static (List<Order>, List<Customer>) GenerateOrderData()
        {
            var customers = Enumerable.Range(1, 100)
                .Select(i => new Customer
                {
                    Id = $"CUST{i:D3}",
                    Name = $"Customer {i}",
                    Email = $"customer{i}@example.com",
                    Tier = i % 10 == 0 ? "Gold" : i % 5 == 0 ? "Silver" : "Bronze"
                })
                .ToList();

            var random = new Random(42);
            var orders = new List<Order>();

            for (int i = 1; i <= 10000; i++)
            {
                var itemCount = random.Next(1, 6);
                var items = Enumerable.Range(1, itemCount)
                    .Select(j => new OrderItem
                    {
                        ProductId = random.Next(1, 1001),
                        ProductName = $"Product {random.Next(1, 1001)}",
                        Quantity = random.Next(1, 10),
                        UnitPrice = (decimal)(random.NextDouble() * 200)
                    })
                    .ToList();

                orders.Add(new Order
                {
                    Id = i,
                    OrderDate = DateTime.Now.AddDays(-random.Next(365)),
                    CustomerId = customers[random.Next(customers.Count)].Id,
                    Items = items,
                    Status = random.Next(10) < 8 ? "Completed" : "Pending",
                    ShippingCost = (decimal)(random.NextDouble() * 20)
                });
            }

            return (orders, customers);
        }
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class Customer
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tier { get; set; } = string.Empty; // "Gold", "Silver", "Bronze"
    }

    public class OrderProcessor
    {
        private readonly List<Order> orders;
        private readonly List<Customer> customers;

        public OrderProcessor(List<Order> orders, List<Customer> customers)
        {
            this.orders = orders;
            this.customers = customers;
        }

        // 1. Get revenue by month
        public Dictionary<string, decimal> GetMonthlyRevenue(int year)
        {
            return orders
                .Where(o => o.OrderDate.Year == year)
                .GroupBy(o => o.OrderDate.ToString("yyyy-MM"))
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(o => o.Items.Sum(i => i.UnitPrice * i.Quantity) + o.ShippingCost)
                );
        }

        // 2. Find top customers by total spend
        public List<CustomerSpending> GetTopCustomers(int count)
        {
            var customerLookup = customers.ToDictionary(c => c.Id);

            return orders
                .GroupBy(o => o.CustomerId)
                .Select(g => new CustomerSpending
                {
                    CustomerId = g.Key,
                    CustomerName = customerLookup.ContainsKey(g.Key) ? customerLookup[g.Key].Name : "Unknown",
                    TotalSpent = g.Sum(o => o.Items.Sum(i => i.UnitPrice * i.Quantity) + o.ShippingCost),
                    OrderCount = g.Count()
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(count)
                .ToList();
        }

        // 3. Get product sales summary
        public List<ProductSales> GetProductSalesSummary()
        {
            return orders
                .SelectMany(o => o.Items)
                .GroupBy(i => new { i.ProductId, i.ProductName })
                .Select(g => new ProductSales
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(i => i.Quantity),
                    Revenue = g.Sum(i => i.Quantity * i.UnitPrice),
                    OrderCount = g.Count()
                })
                .OrderByDescending(p => p.Revenue)
                .ToList();
        }

        // 4. Find orders needing attention
        public List<Order> GetProblematicOrders()
        {
            var goldCustomerIds = customers.Where(c => c.Tier == "Gold").Select(c => c.Id).ToHashSet();

            return orders
                .Where(o =>
                    o.Status == "Pending" &&
                    (DateTime.Now - o.OrderDate).TotalDays > 7 &&
                    (o.Items.Sum(i => i.UnitPrice * i.Quantity) + o.ShippingCost) > 1000 &&
                    goldCustomerIds.Contains(o.CustomerId)
                )
                .ToList();
        }

        // 5. Calculate customer lifetime value
        public Dictionary<string, CustomerLifetimeValue> GetCustomerLTV()
        {
            return orders
                .GroupBy(o => o.CustomerId)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        var totalRevenue = g.Sum(o => o.Items.Sum(i => i.UnitPrice * i.Quantity) + o.ShippingCost);
                        var totalOrders = g.Count();
                        return new CustomerLifetimeValue
                        {
                            CustomerId = g.Key,
                            TotalOrders = totalOrders,
                            TotalRevenue = totalRevenue,
                            AverageOrderValue = totalOrders == 0 ? 0 : totalRevenue / totalOrders,
                            LastOrderDate = g.Max(o => o.OrderDate)
                        };
                    }
                );
        }
        public class CustomerSpending
        {
            public string CustomerId { get; set; } = string.Empty;
            public string CustomerName { get; set; } = string.Empty;
            public decimal TotalSpent { get; set; }
            public int OrderCount { get; set; }
        }

        public class ProductSales
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public int QuantitySold { get; set; }
            public decimal Revenue { get; set; }
            public int OrderCount { get; set; }
        }

        public class CustomerLifetimeValue
        {
            public string CustomerId { get; set; } = string.Empty;
            public int TotalOrders { get; set; }
            public decimal TotalRevenue { get; set; }
            public decimal AverageOrderValue { get; set; }
            public DateTime LastOrderDate { get; set; }
        }
    }


    class Program
    {
        static void Main()
        {
            // Generate test data
            var (orders, customers) = Order.GenerateOrderData();

            // Initialize OrderProcessor
            var processor = new OrderProcessor(orders, customers);

            // 1. Monthly revenue for current year
            var revenue = processor.GetMonthlyRevenue(DateTime.Now.Year);
            Console.WriteLine("Monthly Revenue:");
            foreach (var kv in revenue)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value:C}");
            }
            Console.WriteLine();

            // 2. Top 5 customers by spending
            var topCustomers = processor.GetTopCustomers(5);
            Console.WriteLine("Top 5 Customers:");
            foreach (var c in topCustomers)
            {
                Console.WriteLine($"{c.CustomerName} ({c.CustomerId}) - Total Spent: {c.TotalSpent:C}, Orders: {c.OrderCount}");
            }
            Console.WriteLine();

            // 3. Product sales summary (top 10 by revenue)
            var productSales = processor.GetProductSalesSummary();
            Console.WriteLine("Top 10 Product Sales:");
            foreach (var p in productSales.Take(10))
            {
                Console.WriteLine($"{p.ProductName} ({p.ProductId}) - Qty: {p.QuantitySold}, Revenue: {p.Revenue:C}, Orders: {p.OrderCount}");
            }
            Console.WriteLine();

            // 4. Problematic orders
            var problematicOrders = processor.GetProblematicOrders();
            Console.WriteLine($"Problematic Orders: {problematicOrders.Count}");
            foreach (var o in problematicOrders.Take(10))
            {
                Console.WriteLine($"Order {o.Id} - Customer: {o.CustomerId}, Total: {o.Items.Sum(i => i.Quantity * i.UnitPrice) + o.ShippingCost:C}, Date: {o.OrderDate:d}");
            }
            Console.WriteLine();

            // 5. Customer Lifetime Value for first 5 customers
            var ltv = processor.GetCustomerLTV();
            Console.WriteLine("Customer Lifetime Value (first 5 customers):");
            foreach (var kv in ltv.Take(5))
            {
                var c = kv.Value;
                Console.WriteLine($"{c.CustomerId} - Orders: {c.TotalOrders}, Total Revenue: {c.TotalRevenue:C}, Avg Order: {c.AverageOrderValue:C}, Last Order: {c.LastOrderDate:d}");
            }
        }

        // Copy the GenerateOrderData method here from your test data
    }
}
