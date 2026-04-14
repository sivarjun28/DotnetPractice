using ECommerceAPI.Models.Entities;
using ECommerceAPI.Models.Entities.ECommerceAPI.Models.Entities;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI.Services.Implementations
{

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService,
            ILogger<AuthService> logger,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User already exists"
                };
            }

            var hashedPassword = _passwordHasher.HashPassword(null, request.Password);

            var user = new User
            {
                Email = request.Email,
                Username = request.Username,
                PasswordHash = hashedPassword,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = "Customer",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateUserAsync(user);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var expiresAt = DateTime.UtcNow.AddHours(1);

            return new AuthResponse(accessToken, refreshToken, user.Username, user.Role, expiresAt);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Username);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddHours(1);

            return new AuthResponse(accessToken, refreshToken, user.Username, user.Role, expiresAt);
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var user = await _tokenService.ValidateRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid refresh token"
                };
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            return new AuthResponse
            {
                Success = true,
                Message = "Tokens refreshed successfully",
                Token = accessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            return await _tokenService.ValidateTokenAsync(token);
        }
    }
}