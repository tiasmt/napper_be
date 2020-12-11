using System;
using System.Security.Cryptography;

namespace napper_be.Helpers
{
    public static class PasswordUtils
    {
        public static string HashPassword(byte[] salt, string password)
        {
            //Create the Rfc2898DeriveBytes and get the hash value:
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            //Combine the salt and password bytes for later use:
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            //Return combined salt+hash into a string for storage
            return Convert.ToBase64String(hashBytes);
        }
    }
}
