using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Model.DTOs.Member
{
    public class DTOCreateMemberResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int MemberId { get; set; }

        public static DTOCreateMemberResult SuccessResult(int memberId)
        {
            return new DTOCreateMemberResult { Success = true, MemberId = memberId };
        }

        public static DTOCreateMemberResult Failure(string errorMessage)
        {
            return new DTOCreateMemberResult { Success = false, ErrorMessage = errorMessage };
        }
    }
}
