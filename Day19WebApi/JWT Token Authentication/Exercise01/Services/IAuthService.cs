using Exercise01.Models;

namespace Exercise01.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenasync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
        string GenerateRefreshToken(User user);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
