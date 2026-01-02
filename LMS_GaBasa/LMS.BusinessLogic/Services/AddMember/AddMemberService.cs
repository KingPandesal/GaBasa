using LMS.BusinessLogic.Managers;
using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Member;
using System;
using System.IO;

namespace LMS.BusinessLogic.Services.AddMember
{
    public class AddMemberService : IAddMemberService
    {
        private readonly IMemberRepository _memberRepo;
        private readonly IPasswordHasher _passwordHasher;

        private const string MemberPhotosFolder = "Assets\\dataimages\\Members";
        private const string ValidIdFolder = "Assets\\dataimages\\ValidIDs";

        public AddMemberService(IMemberRepository memberRepo, IPasswordHasher passwordHasher)
        {
            _memberRepo = memberRepo ?? throw new ArgumentNullException(nameof(memberRepo));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public DTOCreateMemberResult CreateMember(DTOCreateMember dto)
        {
            // Check if username already exists
            if (_memberRepo.UsernameExists(dto.Username))
            {
                return DTOCreateMemberResult.Failure("Username already exists.");
            }

            // Get member type ID
            int? memberTypeId = _memberRepo.GetMemberTypeIdByName(dto.MemberTypeName);
            if (!memberTypeId.HasValue)
            {
                return DTOCreateMemberResult.Failure("Invalid member type selected.");
            }

            try
            {
                // Copy images to storage
                string photoRelativePath = CopyImageToStorage(dto.PhotoPath, MemberPhotosFolder);
                string validIdRelativePath = CopyImageToStorage(dto.ValidIdPath, ValidIdFolder);

                // Hash the password
                string passwordHash = _passwordHasher.Hash(dto.Password);

                // Create member in database
                int userId = _memberRepo.AddMember(
                    dto.FirstName,
                    dto.LastName,
                    dto.Email,
                    dto.ContactNumber,
                    dto.Username,
                    passwordHash,
                    photoRelativePath,
                    dto.Address,
                    validIdRelativePath,
                    memberTypeId.Value
                );

                return DTOCreateMemberResult.SuccessResult(userId);
            }
            catch (Exception ex)
            {
                return DTOCreateMemberResult.Failure($"Failed to create member: {ex.Message}");
            }
        }

        private string CopyImageToStorage(string sourcePath, string destinationFolder)
        {
            if (string.IsNullOrEmpty(sourcePath))
                return null;

            // Use AppDomain.CurrentDomain.BaseDirectory instead of Application.StartupPath
            string appBasePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullDestinationFolder = Path.Combine(appBasePath, destinationFolder);

            // Create directory if it doesn't exist
            if (!Directory.Exists(fullDestinationFolder))
            {
                Directory.CreateDirectory(fullDestinationFolder);
            }

            // Generate unique filename
            string extension = Path.GetExtension(sourcePath);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
            string newFileName = $"member_{timestamp}_{uniqueId}{extension}";

            string destinationPath = Path.Combine(fullDestinationFolder, newFileName);

            // Copy the file
            File.Copy(sourcePath, destinationPath, overwrite: true);

            // Return relative path for database storage
            return Path.Combine(destinationFolder, newFileName);
        }
    }
}
