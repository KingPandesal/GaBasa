using LMS.BusinessLogic.Managers;
using LMS.Presentation.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.Presentation
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Presentation only knows about UserManager (BLL)
            IUserManager userManager = new UserManager(); // UserManager internally creates UserRepository
            Application.Run(new Login(userManager));
        }
    }
}
