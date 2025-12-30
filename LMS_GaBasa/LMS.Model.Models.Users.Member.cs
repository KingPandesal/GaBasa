using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.Models.Members;
using LMS.Model.Models.Enums;

namespace LMS.Model.Models.Users
{
    public class Member : User
    {
        public override Role Role => Role.Member;
        
        // update: obsolete methods moved to RolePermissionService
        //public override bool CanManageUsers() => false;
        //public override bool CanManageCatalog() => false;
        //public override bool CanCirculateBooks() => false;
        //public override bool CanViewReports() => false;

    }

}