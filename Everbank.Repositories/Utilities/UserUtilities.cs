using System;
using System.Text;
using System.Security.Cryptography;

namespace Everbank.Repositories.Utilities
{
    public static class UserUtilities
    {
        ///<summary>
        ///Returns a SHA256 hash of the provided string
        ///</summary>
        public static string HashString(string input)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] encryptedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder stringBuilder = new StringBuilder();
            foreach(byte b in encryptedBytes)
            {
                stringBuilder.Append(b);
            }
            return stringBuilder.ToString();
        }

        ///<summary>
        /// Conforms a string to lower case and trims any white space for storage
        ///</summary>
        public static string ConformString(string input)
        {
            return input.ToLower().Trim();
        }
    }
}
