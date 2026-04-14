using System.Security.Cryptography;

namespace ECommerceAPI.Helpers
{
    public class SecurityKeyHelper
    {
        public static string GenerateSecurityKey()
        {
            using (var hmac = new HMACSHA256())
            {
                var key = hmac.Key;
                return Convert.ToBase64String(key);
            }
        }
    }
}