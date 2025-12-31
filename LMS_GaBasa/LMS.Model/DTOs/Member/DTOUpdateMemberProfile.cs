using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.DTOs.User;

namespace LMS.Model.DTOs.Member
{
    public class DTOUpdateMemberProfile : DTOUpdateUserProfile
    {
        public string Address { get; set; }

    }
}
