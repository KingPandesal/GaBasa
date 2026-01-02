using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.User;
using LMS.Model.Models.Users;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace LMS.BusinessLogic.Services.EditUser
{
    public class EditUserService : IEditUserService
    {
        private readonly IUserRepository _userRepo;

        public EditUserService(IUserRepository userRepo)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        }

        public DTOEditUser GetUserForEdit(int userId)
        {
            var user = _userRepo.GetById(userId);
            if (user == null)
                return null;

            return new DTOEditUser
            {
                UserID = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ContactNumber = user.ContactNumber,
                PhotoPath = user.PhotoPath,
                Role = user.Role,
                Status = user.Status
            };
        }

        public UserEditResult UpdateUser(DTOEditUser dto)
        {
            // ===== VALIDATION =====
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                return UserEditResult.Fail("First name is required.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                return UserEditResult.Fail("Last name is required.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return UserEditResult.Fail("Email is required.");

            if (!IsValidEmail(dto.Email))
                return UserEditResult.Fail("Please enter a valid email address.");

            if (string.IsNullOrWhiteSpace(dto.ContactNumber))
                return UserEditResult.Fail("Contact number is required.");

            var contactValidation = ValidateContactNumber(dto.ContactNumber);
            if (!contactValidation.IsValid)
                return UserEditResult.Fail(contactValidation.ErrorMessage);

            // ===== UPDATE USER =====
            bool success = _userRepo.UpdateUser(
                dto.UserID,
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.ContactNumber,
                dto.PhotoPath,
                dto.Role,
                dto.Status
            );

            return success ? UserEditResult.Ok() : UserEditResult.Fail("Failed to update user.");
        }

        private (bool IsValid, string ErrorMessage) ValidateContactNumber(string contactNumber)
        {
            string digitsOnly = new string(contactNumber.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length != contactNumber.Length)
                return (false, "Contact number must contain numbers only.");

            if (digitsOnly.Length != 11)
                return (false, "Contact number must be exactly 11 digits.");

            return (true, null);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
