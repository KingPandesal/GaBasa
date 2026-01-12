using LMS.DataAccess.Interfaces;
using LMS.Model.Models.Transactions;
using System;

namespace LMS.BusinessLogic.Services.Audit
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepo;

        public AuditLogService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepo = auditLogRepository ?? throw new ArgumentNullException(nameof(auditLogRepository));
        }

        public void LogAddBook(int userId, int copyCount, string title, string category)
        {
            var log = new AuditLog
            {
                UserID = userId,
                ModuleName = "Inventory",
                ActionPerformed = "Add Book",
                Details = $"Added {copyCount} copy of {title} to {category}",
                Timestamp = DateTime.UtcNow
            };

            _auditLogRepo.Add(log);
        }

        public void LogEditBook(int userId, string title)
        {
            var log = new AuditLog
            {
                UserID = userId,
                ModuleName = "Inventory",
                ActionPerformed = "Edit Book",
                Details = $"Edited book: {title}",
                Timestamp = DateTime.UtcNow
            };

            _auditLogRepo.Add(log);
        }

        public void LogAddUser(int userId, string roleAdded)
        {
            var log = new AuditLog
            {
                UserID = userId,
                ModuleName = "User Module",
                ActionPerformed = "Add User",
                Details = $"Added {roleAdded}",
                Timestamp = DateTime.UtcNow
            };

            _auditLogRepo.Add(log);
        }

        public void LogAddMember(int userId, string memberType, string memberFullName)
        {
            var detailsName = string.IsNullOrWhiteSpace(memberFullName) ? "Unknown" : memberFullName.Trim();
            var log = new AuditLog
            {
                UserID = userId,
                ModuleName = "Member Module",
                ActionPerformed = "Add Member",
                Details = $"Added a {memberType} member: {detailsName}",
                Timestamp = DateTime.UtcNow
            };

            _auditLogRepo.Add(log);
        }

        public void LogPayFines(int userId, string memberFullName)
        {
            var detailsName = string.IsNullOrWhiteSpace(memberFullName) ? "Unknown" : memberFullName.Trim();
            var log = new AuditLog
            {
                UserID = userId,
                ModuleName = "Fine Module",
                ActionPerformed = "Pay Fines",
                Details = $"Payed fines for {detailsName}",
                Timestamp = DateTime.UtcNow
            };

            _auditLogRepo.Add(log);
        }

        public void LogWaiveFines(int userId, string memberFullName, string reason)
        {
            var detailsName = string.IsNullOrWhiteSpace(memberFullName) ? "Unknown" : memberFullName.Trim();
            var detailsReason = string.IsNullOrWhiteSpace(reason) ? "No reason provided" : reason.Trim();

            var log = new AuditLog
            {
                UserID = userId,
                ModuleName = "Fine Module",
                ActionPerformed = "Waive Fines",
                Details = $"Waived fines for {detailsName}. Reason: {detailsReason}",
                Timestamp = DateTime.UtcNow
            };

            _auditLogRepo.Add(log);
        }

        public void LogApproveBorrowBook(int userId, string memberFullName, string bookTitle)
        {
            var detailsName = string.IsNullOrWhiteSpace(memberFullName) ? "Unknown" : memberFullName.Trim();
            var detailsTitle = string.IsNullOrWhiteSpace(bookTitle) ? "Unknown" : bookTitle.Trim();

            var log = new AuditLog
            {
                UserID = userId,
                ModuleName = "Circulation Module",
                ActionPerformed = "Approved Borrow Book",
                Details = $"Approved a borrow book for {detailsName}. Book: {detailsTitle}",
                Timestamp = DateTime.UtcNow
            };

            _auditLogRepo.Add(log);
        }

        public void LogReturnBook(int userId, string memberFullName, string bookTitle)
        {
            var detailsName = string.IsNullOrWhiteSpace(memberFullName) ? "Unknown" : memberFullName.Trim();
            var detailsTitle = string.IsNullOrWhiteSpace(bookTitle) ? "Unknown" : bookTitle.Trim();

            var log = new AuditLog
            {
                UserID = userId,
                ModuleName = "Circulation Module",
                ActionPerformed = "Return Book",
                Details = $"Return book for {detailsName}. Book: {detailsTitle}",
                Timestamp = DateTime.UtcNow
            };

            _auditLogRepo.Add(log);
        }

        public void LogRenewBook(int userId, string memberFullName, string bookTitle)
        {
            var detailsName = string.IsNullOrWhiteSpace(memberFullName) ? "Unknown" : memberFullName.Trim();
            var detailsTitle = string.IsNullOrWhiteSpace(bookTitle) ? "Unknown" : bookTitle.Trim();

            var log = new AuditLog
            {
                UserID = userId,
                ModuleName = "Circulation Module",
                ActionPerformed = "Renew Book",
                Details = $"Renewed book for {detailsName}. Book: {detailsTitle}",
                Timestamp = DateTime.UtcNow
            };

            _auditLogRepo.Add(log);
        }

        public void LogReserveBook(int userId, string memberFullName, string bookTitle)
        {
            var detailsName = string.IsNullOrWhiteSpace(memberFullName) ? "Unknown" : memberFullName.Trim();
            var detailsTitle = string.IsNullOrWhiteSpace(bookTitle) ? "Unknown" : bookTitle.Trim();

            var log = new AuditLog
            {
                UserID = userId,
                ModuleName = "Catalog Module",
                ActionPerformed = "Reserve Book",
                Details = $"{detailsName} reserved book: {detailsTitle}",
                Timestamp = DateTime.UtcNow
            };

            _auditLogRepo.Add(log);
        }

        public void LogBulkImportBook(int userId)
        {
            var log = new AuditLog
            {
                UserID = userId,
                ModuleName = "Inventory Module",
                ActionPerformed = "Bulk Import Book",
                Details = "Bulk imported books",
                Timestamp = DateTime.UtcNow
            };

            _auditLogRepo.Add(log);
        }
    }
}