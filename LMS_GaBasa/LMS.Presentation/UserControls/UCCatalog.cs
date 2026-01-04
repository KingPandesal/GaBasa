using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.Presentation.UserControls
{
    public partial class UCCatalog : UserControl
    {
        public UCCatalog()
        {
            InitializeComponent();

            // Force horizontal scrollbars by setting minimum content width
            // Width = rightmost panel X (1078) + panel width (533) = 1611
            var scrollWidth = new Size(1620, 0);
            panel5.AutoScrollMinSize = scrollWidth;
            PnlPopularBooksSection.AutoScrollMinSize = scrollWidth;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LblBtnSearchLogic_Click(object sender, EventArgs e)
        {

            // Toggle panel visibility
            PnlSearchLogic.Visible = !PnlSearchLogic.Visible;

            // Change arrow to show expanded/collapsed state
            if (PnlSearchLogic.Visible)
                LblBtnSearchLogic.Text = "▼ Search Logic";
            else
                LblBtnSearchLogic.Text = "▶ Search Logic";
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void UCCatalog_Load(object sender, EventArgs e)
        {

        }
    }
}
