using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LSSD.StoreFront.Lib.Utilities
{
    public static class Crypto
    {
        public static string Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedValue = sha256.ComputeHash(Encoding.Default.GetBytes(input));

                StringBuilder returnMe = new StringBuilder();
                for (int i = 0; i < hashedValue.Length; i++)
                {
                    returnMe.Append(hashedValue[i].ToString("x2"));
                }
                return returnMe.ToString();
            }
        }
    }
}
