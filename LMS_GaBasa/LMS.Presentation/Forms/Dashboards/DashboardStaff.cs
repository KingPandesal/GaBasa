using LMS.Model.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.Presentation.Forms.LibraryStaff
{
    public partial class DashboardStaff : Form
    {
        private User _currentUser;

        public DashboardStaff(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LblWelcome.Text = $"Welcome, {_currentUser.FirstName}!";
        }
        private void LblWelcome_Click(object sender, EventArgs e)
        {

        }
    }
}
