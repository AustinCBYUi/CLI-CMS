using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

/*
 * Hasher class to hash and verify passwords
 * @Author: Austin Campbell
 */

namespace CLI_CMS.src
{
    internal class Hasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int iterations = 100_000;


        /// <summary>
        /// Hash the password so it can be stored in the DB.
        /// </summary>
        /// <param name="password">Normal text password</param>
        /// <returns>Hashed password</returns>
        public static string Hash(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                byte[] hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
                return Convert.ToBase64String(hashBytes);
            }
        }


        /// <summary>
        /// Verify the password, compare the string with the hashed password
        /// </summary>
        /// <param name="password">Regular password entered by user.</param>
        /// <param name="hashedPassword">Hashed password from the DB.</param>
        /// <returns></returns>
        public static bool Verify(string password, string hashedPassword)
        {
            //Decode
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            //Extract salt
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            //Extract hash
            byte[] hash = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, hash, 0, HashSize);

            //Verify
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] computedHash = pbkdf2.GetBytes(HashSize);

                return CryptographicOperations.FixedTimeEquals(hash, computedHash);
            }
        }
    }
}
