using LMS.Presentation.Popup.Inventory;
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
    public partial class UCInventory : UserControl
    {
        public UCInventory()
        {
            InitializeComponent();
        }

        private void BtnAddBook_Click(object sender, EventArgs e)
        {
            AddBook addBookForm = new AddBook();
            addBookForm.ShowDialog();

        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            ImportBook importBookForm = new ImportBook();
            importBookForm.ShowDialog();
        }

        private void LblPaginationPrevious_Click(object sender, EventArgs e)
        {

        }

        // end code
    }
}
