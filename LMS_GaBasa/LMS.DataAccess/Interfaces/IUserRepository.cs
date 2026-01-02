using LMS.Model.Models.Users;
using System.Collections.Generic;

namespace LMS.DataAccess.Repositories
{
    public interface IUserRepository
    {
        // Fetch a user by username
        User GetByUsername(string username);

        // Fetch a user by ID
        User GetById(int userId);

        // Update user profile
        bool UpdateProfile(int userId, string firstName, string lastName, string email, string contactNumber, string photoPath);

        int Add(User user);
        bool UsernameExists(string username);
        
        // New method for listing users (excluding Members for staff management)
        List<User> GetAllStaffUsers();
    }
}
