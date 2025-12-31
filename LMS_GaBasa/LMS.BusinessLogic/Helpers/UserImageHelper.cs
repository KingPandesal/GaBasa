using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Helpers
{
    public static class UserImageHelper
    {
        // Relative path from the executable
        private const string UserImagesFolder = @"Assets\dataimages\Users";

        /// <summary>
        /// Copies the source image to the Users folder and returns the relative path.
        /// </summary>
        /// <param name="sourceImagePath">Absolute path of the selected image</param>
        /// <param name="userId">User ID for unique filename</param>
        /// <returns>Relative path to store in database, or null if failed</returns>
        public static string CopyImageToStorage(string sourceImagePath, int userId)
        {
            if (string.IsNullOrEmpty(sourceImagePath) || !File.Exists(sourceImagePath))
                return null;

            try
            {
                // Get the application's base directory
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string targetFolder = Path.Combine(baseDir, UserImagesFolder);

                // Create directory if it doesn't exist
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                // Create unique filename: UserID_timestamp.extension
                string extension = Path.GetExtension(sourceImagePath);
                string fileName = $"user_{userId}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                string targetPath = Path.Combine(targetFolder, fileName);

                // Copy the file (overwrite if exists)
                File.Copy(sourceImagePath, targetPath, true);

                // Return relative path to store in database
                return Path.Combine(UserImagesFolder, fileName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts a relative path to an absolute path for loading images.
        /// </summary>
        public static string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return null;

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDir, relativePath);
        }

        /// <summary>
        /// Checks if the path is already a relative path (stored in our Assets folder).
        /// </summary>
        public static bool IsRelativePath(string path)
        {
            return !string.IsNullOrEmpty(path) && path.StartsWith(UserImagesFolder);
        }
    }
}
