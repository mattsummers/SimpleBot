using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SimpleBotWeb.Models.Helpers
{

    public class EncryptionHelper
    {
        /// <summary>
        /// Good for generating salts, or initialization vectors or whatnot.
        /// </summary>
        /// <param name="byteLength">16 is a useful length, used by many encryption algorithms</param>
        /// <returns>A base64 encoded string</returns>
        public static string GenerateRandomNoise(int byteLength)
        {
            // Create two random values
            RandomNumberGenerator randomGenerator = RandomNumberGenerator.Create();
            byte[] randomNoise = new byte[byteLength];

            // Fill this array with nonsense
            randomGenerator.GetBytes(randomNoise);

            return Convert.ToBase64String(randomNoise.Take(byteLength).ToArray());
        }

        /// <summary>
        /// A computationally expensive KDF that is difficult to greatly parallelize even on specialized hardware due to memory requirements requirements.
        /// This function should generally only be used in instances where it is called once, like setting or verifying a plaintext password. 
        /// This function also specifies defaults for N, r, p, and block size
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A base64 encoded string</returns>
        public static string GenerateSCryptHash(string input, string salt)
        {
            var extraSalt = (@"$2a$10$ibW6QAL3GpiD3rX8AyjNM.");

            // Convert the original string to array of Bytes
            var output = BCrypt.Net.BCrypt.HashPassword(input + salt, extraSalt);
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(output));
        }
    }
}
