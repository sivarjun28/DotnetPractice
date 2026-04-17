using HierarchicalOrganizationStructure.Models.Requests;
using HierarchicalOrganizationStructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HierarchicalOrganizationStructure.Controllers
{

namespace BlogSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _service;

        public OrganizationController(IOrganizationService service)
        {
            _service = service;
        }


        [HttpPost("department")]
        public async Task<IActionResult> CreateDepartment(
            string name,
            int? parentDepartmentId)
        {
            var result = await _service.CreateDepartmentAsync(name, parentDepartmentId);
            return Ok(result);
        }

        [HttpPost("employee")]
        public async Task<IActionResult> HireEmployee([FromBody] HireEmployeeRequest request)
        {
            var result = await _service.HireEmployeeAsync(request);
            return Ok(result);
        }


        [HttpPut("assign-manager")]
        public async Task<IActionResult> AssignManager(int employeeId, int managerId)
        {
            await _service.AssignManagerAsync(employeeId, managerId);
            return Ok("Manager assigned successfully");
        }

        [HttpGet("department/{id}")]
        public async Task<IActionResult> GetDepartmentHierarchy(int id)
        {
            var result = await _service.GetDepartmentHierarchyAsync(id);

            if (result == null)
                return NotFound("Department not found");

            return Ok(result);
        }


        [HttpGet("employee/{id}")]
        public async Task<IActionResult> GetEmployeeWithTeam(int id)
        {
            var result = await _service.GetEmployeeWithTeamAsync(id);

            if (result == null)
                return NotFound("Employee not found");

            return Ok(result);
        }


        [HttpGet("employee/{id}/subordinates")]
        public async Task<IActionResult> GetAllSubordinates(int id)
        {
            var result = await _service.GetAllSubordinatesAsync(id);
            return Ok(result);
        }

        [HttpGet("company-structure")]
        public async Task<IActionResult> GetCompanyStructure()
        {
            var result = await _service.GetCompanyStructureAsync();
            return Ok(result);
        }
    }
}
}