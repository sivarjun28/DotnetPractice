namespace HierarchicalOrganizationStructure.Models.Requests
{
    public class HireEmployeeRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public decimal Salary { get; set; }

        public int DepartmentId { get; set; }
        public int? ManagerId { get; set; }
    }
}