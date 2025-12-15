using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.Models.Users
{
    public class LibraryStaff : User
    {
        public override Role Role => Role.Staff;

        // update: obsolete methods moved to RolePermissionService
        //public override bool CanManageUsers() => false;
        //public override bool CanManageCatalog() => true;
        //public override bool CanCirculateBooks() => true;
        //public override bool CanViewReports() => false;


    }

}
