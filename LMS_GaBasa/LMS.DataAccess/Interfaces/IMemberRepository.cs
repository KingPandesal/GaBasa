using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.DTOs.Member;

namespace LMS.DataAccess.Interfaces
{
    public interface IMemberRepository
    {
        /// <summary>
        /// Gets member profile data by UserID (joins User, Member, and MemberType tables)
        /// </summary>
        MemberProfileDto GetMemberProfileByUserId(int userId);
        
        bool UpdateMemberProfile(int userId, string firstName, string lastName, string email, 
            string contactNumber, string photoPath, string address, string validIdPath, string username);
        
        bool UpdateMemberPassword(int userId, string newPasswordHash);
        
        string GetPasswordHash(int userId);
        
        bool UsernameExistsForOtherUser(int userId, string username);
    }
}
