namespace HierarchicalOrganizationStructure.Models.Entities
{
    public class Department
    {
        public int Id{get; set;}
        public string Name{get; set;} = string.Empty;
        public int? ParentDepartmentId{get; set;}
        public int? ManagerId { get; set; }   

        public Department? ParentDepartment{get; set;}
        public List<Department> SubDepartments{get; set;} = new();

        public List<Employee> Employees{get; set;} = new();
        public Employee? Manager{get; set;} 
    }
}