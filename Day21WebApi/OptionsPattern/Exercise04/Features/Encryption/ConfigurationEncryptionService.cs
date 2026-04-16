using System.Text;

namespace Exercise04.Features.Encryption
{
    public class ConfigurationEncryptionService
    {
        public string Encrypt(string plain)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));
        }
        public string Bcrypt(string encrypted)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encrypted));
        }
    }
}