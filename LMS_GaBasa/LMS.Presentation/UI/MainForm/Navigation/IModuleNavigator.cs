using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.Presentation.UI.MainForm.Navigation
{
    internal interface IModuleNavigator
    {
        /// <summary>
        /// Register module factories (name -> factory).
        /// </summary>
        void RegisterFactories(IDictionary<string, Func<UserControl>> factories);

        /// <summary>
        /// Create the UserControl instance for the given module name.
        /// Returns null if not found; caller may show a fallback.
        /// </summary>
        UserControl CreateModule(string moduleName);
    }
}
