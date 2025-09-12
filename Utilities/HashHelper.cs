using System.Security.Cryptography;
using System.Text;

namespace PathLabAPI.Utilities
{
    public class HashHelper
    {
        public static string Sha256(string plain)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(plain ?? "");
            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }
    }
}
