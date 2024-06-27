using System;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes
{
    public partial class Password : Form
    {
        public string DBPassword = "";

        public Password()
        {
            InitializeComponent();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            DBPassword = txtPassword.Text.Trim();
            Close();
        }
    }
}
