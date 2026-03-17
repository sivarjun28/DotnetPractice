// See https://aka.ms/new-console-template for more information
using System;
namespace Exercise01
{

    /*
Create queries for:
1. Electronics products under $100
2. Out of stock products
3. Products with low stock (< 10)
4. All product names and prices as anonymous type
5. Products with 10% discount applied
6. Product summary: Name, Category, InStock (bool)
7. Expensive products (> $200) from TechCorp
8. Products grouped by category, show count and total value
*/
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new()
{
    new Product { Id = 1, Name = "Laptop", Price = 999.99m, Category = "Electronics", Stock = 5, Supplier = "TechCorp" },
    new Product { Id = 2, Name = "Mouse", Price = 25.50m, Category = "Electronics", Stock = 50, Supplier = "TechCorp" },
    new Product { Id = 3, Name = "Desk", Price = 299.99m, Category = "Furniture", Stock = 10, Supplier = "OfficePlus" },
    new Product { Id = 4, Name = "Chair", Price = 199.99m, Category = "Furniture", Stock = 15, Supplier = "OfficePlus" },
    new Product { Id = 5, Name = "Monitor", Price = 349.99m, Category = "Electronics", Stock = 0, Supplier = "TechCorp" },
    new Product { Id = 6, Name = "Keyboard", Price = 79.99m, Category = "Electronics", Stock = 30, Supplier = "TechCorp" },
    new Product { Id = 7, Name = "Bookshelf", Price = 149.99m, Category = "Furniture", Stock = 8, Supplier = "OfficePlus" },
    new Product { Id = 8, Name = "Webcam", Price = 89.99m, Category = "Electronics", Stock = 20, Supplier = "TechCorp" }
};
            //Query 1 - Cheap electronics
            var cheapProducts = products.
                                Where(p => p.Category == "Electronics" && p.Price < 100);
            System.Console.WriteLine("Cheap electronics: ");
            foreach (var item in cheapProducts)
            {
                System.Console.WriteLine($"{item.Name} :  {item.Price}");
            }

            //Query 2 - Out of stock
            System.Console.WriteLine("Out of stock products ");
            products
            .Where(p => p.Stock <= 0)
            .ToList()
            .ForEach(p => System.Console.WriteLine(p.Name));
            //Query 3 - Low stock
            System.Console.WriteLine("Low stock products");
            products
            .Where(p => p.Stock <= 10)
            .ToList()
            .ForEach(p => System.Console.WriteLine(p.Name));

            // Query 4 - Name and price projection
            var priceList = products.Select(p =>
            new
            {
                p.Name,
                p.Price
            });
            System.Console.WriteLine("Name and price projection");
            foreach (var item in priceList)
            {
                System.Console.WriteLine(item);
            }

            //Query 5 - With discount
            var discounted = products.Select(p => new
            {
                p.Name,
                OrginalPrice = p.Price,
                DiscountedPrice = p.Price * 0.9m,
                Savings = p.Price * 0.1m
            });
            foreach (var item in discounted)
            {
                System.Console.WriteLine(item);
            }
            // TODO: Query 6 - Product summary
            var productSummary = products.Select(p => new
            {
                p.Name,
                p.Stock,
                OrginalPrice = p.Price,
                DiscountedPrice = p.Price * 0.9m,
                Savings = p.Price * 0.1m,
                status = p.Stock <= 0 ? "out of stock" : p.Stock <= 10 ? "Low stock" : "in stock"


            });
            System.Console.WriteLine("Product Summary:");
            foreach (var p in productSummary)
            {
                Console.WriteLine(p);
            }

            // TODO: Query 7 - Expensive TechCorp products
            var expensiveTechcorp = products.
                                    Where(p => p.Price > 100 && p.Supplier == "TechCorp");
            System.Console.WriteLine("Expensive TechCorp products");
            foreach (var item in expensiveTechcorp)
            {
                System.Console.WriteLine($"{item.Name} : {item.Price}");
            }
            // TODO: Query 8 - Category analysis
            var categoryAnalysis = products
                                    .GroupBy(p => p.Category)
                                    .Select(g => new
                                    {
                                        Category = g.Key,
                                        ProductCount = g.Count(),
                                        TotalStock = g.Sum(p => p.Stock),
                                        AveragePrice = g.Average(p => p.Price),
                                        MostExpensive = g.OrderByDescending(p => p.Price).First().Name,
                                        Cheapest = g.OrderBy(p => p.Price).First().Name
                                    });
            System.Console.WriteLine("Category analysis");
            foreach (var c in categoryAnalysis)
            {
                Console.WriteLine($"Category: {c.Category}");
                Console.WriteLine($"  Products: {c.ProductCount}");
                Console.WriteLine($"  Total Stock: {c.TotalStock}");
                Console.WriteLine($"  Average Price: {c.AveragePrice:C}");
                Console.WriteLine($"  Most Expensive Product: {c.MostExpensive}");
                Console.WriteLine($"  Cheapest Product: {c.Cheapest}");
                Console.WriteLine();
            }


        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string Supplier { get; set; } = string.Empty;
    }
}
