using System.Security.Cryptography;
using System.Text;

namespace Welp.Models
{
    public class Hasher
    {
        public static string GenerateSalt(int size)
        {
            var rng = new RNGCryptoServiceProvider();
            var salt = new byte[size];
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public static string ComputeHmacHash(string input, string salt)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt)))
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = hmac.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}