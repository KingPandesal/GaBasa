using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Helpers
{
    public static class UserIdFormatter
    {
        private const int MinDigits = 4;

        public static string Format(int id, string role)
        {
            string prefix = GetRolePrefix(role);
            return FormatWithPrefix(id, prefix);
        }

        public static string FormatMemberId(int memberId)
        {
            return FormatWithPrefix(memberId, "MEM");
        }

        private static string FormatWithPrefix(int id, string prefix)
        {
            int actualDigits = id.ToString().Length;
            int padLength = MinDigits > actualDigits ? MinDigits : actualDigits;
            return $"{prefix}-{id.ToString().PadLeft(padLength, '0')}";
        }

        private static string GetRolePrefix(string role)
        {
            switch (role?.ToLower())
            {
                case "librarian":
                case "librarian / admin":
                    return "LIB";
                case "staff":
                case "library staff":
                    return "STF";
                case "member":
                    return "MEM";
                default:
                    return "USR";
            }
        }
    }
}
