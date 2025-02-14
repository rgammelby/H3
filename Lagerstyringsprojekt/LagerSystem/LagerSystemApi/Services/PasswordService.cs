using System.Security.Cryptography;
using System.Text;

namespace LagerSystemApi.Services
{
    // byg password service
    public class PasswordService
    {
        // generates a salt to be stored on the user
        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);

        }

        // salts and hashes a password using the generated salt for the user and SHA256 hashing alg
        public string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashedBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // returns true/false depending on whether or not the hashed and salted input password matches the stored password
        public bool VerifyPassword(string inputPassword, string storedHash, string salt)
        {
            string hashedInput = HashPassword(inputPassword, salt);
            return hashedInput == storedHash;
        }
    }

}
