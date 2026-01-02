using LMS.BusinessLogic.Managers;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.User;
using LMS.Model.Models.Enums;
using LMS.Model.Models.Users;
using System;
using System.Linq;
using System.Text.RegularExpressions;

// Alias to resolve conflict between namespace and class
using UserModel = LMS.Model.Models.Users.User;

namespace LMS.BusinessLogic.Services.AddUser
{
    public class AddUserService : IAddUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;

        public AddUserService(IUserRepository userRepo, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public UserCreationResult CreateUser(DTOCreateUser dto)
        {
            // ===== REQUIRED FIELD VALIDATION =====
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                return UserCreationResult.Fail("First name is required.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                return UserCreationResult.Fail("Last name is required.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return UserCreationResult.Fail("Email is required.");

            if (!IsValidEmail(dto.Email))
                return UserCreationResult.Fail("Please enter a valid email address.");

            // ===== CONTACT NUMBER VALIDATION =====
            if (string.IsNullOrWhiteSpace(dto.ContactNumber))
                return UserCreationResult.Fail("Contact number is required.");

            var contactValidation = ValidateContactNumber(dto.ContactNumber);
            if (!contactValidation.IsValid)
                return UserCreationResult.Fail(contactValidation.ErrorMessage);

            // ===== USERNAME VALIDATION =====
            if (string.IsNullOrWhiteSpace(dto.Username))
                return UserCreationResult.Fail("Username is required.");

            if (dto.Username.Length < 8)
                return UserCreationResult.Fail("Username must be at least 8 characters.");

            if (_userRepo.UsernameExists(dto.Username))
                return UserCreationResult.Fail("Username already exists.");

            // ===== PASSWORD VALIDATION =====
            if (string.IsNullOrWhiteSpace(dto.Password))
                return UserCreationResult.Fail("Password is required.");

            var passwordValidation = ValidatePassword(dto.Password);
            if (!passwordValidation.IsValid)
                return UserCreationResult.Fail(passwordValidation.ErrorMessage);

            // ===== CREATE USER =====
            UserModel user = CreateUserByRole(dto.Role);
            user.Username = dto.Username;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.ContactNumber = dto.ContactNumber;
            user.PhotoPath = dto.PhotoPath;
            user.Status = UserStatus.Active;
            user.SetPasswordHash(_passwordHasher.Hash(dto.Password));

            int userId = _userRepo.Add(user);
            return UserCreationResult.Ok(userId);
        }

        private (bool IsValid, string ErrorMessage) ValidateContactNumber(string contactNumber)
        {
            // Remove any spaces or dashes for validation
            string digitsOnly = new string(contactNumber.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length != contactNumber.Length)
                return (false, "Contact number must contain numbers only.");

            if (digitsOnly.Length != 11)
                return (false, "Contact number must be exactly 11 digits.");

            return (true, null);
        }

        private (bool IsValid, string ErrorMessage) ValidatePassword(string password)
        {
            if (password.Length < 8)
                return (false, "Password must be at least 8 characters.");

            if (!password.Any(char.IsUpper))
                return (false, "Password must contain at least 1 uppercase letter.");

            if (!password.Any(char.IsDigit))
                return (false, "Password must contain at least 1 number.");

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
                return (false, "Password must contain at least 1 symbol (e.g., !@#$%^&*).");

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

        private UserModel CreateUserByRole(Role role)
        {
            switch (role)
            {
                case Role.Librarian:
                    return new Librarian();
                case Role.Staff:
                    return new LibraryStaff();
                case Role.Member:
                    return new Member();
                default:
                    throw new ArgumentException($"Unknown role: {role}");
            }
        }
    }
}
