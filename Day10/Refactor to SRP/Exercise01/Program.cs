// See https://aka.ms/new-console-template for more information
/*
Refactor into separate classes:
1. Employee - Pure data model
2. EmployeeValidator - Validation logic
3. EmployeeRepository - Database operations
4. SalaryCalculator - Salary calculations
5. EmailService - Email operations
6. EmployeeReportGenerator - Reporting

Test that all classes work together correctly.
*/


Employee employee1 = new()
{
    Id = 1,
    Name = "sivarjun",
    Email = "arjun@gmail.com",
    Salary = 6787.98m,
    Department = "Software intern"
};
EmployeeValidator validator = new();
if (validator.Validator(employee1))
{
    EmployeeRepository repository = new();
    repository.Save(employee1);
    

    SalaryCalculator salary = new();
    salary.CalculateAnnualSalary(employee1);

    EmailService emailService = new();
    emailService.SendWelcomeMail(employee1);

    EmployeeReportGenerator reportGenerator = new();
    reportGenerator.GenerateReport(employee1);
}
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public string Department { get; set; } = string.Empty;

}

public class EmployeeValidator
{
    public bool Validator(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Name))
            return false;
        if (string.IsNullOrWhiteSpace(employee.Email) || !employee.Email.Contains("@"))
            return false;
        if (employee.Salary <= 0)
            return false;
        return true;

    }
}

public class EmployeeRepository
{
    public void Save(Employee employee)
    {
        System.Console.WriteLine($"Employee {employee.Name} saved to the database");
    }
}

public class SalaryCalculator
{
    public decimal CalculateAnnualSalary(Employee employee)
    {
        return employee.Salary * 12;
    }
}

public class EmailService
{
    public void SendWelcomeMail(Employee employee)
    {
        System.Console.WriteLine($"Sending welcome mail to the {employee.Email}");
    }
}

public class EmployeeReportGenerator
{
    public void GenerateReport(Employee employee)
    {
        Console.WriteLine($"Generating report for {employee.Name}...");
        Console.WriteLine($"Department: {employee.Department}");
        Console.WriteLine($"Salary: {employee.Salary}");
    }
}