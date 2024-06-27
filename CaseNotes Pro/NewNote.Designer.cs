using System;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes
{
    partial class NewNote
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewNote));
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
            this.toolBarHighlight = new System.Windows.Forms.ToolStripButton();
            this.FontColour = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TemplateButton = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbNotesRuler = new System.Windows.Forms.ToolStripButton();
            this.tsbNotesSpell = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelButton = new System.Windows.Forms.Button();
            this.btnSaveButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.italicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.underlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.NoteToolStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.toolBarHighlight,
            this.FontColour,
            this.toolStripSeparator1,
            this.TemplateButton,
            this.toolStripSeparator2,
            this.tsbNotesRuler,
            this.tsbNotesSpell});
            this.NoteToolStrip.Location = new System.Drawing.Point(0, 0);
            this.NoteToolStrip.Name = "NoteToolStrip";
            this.NoteToolStrip.Size = new System.Drawing.Size(1201, 25);
            this.NoteToolStrip.TabIndex = 0;
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
            // toolBarHighlight
            // 
            this.toolBarHighlight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBarHighlight.Image = global::FirstResponse.CaseNotes.Properties.Resources.text_highlight_24;
            this.toolBarHighlight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBarHighlight.Name = "toolBarHighlight";
            this.toolBarHighlight.Size = new System.Drawing.Size(23, 22);
            this.toolBarHighlight.Text = "Highlight the selected text";
            this.toolBarHighlight.Click += new System.EventHandler(this.TsbHighlightClick);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // TemplateButton
            // 
            this.TemplateButton.Name = "TemplateButton";
            this.TemplateButton.Size = new System.Drawing.Size(140, 25);
            this.TemplateButton.Text = "Insert template text";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter or paste the text for your new note in the box below:  (\'ctrl + s\' to save)" +
                "";
            // 
            // btnCancelButton
            // 
            this.btnCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelButton.Location = new System.Drawing.Point(1121, 613);
            this.btnCancelButton.Name = "btnCancelButton";
            this.btnCancelButton.Size = new System.Drawing.Size(75, 23);
            this.btnCancelButton.TabIndex = 4;
            this.btnCancelButton.Text = "Cancel";
            this.btnCancelButton.UseVisualStyleBackColor = true;
            this.btnCancelButton.Click += new System.EventHandler(this.BtnCancelButtonClick);
            // 
            // btnSaveButton
            // 
            this.btnSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveButton.Location = new System.Drawing.Point(1040, 613);
            this.btnSaveButton.Name = "btnSaveButton";
            this.btnSaveButton.Size = new System.Drawing.Size(75, 23);
            this.btnSaveButton.TabIndex = 3;
            this.btnSaveButton.Text = "Save";
            this.btnSaveButton.UseVisualStyleBackColor = true;
            this.btnSaveButton.Click += new System.EventHandler(this.BtnSaveButtonClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.boldToolStripMenuItem,
            this.italicToolStripMenuItem,
            this.underlineToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1199, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.ShowShortcutKeys = false;
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.saveToolStripMenuItem.Text = "save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItemClick);
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
            // dateTimePicker
            // 
            this.dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker.Location = new System.Drawing.Point(1036, 26);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(156, 20);
            this.dateTimePicker.TabIndex = 6;
            this.dateTimePicker.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1034, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Occurred Date:";
            this.label2.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(0, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1201, 555);
            this.panel1.TabIndex = 8;
            // 
            // NewNote
            // 
            this.AcceptButton = this.btnSaveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1201, 642);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.btnSaveButton);
            this.Controls.Add(this.btnCancelButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NoteToolStrip);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewNote";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "New Note";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.NoteToolStrip.ResumeLayout(false);
            this.NoteToolStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip NoteToolStrip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelButton;
        private System.Windows.Forms.Button btnSaveButton;
        private System.Windows.Forms.ToolStripButton toolStripCutButton;
        private System.Windows.Forms.ToolStripButton toolStripCopyButton;
        private System.Windows.Forms.ToolStripButton toolStripPasteButton;
        private System.Windows.Forms.ToolStripButton toolBarBold;
        private System.Windows.Forms.ToolStripButton toolBarItalic;
        private System.Windows.Forms.ToolStripButton toolBarUnder;
        private System.Windows.Forms.ToolStripButton TextLeft;
        private System.Windows.Forms.ToolStripButton TextCenter;
        private System.Windows.Forms.ToolStripButton TextRight;
        private System.Windows.Forms.ToolStripSeparator toolStripButton7;
        private System.Windows.Forms.ToolStripSeparator toolStripButton8;
        private System.Windows.Forms.ToolStripSeparator toolStripButton6;
        private System.Windows.Forms.ToolStripComboBox FontButton;
        private System.Windows.Forms.ToolStripButton FontColour;
        private System.Windows.Forms.ToolStripButton toolBarHighlight;
        private System.Windows.Forms.ToolStripComboBox SizeButton;
        private System.Windows.Forms.ToolStripButton BulletList;
        private System.Windows.Forms.ToolStripSeparator toolStripButton2;
        private System.Windows.Forms.ToolStripButton Outdent;
        private System.Windows.Forms.ToolStripButton Indent;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox TemplateButton;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbNotesSpell;
        private System.Windows.Forms.ToolStripButton tsbNotesRuler;
        private System.Windows.Forms.ToolStripMenuItem boldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem italicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem underlineToolStripMenuItem;
    }
}