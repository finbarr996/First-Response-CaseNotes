namespace FirstResponse.CaseNotes
{
    partial class AboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.labelProductName = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.labelCompanyName = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.textBoxDescription = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelLatestVersion = new System.Windows.Forms.Label();
            this.btnUpdates = new System.Windows.Forms.Button();
            this.lnklblCaseNotes = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelProductName
            // 
            this.labelProductName.BackColor = System.Drawing.SystemColors.Control;
            this.labelProductName.Location = new System.Drawing.Point(4, 154);
            this.labelProductName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(451, 17);
            this.labelProductName.TabIndex = 27;
            this.labelProductName.Text = "Product Name";
            this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVersion
            // 
            this.labelVersion.BackColor = System.Drawing.SystemColors.Control;
            this.labelVersion.Location = new System.Drawing.Point(4, 176);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(451, 17);
            this.labelVersion.TabIndex = 25;
            this.labelVersion.Text = "Version";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCopyright
            // 
            this.labelCopyright.BackColor = System.Drawing.SystemColors.Control;
            this.labelCopyright.Location = new System.Drawing.Point(4, 220);
            this.labelCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(451, 17);
            this.labelCopyright.TabIndex = 28;
            this.labelCopyright.Text = "Copyright";
            this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCompanyName
            // 
            this.labelCompanyName.BackColor = System.Drawing.SystemColors.Control;
            this.labelCompanyName.Location = new System.Drawing.Point(4, 198);
            this.labelCompanyName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelCompanyName.Name = "labelCompanyName";
            this.labelCompanyName.Size = new System.Drawing.Size(451, 17);
            this.labelCompanyName.TabIndex = 29;
            this.labelCompanyName.Text = "Company Name";
            this.labelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(380, 338);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 31;
            this.okButton.Text = "&OK";
            this.okButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBoxDescription.Location = new System.Drawing.Point(6, 278);
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.Size = new System.Drawing.Size(442, 56);
            this.textBoxDescription.TabIndex = 32;
            this.textBoxDescription.Text = "Description";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(5, 262);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(451, 10);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::FirstResponse.CaseNotes.Properties.Resources.CaseNotesLogo;
            this.pictureBox1.Location = new System.Drawing.Point(5, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(454, 142);
            this.pictureBox1.TabIndex = 34;
            this.pictureBox1.TabStop = false;
            // 
            // labelLatestVersion
            // 
            this.labelLatestVersion.BackColor = System.Drawing.SystemColors.Control;
            this.labelLatestVersion.Location = new System.Drawing.Point(4, 242);
            this.labelLatestVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelLatestVersion.Name = "labelLatestVersion";
            this.labelLatestVersion.Size = new System.Drawing.Size(451, 17);
            this.labelLatestVersion.TabIndex = 35;
            this.labelLatestVersion.Text = "Latest version:";
            this.labelLatestVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUpdates
            // 
            this.btnUpdates.Location = new System.Drawing.Point(299, 338);
            this.btnUpdates.Name = "btnUpdates";
            this.btnUpdates.Size = new System.Drawing.Size(75, 23);
            this.btnUpdates.TabIndex = 36;
            this.btnUpdates.Text = "Updates";
            this.btnUpdates.UseVisualStyleBackColor = true;
            this.btnUpdates.Click += new System.EventHandler(this.btnUpdatesClick);
            // 
            // lnklblCaseNotes
            // 
            this.lnklblCaseNotes.AutoSize = true;
            this.lnklblCaseNotes.Location = new System.Drawing.Point(78, 244);
            this.lnklblCaseNotes.Name = "lnklblCaseNotes";
            this.lnklblCaseNotes.Size = new System.Drawing.Size(189, 13);
            this.lnklblCaseNotes.TabIndex = 37;
            this.lnklblCaseNotes.TabStop = true;
            this.lnklblCaseNotes.Text = "https://first-response.co.uk/casenotes";
            this.lnklblCaseNotes.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblCaseNotesLinkClicked);
            // 
            // AboutBox
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size(460, 368);
            this.Controls.Add(this.lnklblCaseNotes);
            this.Controls.Add(this.btnUpdates);
            this.Controls.Add(this.labelLatestVersion);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelProductName);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.labelCompanyName);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.textBoxDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About First Response CaseNotes Professional";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Label labelCompanyName;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.RichTextBox textBoxDescription;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelLatestVersion;
        private System.Windows.Forms.Button btnUpdates;
        private System.Windows.Forms.LinkLabel lnklblCaseNotes;

    }
}
