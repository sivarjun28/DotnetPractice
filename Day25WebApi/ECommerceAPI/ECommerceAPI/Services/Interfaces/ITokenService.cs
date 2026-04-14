using ECommerceAPI.Models.Entities;

namespace ECommerceAPI.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        Task<User?> ValidateRefreshTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
    }
}