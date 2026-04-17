using HierarchicalOrganizationStructure.Data;
using HierarchicalOrganizationStructure.Models.Entities;
using HierarchicalOrganizationStructure.Models.Requests;
using HierarchicalOrganizationStructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HierarchicalOrganizationStructure.Services.Implementations
{
    public class OrganizationService : IOrganizationService
    {

        private readonly EmployeedbContext _context;
        public OrganizationService(EmployeedbContext context)
        {
            _context = context;
        }
        public async Task AssignManagerAsync(int employeeId, int managerId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null) throw new Exception("Employee not found");

            employee.ManagerId = managerId;

            await _context.SaveChangesAsync();
        }

        public async Task<Department> CreateDepartmentAsync(string name, int? parentDepartmentId)
        {
            var department = new Department
            {
                Name = name,
                ParentDepartmentId = parentDepartmentId
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;


        }

        public async Task<List<Employee>> GetAllSubordinatesAsync(int managerId)
        {
            var result = new List<Employee>();

            var direct = await _context.Employees
                             .Where(e => e.ManagerId == managerId)
                             .ToListAsync();

            foreach (var emp in direct)
            {
                result.Add(emp);
                result.AddRange(await GetAllSubordinatesAsync(emp.Id));
            }
            return result;
        }

        public async Task<List<Department>> GetCompanyStructureAsync()
        {
            return await _context.Departments
                        .Where(d => d.ParentDepartmentId == null)
                        .Include(d => d.SubDepartments)
                            .ThenInclude(sd => sd.SubDepartments)
                        .Include(d => d.Employees)
                        .ToListAsync();
        }

        public async Task<Department?> GetDepartmentHierarchyAsync(int departmentId)
        {
            return await _context.Departments
                    .Include(d => d.Employees)
                    .Include(d => d.Manager)
                    .Include(d => d.SubDepartments)
                            .ThenInclude(sd => sd.Employees)
                    .FirstOrDefaultAsync(d => d.Id == departmentId);

        }

        public async Task<Employee?> GetEmployeeWithTeamAsync(int employeeId)
        {
            return await _context.Employees
                        .Include(e => e.Department)
                        .Include(e => e.Manager)
                        .Include(e => e.Subordinates)
                        .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task<Employee> HireEmployeeAsync(HireEmployeeRequest request)
        {
            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Salary = request.Salary,
                JobTitle = request.JobTitle,
                HireDate = DateTime.UtcNow,
                DepartmentId = request.DepartmentId,
                ManagerId = request.ManagerId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
    }
}