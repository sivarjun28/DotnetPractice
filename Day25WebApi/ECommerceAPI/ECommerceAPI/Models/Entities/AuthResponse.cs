namespace ECommerceAPI.Models.Entities
{
    namespace ECommerceAPI.Models.Entities
    {
        public class AuthResponse
        {
            public string Token { get; set; } = string.Empty;
            public string RefreshToken { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
            public string Role { get; set; } = "Customer";
            public DateTime ExpiresAt { get; set; }

            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;

            public AuthResponse(string token, string refreshToken, string username, string role, DateTime expiresAt)
            {
                Token = token;
                RefreshToken = refreshToken;
                Username = username;
                Role = role;
                ExpiresAt = expiresAt;
                Success = true;
                Message = "Operation successful";
            }

            public AuthResponse()
            {
                Success = false;
                Message = "Operation failed";
            }
        }
    }
}