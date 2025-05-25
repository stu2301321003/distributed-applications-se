using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace VacationManager.Auth.Helpers
{
    public static class PasswordHelper
    {
        public static string GenerateSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string salt) =>
            Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                    password: password!,
                                    salt: Encoding.UTF8.GetBytes(salt),
                                    prf: KeyDerivationPrf.HMACSHA256,
                                    iterationCount: 100000,
                                    numBytesRequested: 256 / 8));
    }
}
