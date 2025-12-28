using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LMS.Presentation.UI.MainForm.Sidebar
{
    internal interface ISidebarBuilder
    {
        /// <summary>
        /// Build sidebar UI inside the provided container.
        /// The builder is responsible for clearing the container and adding module buttons and a logout button.
        /// </summary>
        /// <param name="container">Panel that hosts sidebar content.</param>
        /// <param name="sidebarLayout">Category -> modules layout.</param>
        /// <param name="onModuleSelected">Callback invoked with module name when a module button is clicked.</param>
        /// <param name="onLogout">Callback invoked when the logout control is clicked.</param>
        /// <param name="initialSelectedModule">Optional module name that should be shown as selected after build.</param>
        void BuildSidebar(
            Panel container,
            IReadOnlyDictionary<string, string[]> sidebarLayout,
            Action<string> onModuleSelected,
            Action onLogout,
            string initialSelectedModule);
    }
}
