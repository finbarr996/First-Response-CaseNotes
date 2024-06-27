using System;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes
{
    public partial class CheckListMetadata : Form
    {
        public string CheckName { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public string Version { get; set; }
        public string Size { get; set; }

        public CheckListMetadata()
        {
            InitializeComponent();
        }

        private void CheckListMetadataShown(Object sender, EventArgs e)
        {
            txtName.Text = CheckName;
            txtDescription.Text = Description;
            txtAuthor.Text = Author;
            lblDateCreated.Text = DateCreated;
            lblDateModified.Text = DateModified;
            lblVersion.Text = Version;
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            CheckName = txtName.Text.Trim();
            Description = txtDescription.Text.Trim();
            Author = txtAuthor.Text.Trim();
            Close();
        }
    }
}
