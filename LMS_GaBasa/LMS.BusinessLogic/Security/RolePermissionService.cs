using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.Models.Enums;
using LMS.DataAccess.Repositories;

namespace LMS.BusinessLogic.Security
{
    public class RolePermissionService : IPermissionService
    {
        // ========== MAIN ==========
        // All roles can view catalog
        // wla dri ang dashboard
        public bool CanViewCatalog(User user) => true;

        // ========== INSIGHTS ==========
        public bool CanGenerateReports(User user)
            => user.Role == Role.Librarian;

        // ========== MANAGEMENT ==========
        public bool CanManageUsers(User user)
            => user.Role == Role.Librarian;

        public bool CanManageMembers(User user)
            => user.Role == Role.Librarian 
            || user.Role == Role.Staff;

        public bool CanManageInventory(User user)
            => user.Role == Role.Librarian
            || user.Role == Role.Staff;

        public bool CanManageReservations(User user)
            => user.Role == Role.Librarian
            || user.Role == Role.Staff;

        public bool CanManageCirculation(User user)
            => user.Role == Role.Librarian
            || user.Role == Role.Staff;

        public bool CanManageFines(User user)
            => user.Role == Role.Librarian
            || user.Role == Role.Staff;

        // ========== CONFIGURATION ==========
        public bool CanAccessSettings(User user)
            => user.Role == Role.Librarian;

        // ========== Members Only ==========
        // sidebar
        public bool CanViewWishlist(User user)
            => user.Role == Role.Member;

        public bool CanViewBorrowed(User user)
            => user.Role == Role.Member;

        public bool CanViewOverdue(User user)
            => user.Role == Role.Member;

        public bool CanViewReserved(User user)
            => user.Role == Role.Member;

        public bool CanViewFines(User user)
            => user.Role == Role.Member;

        public bool CanViewHistory(User user)
            => user.Role == Role.Member;

        // member actions
        public bool CanWishlistBooks(User user)
            => user.Role == Role.Member;

        public bool CanBorrowBooks(User user)
            => user.Role == Role.Member;

        public bool CanReturnBooks(User user)
            => user.Role == Role.Member;

        public bool CanReserveBooks(User user)
        {
            // Require Member role
            if (user == null || user.Role != Role.Member)
                return false;

            try
            {
                // Use MemberRepository to obtain the member profile DTO which contains ReservationPrivilege.
                var repo = new MemberRepository();
                var profile = repo.GetMemberProfileByUserId(user.UserID);

                // If we couldn't load profile, deny by default.
                if (profile == null)
                    return false;

                // Reservation allowed only when the MemberType grants the privilege.
                if (!profile.ReservationPrivilege)
                    return false;

                // Deny reservation if member has unpaid fines
                if (profile.TotalUnpaidFines > 0)
                    return false;

                // Deny reservation if member has overdue items
                if (profile.OverdueCount > 0)
                    return false;

                return true;
            }
            catch
            {
                // On any error, be conservative and deny reservation capability.
                return false;
            }
        }

        public bool CanCancelReservation(User user)
            => user.Role == Role.Member;

        // end code
    }
}
