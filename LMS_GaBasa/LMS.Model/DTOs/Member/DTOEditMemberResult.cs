using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.DTOs.Member
{
    public class DTOEditMemberResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static DTOEditMemberResult SuccessResult()
        {
            return new DTOEditMemberResult { Success = true };
        }

        public static DTOEditMemberResult Failure(string errorMessage)
        {
            return new DTOEditMemberResult { Success = false, ErrorMessage = errorMessage };
        }
    }
}
