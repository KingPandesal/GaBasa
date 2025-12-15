using LMS.BusinessLogic.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Security
{
    public class Sha256PasswordHasher : IPasswordHasher
    {
        public string Hash(string plainPassword)
        {
            if (plainPassword == null) throw new ArgumentNullException(nameof(plainPassword));

            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(plainPassword);
                byte[] hash = sha256.ComputeHash(bytes);

                var sb = new StringBuilder();
                foreach (byte b in hash)
                    sb.Append(b.ToString("X2"));

                return sb.ToString();
            }
        }

        public bool Verify(string hashedPassword, string plainPassword)
        {
            if (hashedPassword == null) throw new ArgumentNullException(nameof(hashedPassword));
            if (plainPassword == null) throw new ArgumentNullException(nameof(plainPassword));

            string computed = Hash(plainPassword);
            return FixedTimeEquals(computed, hashedPassword);
        }

        // simple fixed-time string comparer to reduce timing attack surface
        private bool FixedTimeEquals(string a, string b)
        {
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];

            return diff == 0;
        }

        // end code
    }
}
