using System.Security.Cryptography;

namespace Exercise01.Helpers
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