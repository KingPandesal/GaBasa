using LMS.Presentation.Popup.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCUsers : UserControl
    {
        public UCUsers()
        {
            InitializeComponent();
        }

        // for testing only (add data)
        private void UCUsers_Load(object sender, EventArgs e)
        {
            // Add data
            String[] row;
            row = new string[] { "kennnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn", "22"};
            DgwUsers.Rows.Add(row);
        }

        private void DgwUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // dri ibutang and handle click sa edit ug delete
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            AddUser addUserForm = new AddUser();
            addUserForm.ShowDialog(); // Use ShowDialog() to open as modal
        }

        // end code
    }
}
