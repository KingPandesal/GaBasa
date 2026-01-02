using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Services.ArchiveUser
{
    public interface IArchiveUserService
    {
        ArchiveUserResult Archive(int userId);
    }

    public class ArchiveUserResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static ArchiveUserResult Ok() => new ArchiveUserResult { Success = true };
        public static ArchiveUserResult Fail(string message) => new ArchiveUserResult { Success = false, ErrorMessage = message };
    }
}
