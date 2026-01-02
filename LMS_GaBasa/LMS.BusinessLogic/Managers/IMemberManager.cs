using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.Models.Enums;

namespace LMS.BusinessLogic.Managers
{
    public interface IMemberManager
    {
        /// <summary>
        /// Gets the member status by user ID. Returns null if user is not a member.
        /// </summary>
        MemberStatus? GetMemberStatus(int userId);

        /// <summary>
        /// Validates if a member can login based on their MemberStatus.
        /// Returns the appropriate AuthFailureReason if login should be denied.
        /// </summary>
        AuthFailureReason? ValidateMemberLoginStatus(int userId);
    }
}
