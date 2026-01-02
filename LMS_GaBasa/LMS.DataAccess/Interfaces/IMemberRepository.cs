using System;
using System.Collections.Generic;
using LMS.Model.DTOs.Member;
using LMS.Model.Models.Enums;

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

        /// <summary>
        /// Checks if username already exists
        /// </summary>
        bool UsernameExists(string username);

        /// <summary>
        /// Gets MemberTypeID by type name
        /// </summary>
        int? GetMemberTypeIdByName(string typeName);

        /// <summary>
        /// Creates a new member (inserts into User and Member tables)
        /// </summary>
        int AddMember(string firstName, string lastName, string email, string contactNumber,
            string username, string passwordHash, string photoPath, string address, 
            string validIdPath, int memberTypeId);

        /// <summary>
        /// Gets member status by UserID (returns null if not a member)
        /// </summary>
        MemberStatus? GetMemberStatusByUserId(int userId);

        /// <summary>
        /// Gets all members with their details for display in DataGridView
        /// </summary>
        List<DTOFetchAllMembers> GetAllMembers();

        /// <summary>
        /// Gets member data for editing
        /// </summary>
        DTOEditMember GetMemberForEdit(int memberId);

        /// <summary>
        /// Updates member information
        /// </summary>
        bool UpdateMember(DTOEditMember dto);

        /// <summary>
        /// Updates all Active members whose expiration date has passed to Expired status
        /// </summary>
        void UpdateExpiredMembers();

        /// <summary>
        /// Gets expiration date by UserID
        /// </summary>
        DateTime? GetExpirationDateByUserId(int userId);

        /// <summary>
        /// Updates member status by UserID
        /// </summary>
        bool UpdateMemberStatusByUserId(int userId, MemberStatus status);

        /// <summary>
        /// Renews member's membership - updates registration date to today and expiration date to 1 year from today
        /// </summary>
        bool RenewMembership(int memberId);
    }
}
