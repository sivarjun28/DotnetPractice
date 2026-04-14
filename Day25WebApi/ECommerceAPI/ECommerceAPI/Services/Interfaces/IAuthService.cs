using ECommerceAPI.Models.Entities;
using ECommerceAPI.Models.Entities.ECommerceAPI.Models.Entities;

namespace ECommerceAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
    }
}