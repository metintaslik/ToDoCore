using System;
using System.Security.Cryptography;
using System.Text;

namespace ToDo.API.Helper
{
    public class Hasher
    {
        public static string Hash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "").ToLower();
            }
        }
    }
}
