using System.Security.Cryptography;
using System.Text;

namespace EVote360Pro.Core.Application.Helpers
{
    public static class PasswordEncryption
    {
        public static string Encrypt(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}