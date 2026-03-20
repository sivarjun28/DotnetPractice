using System;
namespace Exercise05
{

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }
        public int ManagerId { get; set; }

        public static List<Employee> GetEmployees()
        {
            return new List<Employee>()
            {
                new Employee { Id = 1, Name = "Alice", DepartmentId = 1, Salary = 70000m, ManagerId = 0 },
                new Employee { Id = 2, Name = "Bob", DepartmentId = 1, Salary = 65000m, ManagerId = 1 },
                new Employee { Id = 3, Name = "Charlie", DepartmentId = 2, Salary = 60000m, ManagerId = 1 },
                new Employee { Id = 4, Name = "Diana", DepartmentId = 2, Salary = 62000m, ManagerId = 3 },
                new Employee { Id = 5, Name = "Eve", DepartmentId = 3, Salary = 58000m, ManagerId = 3 },
            };
        }
    }

    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Budget { get; set; }

        public static List<Department> GetDepartments()
        {
            return new List<Department>()
            {
                new Department { Id = 1, Name = "IT", Budget = 500000m },
                new Department { Id = 2, Name = "HR", Budget = 200000m },
                new Department { Id = 3, Name = "Finance", Budget = 300000m }
            };
        }
    }

    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<int> EmployeeIds { get; set; } = new();

        public static List<Project> GetProjects()
        {
            return new List<Project>()
            {
                new Project { Id = 1, Name = "Project A", EmployeeIds = new List<int>{ 1, 2 } },
                new Project { Id = 2, Name = "Project B", EmployeeIds = new List<int>{ 3, 4 } },
                new Project { Id = 3, Name = "Project C", EmployeeIds = new List<int>{ 2, 5 } },
            };
        }
    }



    /*
Create comprehensive queries:

1. Department report:
- Department name
- Employee count
- Total payroll
- Average salary
- Budget vs actual spending

2. Employee hierarchy:
- Employee name
- Manager name
- Number of direct reports

3. Project staffing:
- Project name
- Number of employees
- Total cost (sum of salaries)
- Departments involved

4. Salary analysis:
- Employees earning above department average
- Salary distribution by department
- Top 10% earners

5. Cross-department analysis:
- Employees working on multiple projects
- Projects with cross-department teams
- Department collaboration matrix

Output formatted reports with all information.
*/
    internal class Program
    {

        static void Main(string[] args)
        {
            List<Employee> employees = Employee.GetEmployees();
            List<Department> departments = Department.GetDepartments();
            List<Project> projects = Project.GetProjects();

            //1. Department report:
            // - Department name
            // - Employee count
            // - Total payroll
            // - Average salary
            // - Budget vs actual spending

            var departmentReport = from d in departments
                                   join e in employees on d.Id equals e.DepartmentId into dptEmployees
                                   select new
                                   {
                                       Department = d.Name,
                                       EmployeeCount = dptEmployees.Count(),
                                       TotalPayroll = dptEmployees.Sum(s => s.Salary),
                                       AverageSalary = dptEmployees.Average(e => (double)e.Salary),
                                       Budget = d.Budget,
                                       Variance = d.Budget - dptEmployees.Sum(e => e.Salary)
                                   };

            Console.WriteLine("DEPARTMENT REPORT");
            Console.WriteLine("=".PadRight(50, '='));
            foreach (var dept in departmentReport)
            {
                System.Console.WriteLine($"\n{dept.Department}");
                System.Console.WriteLine($" Employees: {dept.EmployeeCount}");
                System.Console.WriteLine($" TotalPayroll: {dept.TotalPayroll}");
                System.Console.WriteLine($" Average Salary: {dept.AverageSalary}");
                System.Console.WriteLine($" Budget: {dept.Budget}");
                System.Console.WriteLine($" Variance: {dept.Variance}");
            }

            // 2. Employee hierarchy:
            // - Employee name
            // - Manager name
            // - Number of direct reports

            var employeeHierarchy = from e in employees
                                    join m in employees on e.ManagerId equals m.Id into mgrGroup
                                    from mg in mgrGroup.DefaultIfEmpty()
                                    let directReports = employees.Count(emp => emp.ManagerId == e.Id)
                                    select new
                                    {
                                        Employee = e.Name,
                                        Manager = mg != null ? mg.Name : "No Manager",
                                        DirectReports = directReports
                                    };
            Console.WriteLine("Employee hierarachy");
            Console.WriteLine("=".PadRight(50, '='));
            foreach (var emp in employeeHierarchy)
            {
                System.Console.WriteLine($"\nEmployee: {emp.Employee}");
                System.Console.WriteLine($" Manager: {emp.Manager}");
                System.Console.WriteLine($" DirectReports: {emp.DirectReports}");
            }

            //         3. Project staffing:
            //    - Project name
            //    - Number of employees
            //    - Total cost (sum of salaries)
            //    - Departments involved

            var projectStaffing = from p in projects
                                  from eid in p.EmployeeIds
                                  join e in employees on eid equals e.Id
                                  join d in departments on e.DepartmentId equals d.Id
                                  group new { e, d } by p.Name into g
                                  select new
                                  {
                                      ProjectName = g.Key,
                                      NumberOfEmployees = g.Select(x => x.e.Id).Distinct().Count(),
                                      TotalCost = g.Select(x => x.e.Salary).Sum(),
                                      DepartmentsInvolved = g.Select(x => x.d.Name).Distinct().ToList()
                                  };
            Console.WriteLine("Project Staffing");
            Console.WriteLine("=".PadRight(50, '='));
            foreach (var ps in projectStaffing)
            {
                Console.WriteLine($"Project: {ps.ProjectName}");
                Console.WriteLine($"  Employees: {ps.NumberOfEmployees}");
                Console.WriteLine($"  Total Cost: {ps.TotalCost:C}");
                Console.WriteLine($"  Departments: {string.Join(", ", ps.DepartmentsInvolved)}");
            }


            //  4. Salary analysis:
            //    - Employees earning above department average
            //    - Salary distribution by department
            //    - Top 10% earners
            Console.WriteLine("Salary analysis");
            Console.WriteLine("=".PadRight(50, '='));
            var departmentAvgSalary = employees
                                        .GroupBy(e => e.DepartmentId)
                                        .Select(g => new
                                        {
                                            DepartmentId = g.Key,
                                            AvgSalary = g.Average(e => e.Salary)

                                        }).ToList();
            var salaryAnalysis = from e in employees
                                 join d in departments on e.DepartmentId equals d.Id
                                 join avg in departmentAvgSalary on e.DepartmentId equals avg.DepartmentId
                                 where e.Salary > avg.AvgSalary
                                 select new
                                 {
                                     Employee = e.Name,
                                     Department = d.Name,
                                     Salary = e.Salary,
                                     DepartmentAverage = avg.AvgSalary
                                 };
            Console.WriteLine("Employees earning above department average:");
            foreach (var emp in salaryAnalysis)
            {
                Console.WriteLine($"{emp.Employee} ({emp.Department}): {emp.Salary:C} > Avg {emp.DepartmentAverage:C}");
            }
            //             5. Cross-department analysis:
            //    - Employees working on multiple projects
            //    - Projects with cross-department teams
            //    - Department collaboration matrix
            Console.WriteLine("Cross-department analysis");
            Console.WriteLine("=".PadRight(50, '='));
            var employeesMultipleProjects = from e in employees
                                            let projectCount = projects.Count(p => p.EmployeeIds.Contains(e.Id))
                                            where projectCount > 1
                                            select new
                                            {
                                                Employee = e.Name,
                                                ProjectCount = projectCount
                                            };
            Console.WriteLine("Employees working on multiple projects:");
            foreach (var emp in employeesMultipleProjects)
            {
                Console.WriteLine($"{emp.Employee}: {emp.ProjectCount} projects");
            }

            var crossDeptProjects = from p in projects
                                    let departmentsInvolved = p.EmployeeIds
                                        .Select(id => employees.First(e => e.Id == id).DepartmentId)
                                        .Distinct()
                                    where departmentsInvolved.Count() > 1
                                    select new
                                    {
                                        ProjectName = p.Name,
                                        Departments = departmentsInvolved
                                                    .Select(did => departments.First(d => d.Id == did).Name)
                                                    .ToList()
                                    };

            Console.WriteLine("\nProjects with cross-department teams:");
            foreach (var p in crossDeptProjects)
            {
                Console.WriteLine($"{p.ProjectName}: Departments [{string.Join(", ", p.Departments)}]");
            }

            var deptPairs = projects.SelectMany(p =>
{
    var deptIds = p.EmployeeIds.Select(id => employees.First(e => e.Id == id).DepartmentId)
                               .Distinct()
                               .OrderBy(id => id)
                               .ToList();

    // Generate all unique pairs of departments in this project
    return deptIds.SelectMany((d1, i) => deptIds.Skip(i + 1), (d1, d2) => new { Dept1 = d1, Dept2 = d2 });
});

            var collaborationMatrix = deptPairs
                .GroupBy(pair => new { pair.Dept1, pair.Dept2 })
                .Select(g => new
                {
                    Department1 = departments.First(d => d.Id == g.Key.Dept1).Name,
                    Department2 = departments.First(d => d.Id == g.Key.Dept2).Name,
                    ProjectsTogether = g.Count()
                });

            Console.WriteLine("\nDepartment collaboration matrix:");
            foreach (var c in collaborationMatrix)
            {
                Console.WriteLine($"{c.Department1} & {c.Department2}: {c.ProjectsTogether} project(s) together");
            }


        }




    }
}
