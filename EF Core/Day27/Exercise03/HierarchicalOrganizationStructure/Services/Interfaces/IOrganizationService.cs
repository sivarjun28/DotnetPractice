using HierarchicalOrganizationStructure.Models.Entities;
using HierarchicalOrganizationStructure.Models.Requests;

namespace HierarchicalOrganizationStructure.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<Department> CreateDepartmentAsync(string name, int? parentDepartmentId);
        Task<Employee> HireEmployeeAsync(HireEmployeeRequest request);
        Task AssignManagerAsync(int employeeId, int managerId);
        Task<Department?> GetDepartmentHierarchyAsync(int departmentId);
        Task<Employee?> GetEmployeeWithTeamAsync(int employeeId);
        Task<List<Employee>> GetAllSubordinatesAsync(int managerId);
        Task<List<Department>> GetCompanyStructureAsync(); 
    }

}