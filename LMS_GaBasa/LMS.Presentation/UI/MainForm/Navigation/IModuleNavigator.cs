using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LMS.Model.Models.Users;
using LMS.BusinessLogic.Security;

namespace LMS.Presentation.UI.MainForm.Navigation
{
    public interface IModuleNavigator
    {
        /// <summary>
        /// Initialize the navigator with the current user and permission service.
        /// </summary>
        void Initialize(User currentUser, IPermissionService permissionService);

        /// <summary>
        /// Register module factories (name -> factory).
        /// </summary>
        void RegisterFactories(IDictionary<string, Func<UserControl>> factories);

        /// <summary>
        /// Create the UserControl instance for the given module name.
        /// Returns a UserControl (fallback may be returned when module not implemented).
        /// </summary>
        UserControl CreateModule(string moduleName);

        /// <summary>
        /// Determine whether the current user is allowed to see/access the named module.
        /// Used by UI to hide/show sidebar entries.
        /// </summary>
        bool IsModuleAllowed(string moduleName);
    }
}
