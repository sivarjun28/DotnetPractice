using Exercise02.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exercise02.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private static List<UserInfo> _users = new()
    {
        new UserInfo { Id = 1, Username = "admin", Role = Roles.Admin },
        new UserInfo { Id = 2, Username = "manager", Role = Roles.Manager },
        new UserInfo { Id = 3, Username = "user", Role = Roles.User }
    };

        [HttpGet]
        public ActionResult<IEnumerable<UserInfo>> GetAll() => Ok(_users);

        [HttpPut("{id}/role")]
        public ActionResult UpdateRole(int id, [FromBody] string role)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            if (!RolePermissions.permissions.ContainsKey(role)) return BadRequest("Invalid role");

            user.Role = role;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            _users.Remove(user);
            return NoContent();
        }
    }

    
}
