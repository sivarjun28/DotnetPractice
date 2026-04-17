namespace HierarchicalOrganizationStructure.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }

        public int DepartmentId{get; set;}
        public int? ManagerId{get; set;}

        public Department Department{get; set;} = null!;
        public Employee? Manager{get; set;}
        public List<Employee> Subordinates { get; set; } = new();

    }
}