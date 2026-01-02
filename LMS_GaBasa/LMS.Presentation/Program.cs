using LMS.BusinessLogic.Hashing;
using LMS.BusinessLogic.Managers;
using LMS.DataAccess.Repositories;
using LMS.DataAccess.Interfaces;
using LMS.Presentation.Forms;
using System;
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

            // Setup dependencies
            IUserRepository userRepo = new UserRepository();
            IMemberRepository memberRepo = new MemberRepository();
            IPasswordHasher hasher = new BcryptPasswordHasher(12);
            IMemberManager memberManager = new MemberManager(memberRepo);
            IUserManager userManager = new UserManager(userRepo, memberManager, hasher);

            Application.Run(new Login(userManager));
        }
    }
}
