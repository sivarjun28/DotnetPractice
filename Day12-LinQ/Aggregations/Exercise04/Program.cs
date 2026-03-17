// See https://aka.ms/new-console-template for more information
using System;
namespace Exercise04
{

    /*
Calculate:
1. Total number of orders
2. Total revenue (all orders)
3. Average order value
4. Highest order value
5. Lowest order value
6. Number of completed orders
7. Revenue from completed orders only
8. Average order value per customer
9. Total orders per status
*/
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Order> orders = new()
{
    new Order { OrderId = 1, Customer = "Alice", Total = 150.00m, OrderDate = DateTime.Now.AddDays(-30), Status = "Completed" },
    new Order { OrderId = 2, Customer = "Bob", Total = 200.00m, OrderDate = DateTime.Now.AddDays(-25), Status = "Completed" },
    new Order { OrderId = 3, Customer = "Alice", Total = 75.50m, OrderDate = DateTime.Now.AddDays(-20), Status = "Completed" },
    new Order { OrderId = 4, Customer = "Charlie", Total = 300.00m, OrderDate = DateTime.Now.AddDays(-15), Status = "Pending" },
    new Order { OrderId = 5, Customer = "Bob", Total = 125.00m, OrderDate = DateTime.Now.AddDays(-10), Status = "Completed" },
    new Order { OrderId = 6, Customer = "Alice", Total = 180.00m, OrderDate = DateTime.Now.AddDays(-5), Status = "Cancelled" },
    new Order { OrderId = 7, Customer = "David", Total = 250.00m, OrderDate = DateTime.Now.AddDays(-3), Status = "Completed" }
};

            //Total orders
            int totalOrders = orders.Count();
            System.Console.WriteLine($"Total Orders: {totalOrders}");

            // TODO: Total revenue
            decimal totalRevenue = orders.Sum(o => o.Total);
            System.Console.WriteLine($"Total revenue: {totalRevenue:C}");

            //Average Order
            double avgOrder = orders.Average(o => (double)o.Total);
            System.Console.WriteLine($"Average orders: {avgOrder:C}");

            // TODO: Max and Min
            decimal maxOrder = orders.Max(o => o.Total);
            decimal minOrder = orders.Min(o => o.Total);
            System.Console.WriteLine($"Min and max orders are Min: {minOrder} & Max: {maxOrder}");

            // TODO: Completed orders
            int completedCount = orders.Count(o => o.Status == "Completed");
            System.Console.WriteLine($"completed Count: {completedCount}");
            decimal completedRevenue = orders
                .Where(o => o.Status == "Completed")
                .Sum(o => o.Total);
            System.Console.WriteLine($"completed Revenue: {completedRevenue}");

            //Average per customer
            double avgPerCustomer = orders
                                    .GroupBy(o => o.Customer)
                                    .Select(g => g.Average(o => (double)o.Total))
                                    .Average();
            System.Console.WriteLine($"Average per Customer: {avgPerCustomer}");

            //Orders per status
            var ordersPerStatus = orders
    .GroupBy(o => o.Status)
    .Select(g => new
    {
        Status = g.Key,
        Count = g.Count()
    });


        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string Customer { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }


}
