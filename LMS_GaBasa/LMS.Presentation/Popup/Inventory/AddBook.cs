using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Inventory
{
    public partial class AddBook : Form
    {
        public AddBook()
        {
            InitializeComponent();
        }

        private void LblFirstName_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void AddBook_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void RdoBtnPhysicalBook_CheckedChanged(object sender, EventArgs e)
        {
            PnlforRdoBtnPhysicalBooks.Visible = true;
            PnlforRdoBtnEBook.Visible = false;
        }

        private void RdoBtnEBook_CheckedChanged(object sender, EventArgs e)
        {
            PnlforRdoBtnEBook.Visible = true;
            PnlforRdoBtnPhysicalBooks.Visible = false;
        }

        private void RdoBtnTheses_CheckedChanged(object sender, EventArgs e)
        {
            PnlforRdoBtnEBook.Visible = false;
            PnlforRdoBtnPhysicalBooks.Visible = false;
        }

        private void RdoBtnPeriodical_CheckedChanged(object sender, EventArgs e)
        {
            PnlforRdoBtnEBook.Visible = false;
            PnlforRdoBtnPhysicalBooks.Visible = false;
        }

        private void RdoBtnAV_CheckedChanged(object sender, EventArgs e)
        {
            PnlforRdoBtnEBook.Visible = false;
            PnlforRdoBtnPhysicalBooks.Visible = false;
        }
    }
}
