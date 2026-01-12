using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Services.Audit
{
    public interface IAuditLogService
    {
        void LogAddBook(int userId, int copyCount, string title, string category);
        void LogEditBook(int userId, string title);
        void LogAddUser(int userId, string roleAdded);
        void LogAddMember(int userId, string memberType, string memberFullName);
        void LogPayFines(int userId, string memberFullName);
        void LogWaiveFines(int userId, string memberFullName, string reason);
        void LogApproveBorrowBook(int userId, string memberFullName, string bookTitle);
        void LogReturnBook(int userId, string memberFullName, string bookTitle);
        void LogRenewBook(int userId, string memberFullName, string bookTitle);
        void LogReserveBook(int userId, string memberFullName, string bookTitle);
        void LogBulkImportBook(int userId);
    }
}
