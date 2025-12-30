using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs;

namespace LMS.BusinessLogic.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserRepository _userRepo;
        //private readonly IMemberRepository _memberRepo;

        public UserProfileService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
            //_memberRepo = memberRepo;
        }

        public DTOUserProfile GetUserProfile(int userId)
        {
            var user = _userRepo.GetById(userId);
            if (user == null) return null;

            return new DTOUserProfile
            {
                UserID = user.UserID,
                Username = user.Username,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                ContactNumber = user.ContactNumber,
                PhotoPath = user.PhotoPath,
                Role = user.Role.ToString(),
                Status = user.Status.ToString()

            };
        }

        //public DTOMemberProfile GetMemberProfile(int userId)
        //{
        //    var user = _userRepo.GetById(userId);
        //    var member = _memberRepo.GetByUserId(userId);
        //    if (user == null || member == null) return null;

        //    return new DTOMemberProfile
        //    {
        //        UserID = user.UserID,
        //        Username = user.Username,
        //        FullName = $"{user.FirstName} {user.LastName}",
        //        Email = user.Email,
        //        ContactNumber = user.ContactNumber,
        //        PhotoPath = user.PhotoPath,
        //        MemberTypeID = member.MemberTypeID,
        //        TypeName = member.MemberType.TypeName,
        //        MaxBooksAllowed = member.MemberType.MaxBooksAllowed,
        //        BorrowingPeriod = member.MemberType.BorrowingPeriod,
        //        RenewalLimit = member.MemberType.RenewalLimit,
        //        ReservationPrivilege = member.MemberType.ReservationPrivilege,
        //        FineRate = member.MemberType.FineRate
        //    };
        //}
    }
}
