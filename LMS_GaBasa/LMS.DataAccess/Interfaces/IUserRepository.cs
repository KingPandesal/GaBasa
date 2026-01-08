using LMS.Model.Models.Users;
using System.Collections.Generic;
using LMS.Model.Models.Enums;

namespace LMS.DataAccess.Repositories
{
    public interface IUserRepository
    {
        // Fetch a user by username
        User GetByUsername(string username);

        // Fetch a user by ID
        User GetById(int userId);

        // Update user profile (now includes username)
        bool UpdateProfile(int userId, string firstName, string lastName, string email, string contactNumber, string photoPath, string username);

        int Add(User user);
        bool UsernameExists(string username);
        
        // New method for listing users (excluding Members for staff management)
        List<User> GetAllStaffUsers();
        
        // New method for updating user (including role)
        bool UpdateUser(int userId, 
            string firstName, 
            string lastName, 
            string email, 
            string contactNumber, 
            string photoPath, 
            Role role, 
            UserStatus status);

        // New methods to support password verification/update
        string GetPasswordHash(int userId);
        bool UpdatePassword(int userId, string newPasswordHash);

        // New method for archiving (soft delete)
        bool ArchiveUser(int userId);

        // Update last login timestamp
        bool UpdateLastLogin(int userId);
    }
}
