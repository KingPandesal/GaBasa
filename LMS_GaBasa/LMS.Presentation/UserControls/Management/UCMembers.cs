using LMS.BusinessLogic.Hashing;
using LMS.BusinessLogic.Services.AddMember;
using LMS.DataAccess.Repositories;
using LMS.Presentation.Popup.Members;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCMembers : UserControl
    {
        private readonly IAddMemberService _memberService;

        public UCMembers()
        {
            InitializeComponent();

            // Setup dependencies (consider using a DI container for larger apps)
            var memberRepo = new MemberRepository();
            var passwordHasher = new BcryptPasswordHasher(12);
            _memberService = new AddMemberService(memberRepo, passwordHasher);
        }

        private void BtnAddMember_Click(object sender, EventArgs e)
        {
            var addMemberForm = new AddMember(_memberService);
            if (addMemberForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh member list after successful add
                LoadMembers();
            }
        }

        private void LoadMembers()
        {
            // TODO: Implement loading members into DataGridView
        }

        // end code
    }
}