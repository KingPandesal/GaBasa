using LMS.BusinessLogic.Managers;
using LMS.BusinessLogic.Security;
using LMS.DataAccess.Repositories;
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

            IUserRepository userRepo = new UserRepository();
            IPasswordHasher hasher = new Sha256PasswordHasher();
            IUserManager userManager = new UserManager(userRepo, hasher);

            Application.Run(new Login(userManager));
        }
    }
}
