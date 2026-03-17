using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Generate sample employees
            List<Employee> employees = Employee.GenerateEmployees(50);

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

            List<Student> students = new()
            {
                new Student { Id = 1, Name = "Alice", Age = 20, Major = "Computer Science", GPA = 3.8 },
                new Student { Id = 2, Name = "Bob", Age = 22, Major = "Mathematics", GPA = 3.5 },
                new Student { Id = 3, Name = "Charlie", Age = 21, Major = "Computer Science", GPA = 3.9 },
                new Student { Id = 4, Name = "David", Age = 23, Major = "Physics", GPA = 3.2 },
                new Student { Id = 5, Name = "Eve", Age = 20, Major = "Mathematics", GPA = 3.7 },
                new Student { Id = 6, Name = "Frank", Age = 22, Major = "Computer Science", GPA = 3.4 },
                new Student { Id = 7, Name = "Grace", Age = 21, Major = "Physics", GPA = 3.6 }
            };

            // -------------------
            // 1. CS students with GPA > 3.5
            var csHonorRoll = students
                .Where(s => s.Major == "Computer Science" && s.GPA > 3.5)
                .Select(s => s.Name)
                .OrderBy(name => name);

            Console.WriteLine("Query 1: CS students with GPA > 3.5");
            foreach (var name in csHonorRoll) Console.WriteLine(name);
            Console.WriteLine();

            // -------------------
            // 2. Top 5 in-stock electronics with 15% discount
            var top5Electronics = products
                .Where(p => p.Category == "Electronics" && p.Stock > 0)
                .OrderBy(p => p.Price)
                .Take(5)
                .Select(p => new
                {
                    p.Name,
                    OriginalPrice = p.Price,
                    DiscountedPrice = p.Price * 0.85m
                });

            Console.WriteLine("Query 2: Top 5 in-stock electronics with 15% discount");
            foreach (var item in top5Electronics)
                Console.WriteLine($"Product: {item.Name}, Original Price: ${item.OriginalPrice:F2}, Discounted Price: ${item.DiscountedPrice:F2}");
            Console.WriteLine();

            // -------------------
            // 3. IT employees hired in last 5 years, earning > 50k
            var recentITEmployees = employees
                .Where(e => e.Department == "IT" && e.Salary > 50000 && e.HireDate >= DateTime.Now.AddYears(-5))
                .OrderByDescending(e => e.Salary);

            Console.WriteLine("Query 3: IT employees hired in last 5 years earning > 50k");
            foreach (var e in recentITEmployees)
                Console.WriteLine($"{e.Name}, Department: {e.Department}, Salary: ${e.Salary}, HireDate: {e.HireDate.ToShortDateString()}");
            Console.WriteLine();

            // -------------------
            // 4. Total inventory value by category
            var inventoryValueByCategory = products
                .Where(p => p.Stock > 0)
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalValue = g.Sum(p => p.Price * p.Stock)
                });

            Console.WriteLine("Query 4: Total inventory value by category (in-stock only)");
            foreach (var item in inventoryValueByCategory)
                Console.WriteLine($"{item.Category}: ${item.TotalValue:F2}");
            Console.WriteLine();

            // -------------------
            // 5. Top 3 departments by average salary
            var top3Departments = employees
                .GroupBy(e => e.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    AvgSalary = g.Average(e => e.Salary)
                })
                .OrderByDescending(d => d.AvgSalary)
                .Take(3);

            Console.WriteLine("Query 5: Top 3 departments with highest average salary");
            foreach (var d in top3Departments)
                Console.WriteLine($"{d.Department}: Average Salary = ${d.AvgSalary:F2}");
            Console.WriteLine();

            // -------------------
            // 6. Honor roll students
            var honorRollStudents = students
                .Select(s => new { s.Name, s.Age, IsHonorRoll = s.GPA > 3.5 })
                .Where(s => s.IsHonorRoll);

            Console.WriteLine("Query 6: Honor roll students");
            foreach (var s in honorRollStudents)
                Console.WriteLine($"{s.Name}, Age: {s.Age}, Honor Roll: {s.IsHonorRoll}");
            Console.WriteLine();

            // -------------------
            // 7. Low stock products grouped by supplier
            var lowStockBySupplier = products
                .Where(p => p.Stock < 10)
                .GroupBy(p => p.Supplier)
                .Select(g => new { Supplier = g.Key, Count = g.Count() });

            Console.WriteLine("Query 7: Low stock products grouped by supplier");
            foreach (var item in lowStockBySupplier)
                Console.WriteLine($"{item.Supplier}: {item.Count} product(s) low in stock");
            Console.WriteLine();

            // -------------------
            // 8. Employees in top 25% salary range
            var topSalaryThreshold = employees.OrderByDescending(e => e.Salary)
                                              .Skip((int)(employees.Count * 0.25))
                                              .Min(e => e.Salary);

            var top25PercentEmployees = employees
                .Where(e => e.Salary >= topSalaryThreshold)
                .OrderByDescending(e => e.Salary);

            Console.WriteLine("Query 8: Employees in top 25% salary range");
            foreach (var e in top25PercentEmployees)
                Console.WriteLine($"{e.Name}, Salary: ${e.Salary}");
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Major { get; set; } = string.Empty;
        public double GPA { get; set; }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public int Age { get; set; }

        public static List<Employee> GenerateEmployees(int count)
        {
            var departments = new[] { "IT", "HR", "Sales", "Marketing", "Finance" };
            var random = new Random();

            return Enumerable.Range(1, count).Select(i => new Employee
            {
                Id = i,
                Name = $"Employee{i}",
                Department = departments[random.Next(departments.Length)],
                Salary = random.Next(30000, 120000),
                HireDate = DateTime.Now.AddYears(-random.Next(1, 10)),
                Age = random.Next(22, 65)
            }).ToList();
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