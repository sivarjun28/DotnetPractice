using System;
using System.Linq;  // For LINQ methods like Sum

namespace Exercise01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new()
            {
                new PermanentEmployee("Alice", "alice@company.com", 5000, 1000),
                new ContractEmployee("Bob", "bob@contractor.com",5000, 50.00m, 160.0m),
                new Manager("Carol", "carol@company.com", 6000, 1200, 10, 2000)
            };

            // Print all employee details (polymorphism)
            foreach (var employee in employees)
            {
                employee.GetDetails();
                Console.WriteLine();
            }

            // Calculate total payroll
            decimal totalPayroll = employees.Sum(e => e.CalculateSalary());
            Console.WriteLine($"Total Payroll: {totalPayroll:C}");
        }
    }
    
    // Base class Employee
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public decimal BaseSalary { get; set; }  // Use BaseSalary, not Salary

        public Employee(string name, string email, decimal baseSalary)
        {
            Name = name;
            Email = email;
            BaseSalary = baseSalary;
            JoinDate = DateTime.Now;
        }

        public virtual decimal CalculateSalary()
        {
            return BaseSalary;
        }

        public virtual void GetDetails()
        {
            Console.WriteLine($"Employee: {Name}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Salary: {CalculateSalary():C}");
        }
    }

    // Derived class PermanentEmployee
    public class PermanentEmployee : Employee
    {
        public decimal Benefits { get; set; }
        public decimal PensionContribution { get; set; }

        public PermanentEmployee(string name, string email, decimal baseSalary, decimal benefits)
            : base(name, email, baseSalary)
        {
            Benefits = benefits;
        }

        public override decimal CalculateSalary()
        {
            return base.CalculateSalary() + Benefits;
        }

        public override void GetDetails()
        {
            base.GetDetails();
            Console.WriteLine($"Employee Type: Permanent");
            Console.WriteLine($"Benefits: {Benefits:C}");
        }
    }

    // Derived class ContractEmployee
    public class ContractEmployee : Employee
    {
        public decimal HourlyRate { get; set; }
        public decimal HoursWorked { get; set; }

        public ContractEmployee(string name, string email, decimal baseSalary, decimal hourlyRate, decimal hoursWorked)
            : base(name, email, baseSalary)
        {
            HourlyRate = hourlyRate;
            HoursWorked = hoursWorked;
        }

        public override decimal CalculateSalary()
        {
            return HourlyRate * HoursWorked;
        }

        public override void GetDetails()
        {
            base.GetDetails();
            Console.WriteLine($"Employee Type: Contract");
            Console.WriteLine($"Hourly Rate: {HourlyRate:C}");
            Console.WriteLine($"Hours Worked: {HoursWorked}");
        }
    }

    // Derived class Manager (inherits from PermanentEmployee)
    public class Manager : PermanentEmployee
    {
        public int TeamSize { get; set; }
        public decimal Bonus { get; set; }

        public Manager(string name, string email, decimal baseSalary, decimal benefits, int teamSize, decimal bonus)
            : base(name, email, baseSalary, benefits)
        {
            TeamSize = teamSize;
            Bonus = bonus;
        }

        public override decimal CalculateSalary()
        {
            return base.CalculateSalary() + Bonus;
        }

        public override void GetDetails()
        {
            base.GetDetails();
            Console.WriteLine($"Team Size: {TeamSize}");
            Console.WriteLine($"Bonus: {Bonus:C}");
        }
    }
}