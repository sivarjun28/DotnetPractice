// See https://aka.ms/new-console-template for more information
using System;
namespace Exercise03
{
    /*
Implement:
1. Order by salary (descending)
2. Order by department, then by name
3. Top 10 highest paid employees
4. Bottom 5 lowest paid employees
5. Pagination: Get page 3 (page size 10)
6. Skip first 20, take next 10
7. Employees ordered by hire date (oldest first)
8. IT department ordered by salary
*/
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = Employee.GenerateEmployees(30);
            //Query 1 - By salary
            System.Console.WriteLine("By Salary ");
            var bySalary = employees.OrderByDescending(e => e.Salary);
            foreach (var item in bySalary)
            {
                System.Console.WriteLine($"{item.Name} : {item.Salary}");
            }
            //Query 2 - By department then name
            System.Console.WriteLine("By department then name");
            var byDeptName = employees.
                            OrderBy(e => e.Department)
                            .ThenBy(e => e.Name);
            foreach (var item in byDeptName)
            {
                System.Console.WriteLine($"{item.Name} : {item.Department}");
            }

            //Query 3 - Top 10 highest paid

            System.Console.WriteLine("Top 10 highest paid");
            var top10 = employees.
                        OrderByDescending(e => e.Salary)
                        .Take(10);
            foreach (var item in top10)
            {
                System.Console.WriteLine($"{item.Name} : {item.Salary}");
            }

            // Query 4 - Bottom 5 lowest paid
            System.Console.WriteLine("Bottom 5 lowest paid");
            var bottom5 = employees.
                            OrderBy(e => e.Salary)
                            .Take(5);
            foreach (var item in bottom5)
            {
                System.Console.WriteLine($"{item.Name} : {item.Salary}");
            }

            // TODO: Query 5 - Page 3
            int pageSize = 10;
            int pageNumber = 3;
            var page3 = employees
                .OrderBy(e => e.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
            Console.WriteLine($"Employees - Page {pageNumber}:");
            foreach (var e in page3)
            {
                Console.WriteLine($"ID: {e.Id}, Name: {e.Name}");
            }

            // TODO: Query 6 - Skip 20, take 10 
            System.Console.WriteLine("Skip 20, take 10 ");
            var skipAndTake = employees.
                                OrderBy(e => e.Id)
                                .Skip(20)
                                .Take(10);
             foreach (var e in skipAndTake)
            {
                Console.WriteLine($"ID: {e.Id}, Name: {e.Name}");
            }

            // TODO: Query 7 - By hire date
            System.Console.WriteLine("By hire date");
            var byHireDate = employees.
                                OrderByDescending(e => e.HireDate);
            foreach(var e in byHireDate)
            {
                System.Console.WriteLine($"{e.Name} : {e.HireDate}");
            }
            // TODO: Query 8 - IT by salary
            System.Console.WriteLine("IT by salary");
            var ItbySalary = employees.
                            Where(e => e.Department == "IT")
                            .OrderByDescending(e => e.Salary);
            foreach (var item in ItbySalary)
            {
                System.Console.WriteLine($"{item.Name} : {item.Department} : {item.Salary}");
            }
        }
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
            var departments = new[] { "IT", "Sales", "Marketing", "Finance" };

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


}
