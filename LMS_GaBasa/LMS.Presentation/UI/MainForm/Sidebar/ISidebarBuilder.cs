using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Presentation.UI.MainForm.Sidebar
{
    internal interface ISidebarBuilder
    {
        /// <summary>
        /// Build sidebar controls inside the provided container.
        /// The builder should clear the container and add module buttons and the logout button.
        /// </summary>
        /// <param name="container">Panel that hosts sidebar content (modules panel + logout button).</param>
        /// <param name="sidebarLayout">Category -> modules layout.</param>
        /// <param name="isModuleAllowed">Predicate to check whether a module should be visible.</param>
        /// <param name="onModuleSelected">Callback invoked when a module button is clicked (moduleName).</param>
        /// <param name="onLogout">Callback invoked when logout is requested.</param>
        void BuildSidebar(
            Panel container,
            IReadOnlyDictionary<string, string[]> sidebarLayout,
            Func<string, bool> isModuleAllowed,
            Action<string> onModuleSelected,
            Action onLogout);
    }
}
