using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.DTOs.Member
{
    public class DTORenewMemberResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static DTORenewMemberResult SuccessResult()
        {
            return new DTORenewMemberResult { Success = true };
        }

        public static DTORenewMemberResult Failure(string errorMessage)
        {
            return new DTORenewMemberResult { Success = false, ErrorMessage = errorMessage };
        }
    }
}
