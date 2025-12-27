using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Security
{
    public interface IPermissionService
    {
        // MAIN
        // wla dri ang dashboard
        bool CanViewCatalog(User user); // All roles

        // INSIGHTS
        bool CanGenerateReports(User user); // Librarian only

        // MANAGEMENT
        bool CanManageUsers(User user); // Librarian only
        bool CanManageMembers(User user);
        bool CanManageInventory(User user);
        bool CanManageReservations(User user);
        bool CanManageCirculation(User user);
        bool CanManageFines(User user);

        // CONFIGURATION
        bool CanAccessSettings(User user); // Librarian only

        // Members Only
        // sidebar
        bool CanViewWishlist(User user);
        bool CanViewBorrowed(User user);
        bool CanViewOverdue(User user);
        bool CanViewReserved(User user);
        bool CanViewFines(User user);
        bool CanViewHistory(User user);

        // member actions
        bool CanWishlistBooks(User user);
        bool CanBorrowBooks(User user);
        bool CanReturnBooks(User user);
        bool CanReserveBooks(User user);
        bool CanCancelReservation(User user);

        // end code
    }
}
