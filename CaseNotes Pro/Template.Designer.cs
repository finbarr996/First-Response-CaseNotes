namespace FirstResponse.CaseNotes
{
	partial class Template
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Template));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.boldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.italicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.underlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.NoteToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripCutButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripCopyButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripPasteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBarBold = new System.Windows.Forms.ToolStripButton();
            this.toolBarItalic = new System.Windows.Forms.ToolStripButton();
            this.toolBarUnder = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripSeparator();
            this.TextLeft = new System.Windows.Forms.ToolStripButton();
            this.TextCenter = new System.Windows.Forms.ToolStripButton();
            this.TextRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripSeparator();
            this.BulletList = new System.Windows.Forms.ToolStripButton();
            this.Outdent = new System.Windows.Forms.ToolStripButton();
            this.Indent = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripSeparator();
            this.FontButton = new System.Windows.Forms.ToolStripComboBox();
            this.SizeButton = new System.Windows.Forms.ToolStripComboBox();
            this.FontColour = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbNotesRuler = new System.Windows.Forms.ToolStripButton();
            this.tsbNotesSpell = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.NoteToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(742, 27);
            this.panel1.TabIndex = 200;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(283, 13);
            this.label1.TabIndex = 150;
            this.label1.Text = "Enter the template text for your notes in the text box below:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 416);
            this.panel2.Name = "panel2";
            this.panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel2.Size = new System.Drawing.Size(742, 39);
            this.panel2.TabIndex = 100;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boldToolStripMenuItem,
            this.italicToolStripMenuItem,
            this.underlineToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(14, 9);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(164, 24);
            this.menuStrip1.TabIndex = 151;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // boldToolStripMenuItem
            // 
            this.boldToolStripMenuItem.Name = "boldToolStripMenuItem";
            this.boldToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.boldToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.boldToolStripMenuItem.Text = "bold";
            this.boldToolStripMenuItem.Click += new System.EventHandler(this.BoldToolStripMenuItemClick);
            // 
            // italicToolStripMenuItem
            // 
            this.italicToolStripMenuItem.Name = "italicToolStripMenuItem";
            this.italicToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.italicToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.italicToolStripMenuItem.Text = "italic";
            this.italicToolStripMenuItem.Click += new System.EventHandler(this.ItalicToolStripMenuItemClick);
            // 
            // underlineToolStripMenuItem
            // 
            this.underlineToolStripMenuItem.Name = "underlineToolStripMenuItem";
            this.underlineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.underlineToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.underlineToolStripMenuItem.Text = "underline";
            this.underlineToolStripMenuItem.Click += new System.EventHandler(this.UnderlineToolStripMenuItemClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(659, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(578, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 52);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(742, 364);
            this.panel3.TabIndex = 3;
            // 
            // NoteToolStrip
            // 
            this.NoteToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCutButton,
            this.toolStripCopyButton,
            this.toolStripPasteButton,
            this.toolStripButton7,
            this.toolBarBold,
            this.toolBarItalic,
            this.toolBarUnder,
            this.toolStripButton8,
            this.TextLeft,
            this.TextCenter,
            this.TextRight,
            this.toolStripButton2,
            this.BulletList,
            this.Outdent,
            this.Indent,
            this.toolStripButton6,
            this.FontButton,
            this.SizeButton,
            this.FontColour,
            this.toolStripSeparator2,
            this.tsbNotesRuler,
            this.tsbNotesSpell});
            this.NoteToolStrip.Location = new System.Drawing.Point(0, 0);
            this.NoteToolStrip.Name = "NoteToolStrip";
            this.NoteToolStrip.Size = new System.Drawing.Size(742, 25);
            this.NoteToolStrip.TabIndex = 201;
            this.NoteToolStrip.Text = "toolStrip1";
            // 
            // toolStripCutButton
            // 
            this.toolStripCutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripCutButton.Image = global::FirstResponse.CaseNotes.Properties.Resources.CutHS;
            this.toolStripCutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCutButton.Name = "toolStripCutButton";
            this.toolStripCutButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripCutButton.Text = "Cut";
            this.toolStripCutButton.Click += new System.EventHandler(this.ToolStripCutButtonClick);
            // 
            // toolStripCopyButton
            // 
            this.toolStripCopyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripCopyButton.Image = global::FirstResponse.CaseNotes.Properties.Resources.CopyHS;
            this.toolStripCopyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCopyButton.Name = "toolStripCopyButton";
            this.toolStripCopyButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripCopyButton.Text = "Copy";
            this.toolStripCopyButton.Click += new System.EventHandler(this.ToolStripCopyButtonClick);
            // 
            // toolStripPasteButton
            // 
            this.toolStripPasteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripPasteButton.Image = global::FirstResponse.CaseNotes.Properties.Resources.PasteHS;
            this.toolStripPasteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPasteButton.Name = "toolStripPasteButton";
            this.toolStripPasteButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripPasteButton.Text = "Paste";
            this.toolStripPasteButton.Click += new System.EventHandler(this.ToolStripPasteButtonClick);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolBarBold
            // 
            this.toolBarBold.CheckOnClick = true;
            this.toolBarBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBarBold.Image = global::FirstResponse.CaseNotes.Properties.Resources.boldhs;
            this.toolBarBold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBarBold.Name = "toolBarBold";
            this.toolBarBold.Size = new System.Drawing.Size(23, 22);
            this.toolBarBold.Text = "Bold";
            this.toolBarBold.Click += new System.EventHandler(this.FormatTextButton);
            // 
            // toolBarItalic
            // 
            this.toolBarItalic.CheckOnClick = true;
            this.toolBarItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBarItalic.Image = global::FirstResponse.CaseNotes.Properties.Resources.ItalicHS;
            this.toolBarItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBarItalic.Name = "toolBarItalic";
            this.toolBarItalic.Size = new System.Drawing.Size(23, 22);
            this.toolBarItalic.Text = "Italics";
            this.toolBarItalic.Click += new System.EventHandler(this.FormatTextButton);
            // 
            // toolBarUnder
            // 
            this.toolBarUnder.CheckOnClick = true;
            this.toolBarUnder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBarUnder.Image = global::FirstResponse.CaseNotes.Properties.Resources.UnderlineHS;
            this.toolBarUnder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBarUnder.Name = "toolBarUnder";
            this.toolBarUnder.Size = new System.Drawing.Size(23, 22);
            this.toolBarUnder.Text = "Underline";
            this.toolBarUnder.Click += new System.EventHandler(this.FormatTextButton);
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(6, 25);
            // 
            // TextLeft
            // 
            this.TextLeft.CheckOnClick = true;
            this.TextLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TextLeft.Image = global::FirstResponse.CaseNotes.Properties.Resources.AlignTableCellLeftHS;
            this.TextLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TextLeft.Name = "TextLeft";
            this.TextLeft.Size = new System.Drawing.Size(23, 22);
            this.TextLeft.Text = "Align left";
            this.TextLeft.Click += new System.EventHandler(this.TextLeftClick);
            // 
            // TextCenter
            // 
            this.TextCenter.CheckOnClick = true;
            this.TextCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TextCenter.Image = global::FirstResponse.CaseNotes.Properties.Resources.AlignTableCellMiddleCenterHS;
            this.TextCenter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TextCenter.Name = "TextCenter";
            this.TextCenter.Size = new System.Drawing.Size(23, 22);
            this.TextCenter.Text = "Align center";
            this.TextCenter.Click += new System.EventHandler(this.TextCenterClick);
            // 
            // TextRight
            // 
            this.TextRight.CheckOnClick = true;
            this.TextRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TextRight.Image = global::FirstResponse.CaseNotes.Properties.Resources.AlignTableCellMiddleRightHS;
            this.TextRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TextRight.Name = "TextRight";
            this.TextRight.Size = new System.Drawing.Size(23, 22);
            this.TextRight.Text = "Align right";
            this.TextRight.Click += new System.EventHandler(this.TextRightClick);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(6, 25);
            // 
            // BulletList
            // 
            this.BulletList.CheckOnClick = true;
            this.BulletList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BulletList.Image = global::FirstResponse.CaseNotes.Properties.Resources.List_BulletsHS;
            this.BulletList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BulletList.Name = "BulletList";
            this.BulletList.Size = new System.Drawing.Size(23, 22);
            this.BulletList.Text = "Bullet List";
            this.BulletList.Click += new System.EventHandler(this.BulletListClick);
            // 
            // Outdent
            // 
            this.Outdent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Outdent.Image = global::FirstResponse.CaseNotes.Properties.Resources.OutdentHS;
            this.Outdent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Outdent.Name = "Outdent";
            this.Outdent.Size = new System.Drawing.Size(23, 22);
            this.Outdent.Text = "Outdent selection";
            this.Outdent.Click += new System.EventHandler(this.OutdentClick);
            // 
            // Indent
            // 
            this.Indent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Indent.Image = global::FirstResponse.CaseNotes.Properties.Resources.IndentHS;
            this.Indent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Indent.Name = "Indent";
            this.Indent.Size = new System.Drawing.Size(23, 22);
            this.Indent.Text = "Indent Selection";
            this.Indent.Click += new System.EventHandler(this.IndentClick);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(6, 25);
            // 
            // FontButton
            // 
            this.FontButton.Name = "FontButton";
            this.FontButton.Size = new System.Drawing.Size(160, 25);
            this.FontButton.Text = "Select a typeface";
            this.FontButton.SelectedIndexChanged += new System.EventHandler(this.FontButtonSelectedIndexChanged);
            // 
            // SizeButton
            // 
            this.SizeButton.AutoSize = false;
            this.SizeButton.DropDownWidth = 30;
            this.SizeButton.Name = "SizeButton";
            this.SizeButton.Size = new System.Drawing.Size(50, 23);
            this.SizeButton.Text = "Select a size";
            this.SizeButton.SelectedIndexChanged += new System.EventHandler(this.SizeButtonSelectedIndexChanged);
            // 
            // FontColour
            // 
            this.FontColour.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FontColour.Image = global::FirstResponse.CaseNotes.Properties.Resources.Color_font;
            this.FontColour.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FontColour.Name = "FontColour";
            this.FontColour.Size = new System.Drawing.Size(23, 22);
            this.FontColour.Text = "Font colour";
            this.FontColour.Click += new System.EventHandler(this.FontColourClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbNotesRuler
            // 
            this.tsbNotesRuler.Checked = true;
            this.tsbNotesRuler.CheckOnClick = true;
            this.tsbNotesRuler.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbNotesRuler.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNotesRuler.Image = global::FirstResponse.CaseNotes.Properties.Resources.ShowRulerHS;
            this.tsbNotesRuler.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNotesRuler.Name = "tsbNotesRuler";
            this.tsbNotesRuler.Size = new System.Drawing.Size(23, 22);
            this.tsbNotesRuler.Text = "Show or Hide the Ruler for this note";
            this.tsbNotesRuler.Click += new System.EventHandler(this.TsbNotesRulerClick);
            // 
            // tsbNotesSpell
            // 
            this.tsbNotesSpell.Checked = true;
            this.tsbNotesSpell.CheckOnClick = true;
            this.tsbNotesSpell.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbNotesSpell.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNotesSpell.Image = global::FirstResponse.CaseNotes.Properties.Resources.CheckSpellingHS;
            this.tsbNotesSpell.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNotesSpell.Name = "tsbNotesSpell";
            this.tsbNotesSpell.Size = new System.Drawing.Size(23, 22);
            this.tsbNotesSpell.Text = "Switch spell checking on/off";
            this.tsbNotesSpell.Click += new System.EventHandler(this.TsbNotesSpellClick);
            // 
            // Template
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 455);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.NoteToolStrip);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Template";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CaseNotes Template";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TemplateFormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.NoteToolStrip.ResumeLayout(false);
            this.NoteToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStrip NoteToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripCutButton;
        private System.Windows.Forms.ToolStripButton toolStripCopyButton;
        private System.Windows.Forms.ToolStripButton toolStripPasteButton;
        private System.Windows.Forms.ToolStripSeparator toolStripButton7;
        private System.Windows.Forms.ToolStripButton toolBarBold;
        private System.Windows.Forms.ToolStripButton toolBarItalic;
        private System.Windows.Forms.ToolStripButton toolBarUnder;
        private System.Windows.Forms.ToolStripSeparator toolStripButton8;
        private System.Windows.Forms.ToolStripButton TextLeft;
        private System.Windows.Forms.ToolStripButton TextCenter;
        private System.Windows.Forms.ToolStripButton TextRight;
        private System.Windows.Forms.ToolStripSeparator toolStripButton2;
        private System.Windows.Forms.ToolStripButton BulletList;
        private System.Windows.Forms.ToolStripButton Outdent;
        private System.Windows.Forms.ToolStripButton Indent;
        private System.Windows.Forms.ToolStripSeparator toolStripButton6;
        private System.Windows.Forms.ToolStripComboBox FontButton;
        private System.Windows.Forms.ToolStripComboBox SizeButton;
        private System.Windows.Forms.ToolStripButton FontColour;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbNotesRuler;
        private System.Windows.Forms.ToolStripButton tsbNotesSpell;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem boldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem italicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem underlineToolStripMenuItem;
	}
}