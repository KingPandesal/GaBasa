using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Hashing
{
    public interface IPasswordHasher
    {
        string Hash(string plainPassword);

        bool Verify(string hashedPassword, string plainPassword);
    }
}
