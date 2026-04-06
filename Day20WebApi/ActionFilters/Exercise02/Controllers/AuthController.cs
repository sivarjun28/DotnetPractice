using Microsoft.AspNetCore.Mvc;
using Exercise02.Models;
using Exercise02.Services;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "Admin123!")
            {
                var token = _authService.GenerateJwtToken("admin", "Admin");
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
    }
}