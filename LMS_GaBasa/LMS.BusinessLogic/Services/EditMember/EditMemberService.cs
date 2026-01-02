using LMS.DataAccess.Interfaces;
using LMS.Model.DTOs.Member;
using System;
using System.IO;

namespace LMS.BusinessLogic.Services.EditMember
{
    public class EditMemberService : IEditMemberService
    {
        private readonly IMemberRepository _memberRepo;

        private const string MemberPhotosFolder = "Assets\\dataimages\\Members";
        private const string ValidIdFolder = "Assets\\dataimages\\ValidIDs";

        public EditMemberService(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo ?? throw new ArgumentNullException(nameof(memberRepo));
        }

        public DTOEditMember GetMemberForEdit(int memberId)
        {
            return _memberRepo.GetMemberForEdit(memberId);
        }

        public DTOEditMemberResult UpdateMember(DTOEditMember dto)
        {
            if (dto == null)
                return DTOEditMemberResult.Failure("Invalid member data.");

            if (dto.MemberID <= 0)
                return DTOEditMemberResult.Failure("Invalid member ID.");

            try
            {
                // Handle photo - copy new photo if it's a new file path
                string storedPhotoPath = dto.PhotoPath;
                if (!string.IsNullOrEmpty(dto.PhotoPath) && !IsRelativePath(dto.PhotoPath))
                {
                    storedPhotoPath = CopyImageToStorage(dto.PhotoPath, MemberPhotosFolder, dto.MemberID);
                }

                // Handle valid ID - copy new file if it's a new file path
                string storedValidIdPath = dto.ValidIdPath;
                if (!string.IsNullOrEmpty(dto.ValidIdPath) && !IsRelativePath(dto.ValidIdPath))
                {
                    storedValidIdPath = CopyImageToStorage(dto.ValidIdPath, ValidIdFolder, dto.MemberID);
                }

                // Update DTO with stored paths before saving
                dto.PhotoPath = storedPhotoPath;
                dto.ValidIdPath = storedValidIdPath;

                bool success = _memberRepo.UpdateMember(dto);

                if (success)
                    return DTOEditMemberResult.SuccessResult();
                else
                    return DTOEditMemberResult.Failure("Failed to update member in database.");
            }
            catch (Exception ex)
            {
                return DTOEditMemberResult.Failure($"Error updating member: {ex.Message}");
            }
        }

        private bool IsRelativePath(string path)
        {
            return path.StartsWith("Assets\\", StringComparison.OrdinalIgnoreCase);
        }

        private string CopyImageToStorage(string sourcePath, string destinationFolder, int memberId)
        {
            if (string.IsNullOrEmpty(sourcePath))
                return null;

            string appBasePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullDestinationFolder = Path.Combine(appBasePath, destinationFolder);

            if (!Directory.Exists(fullDestinationFolder))
            {
                Directory.CreateDirectory(fullDestinationFolder);
            }

            string extension = Path.GetExtension(sourcePath);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string newFileName = $"member_{memberId}_{timestamp}{extension}";

            string destinationPath = Path.Combine(fullDestinationFolder, newFileName);

            File.Copy(sourcePath, destinationPath, overwrite: true);

            return Path.Combine(destinationFolder, newFileName);
        }
    }
}
