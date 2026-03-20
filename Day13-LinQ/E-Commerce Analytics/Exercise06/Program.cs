using System;
using System.Collections.Generic;
using System.Linq;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Segment { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class Review
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Rating { get; set; }
}

public class Program
{
    static void PrintHeader(string title)
    {
        Console.WriteLine("\n=====================================");
        Console.WriteLine(title);
        Console.WriteLine("=====================================");
    }

    public static void Main()
    {
        // =========================
        // Sample Data
        // =========================
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Alice", Segment = "Premium" },
            new Customer { Id = 2, Name = "Bob", Segment = "Regular" },
            new Customer { Id = 3, Name = "Charlie", Segment = "Regular" }
        };

        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Clothing" }
        };

        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", CategoryId = 1, Price = 50000, Stock = 5 },
            new Product { Id = 2, Name = "Phone", CategoryId = 1, Price = 20000, Stock = 50 },
            new Product { Id = 3, Name = "T-Shirt", CategoryId = 2, Price = 500, Stock = 100 },
            new Product { Id = 4, Name = "Shoes", CategoryId = 2, Price = 2000, Stock = 2 }
        };

        var orders = new List<Order>
        {
            new Order { Id = 1, CustomerId = 1, OrderDate = new DateTime(2025, 1, 10) },
            new Order { Id = 2, CustomerId = 2, OrderDate = new DateTime(2025, 2, 15) },
            new Order { Id = 3, CustomerId = 1, OrderDate = new DateTime(2025, 2, 20) }
        };

        var orderItems = new List<OrderItem>
        {
            new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 50000 },
            new OrderItem { Id = 2, OrderId = 1, ProductId = 3, Quantity = 2, UnitPrice = 500 },
            new OrderItem { Id = 3, OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 20000 },
            new OrderItem { Id = 4, OrderId = 3, ProductId = 3, Quantity = 3, UnitPrice = 500 }
        };

        var reviews = new List<Review>
        {
            new Review { Id = 1, ProductId = 1, Rating = 5 },
            new Review { Id = 2, ProductId = 2, Rating = 4 },
            new Review { Id = 3, ProductId = 3, Rating = 3 }
        };

        // =========================
        // Queries
        // =========================

        var revenueByCategory =
            from oi in orderItems
            join p in products on oi.ProductId equals p.Id
            join c in categories on p.CategoryId equals c.Id
            group oi by c.Name into g
            select new
            {
                Category = g.Key,
                TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
            };

        var topProducts =
            (from oi in orderItems
             join p in products on oi.ProductId equals p.Id
             group oi by p.Name into g
             select new
             {
                 Product = g.Key,
                 TotalSold = g.Sum(x => x.Quantity)
             }).OrderByDescending(x => x.TotalSold).Take(10);

        var avgOrderValueBySegment =
            from o in orders
            join c in customers on o.CustomerId equals c.Id
            join oi in orderItems on o.Id equals oi.OrderId
            group new { oi, c } by c.Segment into g
            select new
            {
                Segment = g.Key,
                AvgOrderValue = g.Sum(x => x.oi.Quantity * x.oi.UnitPrice) /
                                g.Select(x => x.oi.OrderId).Distinct().Count()
            };

        var customerLTV =
            from o in orders
            join oi in orderItems on o.Id equals oi.OrderId
            group oi by o.CustomerId into g
            select new
            {
                CustomerId = g.Key,
                LifetimeValue = g.Sum(x => x.Quantity * x.UnitPrice)
            };

        var purchaseFrequency =
            from o in orders
            group o by o.CustomerId into g
            select new
            {
                CustomerId = g.Key,
                OrdersCount = g.Count()
            };

        var customerSegmentation =
            customerLTV.Select(c => new
            {
                c.CustomerId,
                c.LifetimeValue,
                Segment =
                    c.LifetimeValue > 10000 ? "High" :
                    c.LifetimeValue > 5000 ? "Medium" : "Low"
            });

        var productsNeverOrdered =
            from p in products
            join oi in orderItems on p.Id equals oi.ProductId into gj
            from sub in gj.DefaultIfEmpty()
            where sub == null
            select p;

        var lowStockProducts = products.Where(p => p.Stock < 10);

        var categoryPerformance =
            from oi in orderItems
            join p in products on oi.ProductId equals p.Id
            join c in categories on p.CategoryId equals c.Id
            group oi by c.Name into g
            select new
            {
                Category = g.Key,
                TotalSales = g.Sum(x => x.Quantity),
                Revenue = g.Sum(x => x.Quantity * x.UnitPrice)
            };

        var monthlySales =
            from o in orders
            join oi in orderItems on o.Id equals oi.OrderId
            group oi by new { o.OrderDate.Year, o.OrderDate.Month } into g
            select new
            {
                g.Key.Year,
                g.Key.Month,
                Revenue = g.Sum(x => x.Quantity * x.UnitPrice)
            };

        var seasonalPatterns =
            from o in orders
            join oi in orderItems on o.Id equals oi.OrderId
            group oi by o.OrderDate.Month into g
            select new
            {
                Month = g.Key,
                Revenue = g.Sum(x => x.Quantity * x.UnitPrice)
            };

        var avgRating =
            from r in reviews
            group r by r.ProductId into g
            select new
            {
                ProductId = g.Key,
                AvgRating = g.Average(x => x.Rating)
            };

        var noReviews =
            from p in products
            join r in reviews on p.Id equals r.ProductId into gj
            from sub in gj.DefaultIfEmpty()
            where sub == null
            select p;

        var reviewSales =
            from oi in orderItems
            join r in reviews on oi.ProductId equals r.ProductId
            group new { oi, r } by oi.ProductId into g
            select new
            {
                ProductId = g.Key,
                TotalSales = g.Sum(x => x.oi.Quantity),
                AvgRating = g.Average(x => x.r.Rating)
            };

        // =========================
        // Printing Output
        // =========================

        PrintHeader("Revenue by Category");
        foreach (var x in revenueByCategory)
            Console.WriteLine($"{x.Category,-15} | {x.TotalRevenue,10:C}");

        PrintHeader("Top Products");
        foreach (var x in topProducts)
            Console.WriteLine($"{x.Product,-20} | {x.TotalSold,5}");

        PrintHeader("Avg Order Value by Segment");
        foreach (var x in avgOrderValueBySegment)
            Console.WriteLine($"{x.Segment,-10} | {x.AvgOrderValue,10:C}");

        PrintHeader("Customer LTV");
        foreach (var x in customerLTV)
            Console.WriteLine($"Customer {x.CustomerId} | {x.LifetimeValue:C}");

        PrintHeader("Purchase Frequency");
        foreach (var x in purchaseFrequency)
            Console.WriteLine($"Customer {x.CustomerId} | Orders: {x.OrdersCount}");

        PrintHeader("Customer Segmentation");
        foreach (var x in customerSegmentation)
            Console.WriteLine($"Customer {x.CustomerId} | {x.LifetimeValue:C} | {x.Segment}");

        PrintHeader("Products Never Ordered");
        foreach (var p in productsNeverOrdered)
            Console.WriteLine($"{p.Name}");

        PrintHeader("Low Stock");
        foreach (var p in lowStockProducts)
            Console.WriteLine($"{p.Name} | Stock: {p.Stock}");

        PrintHeader("Category Performance");
        foreach (var x in categoryPerformance)
            Console.WriteLine($"{x.Category} | Sales: {x.TotalSales} | Revenue: {x.Revenue:C}");

        PrintHeader("Monthly Sales");
        foreach (var x in monthlySales)
            Console.WriteLine($"{x.Year}-{x.Month} | {x.Revenue:C}");

        PrintHeader("Seasonal Patterns");
        foreach (var x in seasonalPatterns)
            Console.WriteLine($"Month {x.Month} | {x.Revenue:C}");

        PrintHeader("Average Ratings");
        foreach (var x in avgRating)
            Console.WriteLine($"Product {x.ProductId} | {x.AvgRating:F2}");

        PrintHeader("Products with No Reviews");
        foreach (var p in noReviews)
            Console.WriteLine(p.Name);

        PrintHeader("Review vs Sales");
        foreach (var x in reviewSales)
            Console.WriteLine($"Product {x.ProductId} | Sales: {x.TotalSales} | Rating: {x.AvgRating:F2}");
    }
}