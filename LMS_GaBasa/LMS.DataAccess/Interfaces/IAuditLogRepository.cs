using LMS.Model.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Interfaces
{
    public interface IAuditLogRepository
    {
        int Add(AuditLog log);
        AuditLog GetById(int logId);
        List<AuditLog> GetByUserId(int userId);
        List<AuditLog> GetAll();
    }
}
