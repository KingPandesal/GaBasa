using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.Models.Users
{
    public class Librarian : User
    {
        public override string Role => "Admin";

    }
}
