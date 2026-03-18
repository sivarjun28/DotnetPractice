using System;
using System.Text.RegularExpressions;
namespace AdvanceLinQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Group By Category

            List<Product> products = new()
{
    new Product { Name = "Laptop", Category = "Electronics", Price = 999m },
    new Product { Name = "Mouse", Category = "Electronics", Price = 25m },
    new Product { Name = "Desk", Category = "Furniture", Price = 300m },
    new Product { Name = "Chair", Category = "Furniture", Price = 200m },
    new Product { Name = "Monitor", Category = "Electronics", Price = 350m }
};
            //Group By Category
            var byCategory = products.GroupBy(p => p.Category);

            foreach (var group in byCategory)
            {
                System.Console.WriteLine($"\n{group.Key}");
                foreach (var product in group)
                {
                    System.Console.WriteLine($"{product.Name} : {product.Price}");
                }
            }

            // Group and calculate statistics
            var categoryStats = products
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count(),
                    TotalValue = g.Sum(p => p.Price),
                    AveragePrice = g.Average(p => p.Price),
                    MaxPrice = g.Max(p => p.Price),
                    MinPrice = g.Min(p => p.Price)
                });

            foreach (var stat in categoryStats)
            {
                Console.WriteLine($"{stat.Category}:");
                Console.WriteLine($"  Count: {stat.Count}");
                Console.WriteLine($"  Total: {stat.TotalValue:C}");
                Console.WriteLine($"  Average: {stat.AveragePrice:C}");
            }
            //Query syntax 
            var categoryStats2 = from p in products
                                 group p by p.Category into g
                                 select new
                                 {
                                     Category = g.Key,
                                     Count = g.Count(),
                                     Total = g.Sum(p => p.Price)
                                 };

            System.Console.WriteLine("------------------------------------------------------------");
            System.Console.WriteLine("Multi grouping ");

            List<Sale> sales = Sale.GetSales();

            //group By multiple keys
            var grouped = sales.GroupBy(s => new { s.Region, s.Category });
            foreach (var group in grouped)
            {
                Console.WriteLine($"{group.Key.Region} - {group.Key.Category}: " +
                                 $"{group.Sum(s => s.Amount):C}");
            }

            // Or with tuple
            var grouped2 = sales.GroupBy(s => (s.Region, s.Category));


            System.Console.WriteLine("------------------------------------------------------------");
            System.Console.WriteLine("Join operations ");
            List<Customer> customers = new()
{
    new Customer { Id = 1, Name = "Alice" },
    new Customer { Id = 2, Name = "Bob" },
    new Customer { Id = 3, Name = "Charlie" }
};

            List<Order> orders = new()
{
    new Order { Id = 1, CustomerId = 1, Total = 100m },
    new Order { Id = 2, CustomerId = 1, Total = 200m },
    new Order { Id = 3, CustomerId = 2, Total = 150m }
    // Note: No orders for Charlie (Id = 3)
};

            // INNER JOIN - Method syntax

            var customerOrders1 = customers.Join(
                orders,
                c => c.Id,
                o => o.CustomerId,
                (c, o) => new
                {
                    CustomerName = c.Name,
                    OrderId = o.Id,
                    Total = o.Total
                }

            );
            // INNER JOIN - Query syntax
            var customerOrders2 = from c in customers
                                  join o in orders on c.Id equals o.CustomerId
                                  select new
                                  {
                                      CustomerName = c.Name,
                                      OrderId = o.Id,
                                      Total = o.Total
                                  };

            foreach (var item in customerOrders2)
            {
                System.Console.WriteLine($"{item.CustomerName} || {item.OrderId} || {item.Total} ");
            }


            System.Console.WriteLine("----------------------------------");
            System.Console.WriteLine("Left Join (Group Join)");
            //// LEFT JOIN - Include all customers, even without orders
            var allCustomers = from c in customers
                               join o in orders on c.Id equals o.CustomerId into customerOrders
                               select new
                               {
                                   Customer = c.Name,
                                   OrderCount = customerOrders.Count(),
                                   TotalSpent = customerOrders.Sum(s => s.Total)
                               };
            var allCustomers1 = customers
                                .GroupJoin(orders,
                                c => c.Id,
                                o => o.CustomerId,
                                (c, orderGroup) => new
                                {
                                    Customer = c.Name,
                                    OrderCount = orderGroup.Count(),
                                    TotalSpent = orderGroup.Sum(o => o.Total)
                                }
                                );
            foreach (var c in allCustomers1)
            {
                Console.WriteLine($"{c.Customer}: {c.OrderCount} orders, {c.TotalSpent:C}");
            }

            // Traditional LEFT JOIN pattern
            var lefJoin = from c in customers
                          join o in orders on c.Id equals o.CustomerId into orderGroup2
                          from o in orderGroup2.DefaultIfEmpty()
                          select new
                          {
                              Customer = c.Name,
                              OrderId = o?.Id ?? 0,
                              Total = o?.Total ?? 0
                          };
            foreach (var c in lefJoin)
            {
                Console.WriteLine($"{c.Customer}: {c.OrderId} orders, {c.Total:C}");
            }

            System.Console.WriteLine("----------------------------------------------");
            System.Console.WriteLine(": SelectMany (Flattening) ");
            List<Department> departments = new()
            {
                new Department
                {
                    Name = "IT",
                    Employees = new()
                    {
                        new Employee {Name = "Arjun", Salary = 60000},
                        new Employee {Name = "Siva", Salary = 30000}
                    }
                },
                 new Department
                {
                    Name = "HR",
                    Employees = new()
                    {
                        new Employee {Name = "Sushma", Salary = 40000},
                        new Employee {Name = "Shiva", Salary = 30000}
                    }
                }

            };

            // SELECT many employees from all departments
            var allEmployees = departments.SelectMany(d => d.Employees);
            foreach (var item in allEmployees)
            {
                System.Console.WriteLine($"{item.Name} : {item.Salary}");
            }

            //With department information
            var employeeInfo = departments.SelectMany(
               d => d.Employees,
               (dept, emp) =>
               new
               {
                   Department = dept.Name,
                   Employee = emp.Name,
                   emp.Salary
               }
            );
            //Query Syntax
            var employeeInfo2 = from d in departments
                                from e in d.Employees
                                select new
                                {
                                    Department = d.Name,
                                    Employee = e.Name,
                                    e.Salary
                                };
            foreach (var item in employeeInfo)
            {
                System.Console.WriteLine($"{item.Department}- {item.Employee} - {item.Salary}");
            }
            System.Console.WriteLine("-------------------------------");
            System.Console.WriteLine("Cartesian Product");
            List<string> colors = new() { "Red", "Blue", "Green" };
            List<string> sizes = new() { "Small", "Medium", "Large" };

            var combinations = colors.SelectMany(
                color => sizes,
                (color, size) => $"{color} {size}"
            );

            var combinations1 = from c in colors
                                from s in sizes
                                select $"{c} {s}";
            foreach (var item in combinations1)
            {
                System.Console.WriteLine($"{item}");
            }

            System.Console.WriteLine("--------------------------------------------");
            System.Console.WriteLine(" Set Operations");
            List<int> list1 = new() { 1, 2, 3, 4, 5 };
            List<int> list2 = new() { 4, 5, 6, 7, 8 };

            // UNION - All unique elements from both
            var union = list1.Union(list2);
            // Result: 1, 2, 3, 4, 5, 6, 7, 8
            foreach (var item in union)
            {
                System.Console.WriteLine(item);
            }

            // INTERSECT - Common elements
            var intersect = list1.Intersect(list2);
            // Result: 4, 5

            // EXCEPT - Elements in first but not second
            var except = list1.Except(list2);
            // Result: 1, 2, 3

            // Concat vs Union
            var concat = list1.Concat(list2);
            // Result: 1, 2, 3, 4, 5, 4, 5, 6, 7, 8 (duplicates included)
            // With custom objects (need custom comparer or use DistinctBy)
            // List<Product> store1Products = GetStore1Products();
            // List<Product> store2Products = GetStore2Products();

            // var allProducts = store1Products
            //     .Union(store2Products, new ProductComparer());

            //  Or in C# 10+
            // var allProducts2 = store1Products
            //     .UnionBy(store2Products, p => p.Id);
            System.Console.WriteLine("---------------------------------------");
            System.Console.WriteLine("Complex Patterns");

            System.Console.WriteLine("Above Average");
            decimal avgAmount = sales.Average(s => s.Amount);
            var aboveAvg = sales.Where(s => s.Amount > avgAmount);

            // In one query
            var expensive = sales.Where(s => s.Amount > sales.Average(x => x.Amount));
            foreach (var item in expensive)
            {
                System.Console.WriteLine(item.Amount);
            }
            // Customers who have placed orders
            var customersWithOrders = customers.Where(c =>
                orders.Any(o => o.CustomerId == c.Id));
            // Customers without orders
            var customersWithoutOrders = customers.Where(c =>
                !orders.Any(o => o.CustomerId == c.Id));
            foreach (var item in customersWithOrders)
            {
                System.Console.WriteLine(item.Name);
            }
            foreach (var item in customersWithoutOrders)
            {
                System.Console.WriteLine(item.Name);
            }

            // Query syntax with intermediate variable
            var results = from p in products
                          let discount = p.Price * 0.1m
                          let finalPrice = p.Price - discount
                          where finalPrice < 100
                          select new
                          {
                              p.Name,
                              OriginalPrice = p.Price,
                              Discount = discount,
                              FinalPrice = finalPrice
                          };

            // Avoid recalculating

            // Continue query after GroupBy
            var categoryReport = from p in products
                                 group p by p.Category into categoryGroup
                                 where categoryGroup.Count() > 2
                                 select new
                                 {
                                     Category = categoryGroup.Key,
                                     Count = categoryGroup.Count(),
                                     Average = categoryGroup.Average(p => p.Price)
                                 };

            // Into with Join
            var result = from c in customers
                         join o in orders on c.Id equals o.CustomerId into customerOrders
                         where customerOrders.Any()
                         select new
                         {
                             c.Name,
                             OrderCount = customerOrders.Count()
                         };

            // Pattern 1: Group and aggregate

            var summary = products
                .GroupBy(x => x.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Price) });

            // Pattern 2: Filter groups
            var largeGroups = products
                .GroupBy(x => x.Category)
                .Where(g => g.Count() > 5)
                .Select(g => g.Key);

            // Pattern 3: Nested grouping
            var nested = products
                .GroupBy(x => x.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    // Subcategories = g.GroupBy(x => x.Subcategory)
                });

            // Pattern 4: Join and group
            var report = from c in customers
                         join o in orders on c.Id equals o.CustomerId
                         group o by c.Name into customerOrders
                         select new
                         {
                             Customer = customerOrders.Key,
                             Total = customerOrders.Sum(o => o.Total)
                         };

        }
    }

    public class Product
    {
        public string Name { get; set; } = " ";
        public string Category { get; set; } = " ";
        public decimal Price { get; set; }

    }

    public class Sale
    {
        public string Region { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }

        public static List<Sale> GetSales()
        {
            return new List<Sale>
    {
        new Sale { Region = "North", Category = "Electronics", Amount = 1000 },
        new Sale { Region = "North", Category = "Clothing", Amount = 500 },
        new Sale { Region = "South", Category = "Electronics", Amount = 1500 },
        new Sale { Region = "South", Category = "Clothing", Amount = 700 },
        new Sale { Region = "East", Category = "Electronics", Amount = 1200 },
        new Sale { Region = "West", Category = "Clothing", Amount = 900 },
        new Sale { Region = "North", Category = "Electronics", Amount = 800 }
    };
        }




    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
    }


    public class Department
    {
        public string Name { get; set; } = string.Empty;
        public List<Employee> Employees { get; set; } = new();
    }


    public class Employee
    {
        public string Name { get; set; } = " ";
        public double Salary { get; set; }

    }

}

