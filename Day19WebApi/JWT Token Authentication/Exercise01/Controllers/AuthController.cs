using System.Security.Claims;
using Exercise01.Models;
using Exercise01.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Exercise01.Controllers
{
    public static class AuthRoutes
    {
        public const string Register = "register";
        public const string Login = "login";
        public const string Refresh = "refresh";
        public const string Me = "me";
    }

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        public const string Register = "register";
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost(AuthRoutes.Register)]
        public async Task<ActionResult<AuthResponse>> AuthRegister(RegisterRequest registerRequest)
        {
            try
            {
                var response = await _authService.RegisterAsync(registerRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost(AuthRoutes.Login)]

        public async Task<ActionResult<AuthResponse>> AuthLogin(LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost(AuthRoutes.Refresh)]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest(new { message = "Refresh token is required" });

            try
            {
                var response = await _authService.RefreshTokenasync(refreshToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet(AuthRoutes.Me)]
        [Authorize]
        public ActionResult<UserInfo> GetCurrentUser()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userId == null || username == null || email == null || role == null)
                return Unauthorized();
            return new UserInfo
            {
                Id = int.Parse(userId),
                Username = username,
                Email = email,
                Role = role
            };

        }
    }
}