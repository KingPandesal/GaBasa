using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.Models.Enums;

namespace LMS.Model.Models.Users
{
    public class Librarian : User
    {
        public override Role Role => Role.Librarian;

        protected override string RolePrefix => "LIB";

        // update: obsolete methods moved to RolePermissionService
        //public override bool CanManageUsers() => true;
        //public override bool CanManageCatalog() => true;
        //public override bool CanCirculateBooks() => true;
        //public override bool CanViewReports() => true;

    }
}
