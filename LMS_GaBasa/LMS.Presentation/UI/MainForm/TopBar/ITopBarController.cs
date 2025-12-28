using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LMS.Model.Models.Users;
using LMS.BusinessLogic.Security;

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
        /// Update the visible module title in the top bar (or window title as fallback).
        /// </summary>
        void UpdateModuleTitle(string title, Control topBarContainer);

        void UpdateModuleIcon(string moduleName, PictureBox moduleIconPictureBox);

        // end code
    }
}
