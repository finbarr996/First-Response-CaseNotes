using System;

namespace FirstResponse.CaseNotes
{
    partial class CheckList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckList));
            this.leftPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.picListBox = new System.Windows.Forms.PictureBox();
            this.picTextBox = new System.Windows.Forms.PictureBox();
            this.picRadioButton = new System.Windows.Forms.PictureBox();
            this.picPictureBox = new System.Windows.Forms.PictureBox();
            this.picLabel = new System.Windows.Forms.PictureBox();
            this.picComboBox = new System.Windows.Forms.PictureBox();
            this.picCheckBox = new System.Windows.Forms.PictureBox();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblDateModified = new System.Windows.Forms.Label();
            this.lblDateCreated = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnComboUpdate = new System.Windows.Forms.Button();
            this.txtBoxMulti = new System.Windows.Forms.TextBox();
            this.lblMulti = new System.Windows.Forms.Label();
            this.txtText = new System.Windows.Forms.TextBox();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.centerPanel = new System.Windows.Forms.Panel();
            this.checkListPanel = new System.Windows.Forms.Panel();
            this.leftPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picListBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRadioButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckBox)).BeginInit();
            this.rightPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.centerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // leftPanel
            // 
            this.leftPanel.BackColor = System.Drawing.Color.White;
            this.leftPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.leftPanel.Controls.Add(this.groupBox1);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(154, 756);
            this.leftPanel.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.picListBox);
            this.groupBox1.Controls.Add(this.picTextBox);
            this.groupBox1.Controls.Add(this.picRadioButton);
            this.groupBox1.Controls.Add(this.picPictureBox);
            this.groupBox1.Controls.Add(this.picLabel);
            this.groupBox1.Controls.Add(this.picComboBox);
            this.groupBox1.Controls.Add(this.picCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(8, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(135, 268);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Toolbox";
            // 
            // picListBox
            // 
            this.picListBox.Image = global::FirstResponse.CaseNotes.Properties.Resources.listbox;
            this.picListBox.InitialImage = global::FirstResponse.CaseNotes.Properties.Resources.combobox;
            this.picListBox.Location = new System.Drawing.Point(11, 125);
            this.picListBox.Name = "picListBox";
            this.picListBox.Size = new System.Drawing.Size(112, 21);
            this.picListBox.TabIndex = 7;
            this.picListBox.TabStop = false;
            this.picListBox.Click += new System.EventHandler(this.PicListBoxClick);
            // 
            // picTextBox
            // 
            this.picTextBox.Image = global::FirstResponse.CaseNotes.Properties.Resources.textbox;
            this.picTextBox.InitialImage = global::FirstResponse.CaseNotes.Properties.Resources.textbox;
            this.picTextBox.Location = new System.Drawing.Point(10, 227);
            this.picTextBox.Name = "picTextBox";
            this.picTextBox.Size = new System.Drawing.Size(112, 21);
            this.picTextBox.TabIndex = 6;
            this.picTextBox.TabStop = false;
            this.picTextBox.Click += new System.EventHandler(this.PicTextBoxClick);
            // 
            // picRadioButton
            // 
            this.picRadioButton.Image = global::FirstResponse.CaseNotes.Properties.Resources.radiobutton;
            this.picRadioButton.InitialImage = global::FirstResponse.CaseNotes.Properties.Resources.radiobutton;
            this.picRadioButton.Location = new System.Drawing.Point(10, 193);
            this.picRadioButton.Name = "picRadioButton";
            this.picRadioButton.Size = new System.Drawing.Size(112, 21);
            this.picRadioButton.TabIndex = 5;
            this.picRadioButton.TabStop = false;
            this.picRadioButton.Click += new System.EventHandler(this.PicRadioButtonClick);
            // 
            // picPictureBox
            // 
            this.picPictureBox.AccessibleDescription = "";
            this.picPictureBox.Image = global::FirstResponse.CaseNotes.Properties.Resources.picturebox;
            this.picPictureBox.InitialImage = global::FirstResponse.CaseNotes.Properties.Resources.picturebox;
            this.picPictureBox.Location = new System.Drawing.Point(10, 159);
            this.picPictureBox.Name = "picPictureBox";
            this.picPictureBox.Size = new System.Drawing.Size(112, 21);
            this.picPictureBox.TabIndex = 4;
            this.picPictureBox.TabStop = false;
            this.picPictureBox.Click += new System.EventHandler(this.PicPictureBoxClick);
            // 
            // picLabel
            // 
            this.picLabel.Image = global::FirstResponse.CaseNotes.Properties.Resources.label;
            this.picLabel.InitialImage = global::FirstResponse.CaseNotes.Properties.Resources.label;
            this.picLabel.Location = new System.Drawing.Point(10, 23);
            this.picLabel.Name = "picLabel";
            this.picLabel.Size = new System.Drawing.Size(112, 21);
            this.picLabel.TabIndex = 3;
            this.picLabel.TabStop = false;
            this.picLabel.Click += new System.EventHandler(this.PicLabelClick);
            // 
            // picComboBox
            // 
            this.picComboBox.Image = global::FirstResponse.CaseNotes.Properties.Resources.combobox;
            this.picComboBox.InitialImage = global::FirstResponse.CaseNotes.Properties.Resources.combobox;
            this.picComboBox.Location = new System.Drawing.Point(10, 91);
            this.picComboBox.Name = "picComboBox";
            this.picComboBox.Size = new System.Drawing.Size(112, 21);
            this.picComboBox.TabIndex = 2;
            this.picComboBox.TabStop = false;
            this.picComboBox.Click += new System.EventHandler(this.PicComboBoxClick);
            // 
            // picCheckBox
            // 
            this.picCheckBox.Image = global::FirstResponse.CaseNotes.Properties.Resources.checkbox;
            this.picCheckBox.InitialImage = global::FirstResponse.CaseNotes.Properties.Resources.checkbox;
            this.picCheckBox.Location = new System.Drawing.Point(10, 57);
            this.picCheckBox.Name = "picCheckBox";
            this.picCheckBox.Size = new System.Drawing.Size(112, 21);
            this.picCheckBox.TabIndex = 1;
            this.picCheckBox.TabStop = false;
            this.picCheckBox.Click += new System.EventHandler(this.PicCheckBoxClick);
            // 
            // rightPanel
            // 
            this.rightPanel.BackColor = System.Drawing.Color.White;
            this.rightPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rightPanel.Controls.Add(this.lblAuthor);
            this.rightPanel.Controls.Add(this.lblDescription);
            this.rightPanel.Controls.Add(this.lblName);
            this.rightPanel.Controls.Add(this.lblVersion);
            this.rightPanel.Controls.Add(this.lblDateModified);
            this.rightPanel.Controls.Add(this.lblDateCreated);
            this.rightPanel.Controls.Add(this.label6);
            this.rightPanel.Controls.Add(this.label3);
            this.rightPanel.Controls.Add(this.label7);
            this.rightPanel.Controls.Add(this.label8);
            this.rightPanel.Controls.Add(this.label9);
            this.rightPanel.Controls.Add(this.label10);
            this.rightPanel.Controls.Add(this.btnOpen);
            this.rightPanel.Controls.Add(this.btnCancel);
            this.rightPanel.Controls.Add(this.btnSave);
            this.rightPanel.Controls.Add(this.groupBox2);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightPanel.Location = new System.Drawing.Point(1004, 0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(276, 756);
            this.rightPanel.TabIndex = 1;
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Location = new System.Drawing.Point(71, 534);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(37, 13);
            this.lblAuthor.TabIndex = 30;
            this.lblAuthor.Text = "author";
            this.lblAuthor.Visible = false;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(71, 507);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(58, 13);
            this.lblDescription.TabIndex = 29;
            this.lblDescription.Text = "description";
            this.lblDescription.Visible = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(71, 481);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(33, 13);
            this.lblName.TabIndex = 28;
            this.lblName.Text = "name";
            this.lblName.Visible = false;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(71, 614);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(41, 13);
            this.lblVersion.TabIndex = 27;
            this.lblVersion.Text = "version";
            this.lblVersion.Visible = false;
            // 
            // lblDateModified
            // 
            this.lblDateModified.AutoSize = true;
            this.lblDateModified.Location = new System.Drawing.Point(71, 587);
            this.lblDateModified.Name = "lblDateModified";
            this.lblDateModified.Size = new System.Drawing.Size(46, 13);
            this.lblDateModified.TabIndex = 26;
            this.lblDateModified.Text = "modified";
            this.lblDateModified.Visible = false;
            // 
            // lblDateCreated
            // 
            this.lblDateCreated.AutoSize = true;
            this.lblDateCreated.Location = new System.Drawing.Point(71, 561);
            this.lblDateCreated.Name = "lblDateCreated";
            this.lblDateCreated.Size = new System.Drawing.Size(43, 13);
            this.lblDateCreated.TabIndex = 25;
            this.lblDateCreated.Text = "created";
            this.lblDateCreated.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 614);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Version:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 587);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Last Modified:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 561);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Date Created:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(32, 534);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Author:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 507);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Description:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 481);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Checklist:";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(59, 433);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(59, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpenClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(203, 433);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Exit";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(131, 433);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(59, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnBrowse);
            this.groupBox2.Controls.Add(this.btnComboUpdate);
            this.groupBox2.Controls.Add(this.txtBoxMulti);
            this.groupBox2.Controls.Add(this.lblMulti);
            this.groupBox2.Controls.Add(this.txtText);
            this.groupBox2.Controls.Add(this.txtSize);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtLocation);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(4, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(265, 403);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Properties";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(208, 340);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(50, 21);
            this.btnBrowse.TabIndex = 17;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Visible = false;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowseClick);
            // 
            // btnComboUpdate
            // 
            this.btnComboUpdate.Location = new System.Drawing.Point(154, 340);
            this.btnComboUpdate.Name = "btnComboUpdate";
            this.btnComboUpdate.Size = new System.Drawing.Size(50, 21);
            this.btnComboUpdate.TabIndex = 16;
            this.btnComboUpdate.Text = "Update";
            this.btnComboUpdate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnComboUpdate.UseVisualStyleBackColor = true;
            this.btnComboUpdate.Visible = false;
            this.btnComboUpdate.Click += new System.EventHandler(this.BtnComboUpdateClick);
            // 
            // txtBoxMulti
            // 
            this.txtBoxMulti.Location = new System.Drawing.Point(82, 164);
            this.txtBoxMulti.Multiline = true;
            this.txtBoxMulti.Name = "txtBoxMulti";
            this.txtBoxMulti.Size = new System.Drawing.Size(176, 170);
            this.txtBoxMulti.TabIndex = 15;
            this.txtBoxMulti.TextChanged += new System.EventHandler(this.TxtBoxMultiTextChanged);
            // 
            // lblMulti
            // 
            this.lblMulti.AutoSize = true;
            this.lblMulti.Location = new System.Drawing.Point(26, 167);
            this.lblMulti.Name = "lblMulti";
            this.lblMulti.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMulti.Size = new System.Drawing.Size(54, 13);
            this.lblMulti.TabIndex = 14;
            this.lblMulti.Text = "Text Size:";
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(82, 129);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(176, 20);
            this.txtText.TabIndex = 13;
            this.txtText.TextChanged += new System.EventHandler(this.TxtTextTextChanged);
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(82, 95);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(176, 20);
            this.txtSize.TabIndex = 9;
            this.txtSize.TextChanged += new System.EventHandler(this.TxtSizeTextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(49, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Text:";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(82, 61);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(176, 20);
            this.txtLocation.TabIndex = 7;
            this.txtLocation.TextChanged += new System.EventHandler(this.TxtLocationTextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Location (x,y):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Size (x,y):";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(82, 27);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(176, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // centerPanel
            // 
            this.centerPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.centerPanel.Controls.Add(this.checkListPanel);
            this.centerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerPanel.Location = new System.Drawing.Point(154, 0);
            this.centerPanel.Name = "centerPanel";
            this.centerPanel.Padding = new System.Windows.Forms.Padding(20);
            this.centerPanel.Size = new System.Drawing.Size(850, 756);
            this.centerPanel.TabIndex = 2;
            // 
            // checkListPanel
            // 
            this.checkListPanel.AllowDrop = true;
            this.checkListPanel.BackColor = System.Drawing.Color.White;
            this.checkListPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.checkListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkListPanel.Location = new System.Drawing.Point(20, 20);
            this.checkListPanel.Name = "checkListPanel";
            this.checkListPanel.Padding = new System.Windows.Forms.Padding(20);
            this.checkListPanel.Size = new System.Drawing.Size(810, 716);
            this.checkListPanel.TabIndex = 0;
            this.checkListPanel.Click += new System.EventHandler(this.CheckListPanelClick);
            // 
            // CheckList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 756);
            this.Controls.Add(this.centerPanel);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.leftPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CheckList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CaseNotes Checklist Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.leftPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picListBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRadioButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckBox)).EndInit();
            this.rightPanel.ResumeLayout(false);
            this.rightPanel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.centerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Panel centerPanel;
        private System.Windows.Forms.PictureBox picTextBox;
        private System.Windows.Forms.PictureBox picRadioButton;
        private System.Windows.Forms.PictureBox picPictureBox;
        private System.Windows.Forms.PictureBox picLabel;
        private System.Windows.Forms.PictureBox picComboBox;
        private System.Windows.Forms.PictureBox picCheckBox;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel checkListPanel;
        private System.Windows.Forms.TextBox txtBoxMulti;
        private System.Windows.Forms.Label lblMulti;
        private System.Windows.Forms.Button btnComboUpdate;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.PictureBox picListBox;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblDateModified;
        private System.Windows.Forms.Label lblDateCreated;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}