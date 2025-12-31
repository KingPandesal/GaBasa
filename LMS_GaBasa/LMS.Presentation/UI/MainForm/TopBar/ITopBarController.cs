using System;
using System.Windows.Forms;
using LMS.Model.Models.Users;
using LMS.BusinessLogic.Security;
using LMS.Model.DTOs.User;

namespace LMS.Presentation.UI.MainForm.TopBar
{
    internal interface ITopBarController
    {
        /// <summary>
        /// Initialize profile area: set labels, picture and hook click callbacks.
        /// Keep logic UI-focused (no business rules).
        /// </summary>
        void InitializeProfile(User currentUser, IPermissionService permissionService,
            PictureBox profilePictureBox, Label profileNameLabel, Label profileRoleLabel, Control profileHeader,
            Action onOpenProfile);

        /// <summary>
        /// Refresh the profile display after changes (e.g., after editing profile).
        /// </summary>
        void RefreshProfile(User currentUser, PictureBox profilePictureBox,
            Label profileNameLabel, Label profileRoleLabel);

        /// <summary>
        /// Refresh the profile display using updated profile data from the database.
        /// </summary>
        void RefreshProfile(DTOUserProfile profile, PictureBox profilePictureBox,
            Label profileNameLabel, Label profileRoleLabel);

        /// <summary>
        /// Update the visible module title in the top bar (or window title as fallback).
        /// </summary>
        void UpdateModuleTitle(string title, Control topBarContainer);

        void UpdateModuleIcon(string moduleName, PictureBox moduleIconPictureBox);

        // end code
    }
}
