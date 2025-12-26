using LMS.BusinessLogic.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Hashing
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        // Work factor (cost). 10-12 is a common sweet spot; tune for your environment.
        private readonly int _workFactor;

        public BcryptPasswordHasher(int workFactor = 12)
        {
            if (workFactor < 4 || workFactor > 31)
                throw new ArgumentOutOfRangeException(nameof(workFactor), "Work factor must be between 4 and 31.");
            _workFactor = workFactor;
        }

        public string Hash(string plainPassword)
        {
            if (plainPassword == null) throw new ArgumentNullException(nameof(plainPassword));
            // Generates a salted bcrypt hash. Result contains salt and cost metadata.
            return BCrypt.Net.BCrypt.HashPassword(plainPassword, _workFactor);
        }

        public bool Verify(string hashedPassword, string plainPassword)
        {
            if (hashedPassword == null) throw new ArgumentNullException(nameof(hashedPassword));
            if (plainPassword == null) throw new ArgumentNullException(nameof(plainPassword));

            // BCrypt.Verify expects (plainPassword, hashedPassword).
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
