using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Hashing
{
    /// <summary>
    /// Abstracts password hashing so the algorithm can be changed or mocked for tests.
    /// Implementations must return a hashed representation from <see cref="Hash"/> and
    /// be able to verify a plain password against a stored hash using <see cref="Verify"/>.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hash a plain-text password. Returned string should include any salt/metadata needed
        /// so Verify(...) can validate later.
        /// </summary>
        string Hash(string plainPassword);

        /// <summary>
        /// Verify whether the provided plain-text password matches the stored hashed value.
        /// </summary>
        bool Verify(string hashedPassword, string plainPassword);
    }
}
