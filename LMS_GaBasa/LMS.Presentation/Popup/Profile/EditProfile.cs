using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.Presentation.Popup.Profile
{
    public partial class EditProfile : Form
    {
        public EditProfile()
        {
            InitializeComponent();
        }

        private void LblCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void PicBxProfilePic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select an image";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                PicBxProfilePic.Cursor = Cursors.Hand;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    PicBxProfilePic.Image = Image.FromFile(ofd.FileName);
                    PicBxProfilePic.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }

        }
    }
}
