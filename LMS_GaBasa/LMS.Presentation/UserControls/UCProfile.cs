using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LMS.Model.Models.Users;

namespace LMS.Presentation.UserControls
{
    public partial class UCProfile : UserControl
    {
        private readonly User _currentUser;

        public UCProfile()
        {
            InitializeComponent();
        }

        public UCProfile(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            //LblFullname.Text = _currentUser.FullName;
            LblRole.Text = _currentUser.Role.ToString();

            // Role-specific UI
            ConfigureRoleSpecificUI();
        }

        private void ConfigureRoleSpecificUI()
        {
            switch (_currentUser.Role)
            {
                case Role.Member:
                    PnlMemberPrivilege.Visible = true;  // show for member
                    PnlRegExpDate.Visible = true;
                    break;
                default:
                    PnlMemberPrivilege.Visible = false; // hide for Librarian & Staff
                    PnlRegExpDate.Visible = false;
                    break;
            }
        }

        private void UCProfile_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
