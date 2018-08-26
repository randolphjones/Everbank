using System;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net.Mail;

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
                stringBuilder.Append(b.ToString("x2"));
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

        ///<summary>
        /// Checks the supplied password against complexity standards
        ///</summary>
        public static bool CheckPasswordComplexity(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }
            if (!Regex.IsMatch(password, "[A-z]"))
            {
                return false;
            }
            if (!Regex.IsMatch(password, "[0-9]"))
            {
                return false;
            }
            return true;
        }

        ///<summary>
        /// Returns a boolean to determine if an email address is in a valid format
        ///</summary>
        public static bool CheckEmailValidity(string emailAddress)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(emailAddress);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
