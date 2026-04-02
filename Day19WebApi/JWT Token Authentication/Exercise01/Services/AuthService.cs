using Exercise01.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
namespace Exercise01.Services
{
    public class AuthService : IAuthService
    {
        private IConfiguration _configuration;
        private static readonly List<User> _users = new();
        private static readonly Dictionary<string, RefreshTokenData> _refreshTokens = new();
        private static int _nextUserId = 1;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {


            var existingUser = _users.FirstOrDefault(u => u.Username == request.Username || u.Email == request.Email);
            if (existingUser != null)
            {
                throw new Exception("Username or email already Exists");
            }

            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
            if (!regex.IsMatch(request.Password))
            {
                throw new Exception("Password does not match the requirement completely");
            }

            if (request.Password != request.ConfirmPassword)
            {
                throw new Exception("Password and Confirm Password does not match");
            }

            var hashPassword = HashPassword(request.Password);

            var newUser = new User
            {
                Id = _nextUserId++,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashPassword,
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = null
            };
            _users.Add(newUser);

            var token = GenerateJwtToken(newUser);
            var refreshToken = GenerateRefreshToken(new User());

            return new AuthResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpirationMinutes"]!)),
                User = new UserInfo
                {
                    Id = newUser.Id,
                    Username = newUser.Username,
                    Email = newUser.Email,
                    Role = newUser.Role
                }
            };
        }



        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = _users.FirstOrDefault(u => u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase));
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                throw new Exception("Invalid Username and Password");
            user.LastLoginAt = DateTime.UtcNow;

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(new User());
            var userInfo = new UserInfo
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
            return new AuthResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60")),
                User = userInfo
            };
        }




        public async Task<AuthResponse> RefreshTokenasync(string refreshToken)
        {
            if (!_refreshTokens.ContainsKey(refreshToken))
                throw new Exception("Invalid refresh token");
            var tokenData = _refreshTokens[refreshToken];
            if (tokenData.ExpiresAt < DateTime.UtcNow)
            {
                _refreshTokens.Remove(refreshToken);
                throw new Exception("Refresh token expired");
            }
            var user = _users.FirstOrDefault(u => u.Id == tokenData.UserId);
            if (user == null)
                throw new Exception("User Not Found");
            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken(new User());

            _refreshTokens.Remove(refreshToken);
            return new AuthResponse
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60")),
                User = new UserInfo
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role
                }
            };

        }


        public async Task<bool> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);


            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }


        public string GenerateRefreshToken(User user)
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));


            var refreshTokenExpStr = _configuration["Jwt:RefreshTokenExpirationDays"] ?? throw new Exception("JWT RefreshTokenExpirationDays is missing!");
            if (!int.TryParse(refreshTokenExpStr, out int refreshTokenDays))
                throw new Exception("JWT RefreshTokenExpirationDays is not a valid number!");

            _refreshTokens[refreshToken] = new RefreshTokenData
            {
                UserId = _nextUserId - 1,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDays)
            };

            return refreshToken;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        
        public string GenerateJwtToken(User user)
        {

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };


            var secret = _configuration["Jwt:Key"] ?? throw new Exception("JWT Key is missing!");
            var issuer = _configuration["Jwt:Issuer"] ?? throw new Exception("JWT Issuer is missing!");
            var audience = _configuration["Jwt:Audience"] ?? throw new Exception("JWT Audience is missing!");
            var expirationMinutesStr = _configuration["Jwt:ExpirationMinutes"] ?? throw new Exception("JWT ExpirationMinutes is missing!");

            if (!int.TryParse(expirationMinutesStr, out int expirationMinutes))
                throw new Exception("JWT ExpirationMinutes is not a valid number!");


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }

}