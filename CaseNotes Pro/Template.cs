using System;
using System.Drawing;
using System.Windows.Forms;
using i00SpellCheck;
using Timer = System.Windows.Forms.Timer;

namespace FirstResponse.CaseNotes
{
    public partial class Template : Form
    {
        private string _templateFileName;
        private string _templatePath;
        private bool _changed;
        private string UserDic = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.LastIndexOf("\\")) + "\\dic.dic";
        private readonly string CaseNotesDB;

        private TextRuler TemplateRuler = new TextRuler();
        public RichTextBox rtbTemplate = new RichTextBox();
        private Timer startDelayTimer = new Timer();
        private Preferences prefs = new Preferences();

        public Template(string tmpFileName, string tmpPath, string caseNotesDB)
        {
            InitializeComponent();
            AddControls();

            CaseNotesDB = caseNotesDB;
            _templateFileName = tmpFileName;
            _templatePath = tmpPath;

            prefs.ReadSystemPrefs(caseNotesDB, null, "open");

            if (!string.IsNullOrEmpty(_templateFileName))
                rtbTemplate.LoadFile(_templatePath + "\\" + _templateFileName);
            else
            {
                try
                {
                    rtbTemplate.Font = new Font(prefs.NoteTypeface, prefs.NoteTypeSize, FontStyle.Regular);
                }
                catch
                {}
            }

            rtbTemplate.Font = new Font(prefs.NoteTypeface, prefs.NoteTypeSize, FontStyle.Regular);

            var fonts = new[] { FontStyle.Bold, FontStyle.Italic, FontStyle.Underline };
            toolBarBold.Tag = fonts[0];
            toolBarItalic.Tag = fonts[1];
            toolBarUnder.Tag = fonts[2];

            var ff = FontFamily.Families;
            var m = 0;
            var n = 0;

            string[] sizes = { "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72" };

            FontButton.Items.Clear();
            SizeButton.Items.Clear();

            for (int x = 0; x < ff.Length; x++)
            {
                FontButton.Items.Add(ff[x].Name);
                if (ff[x].Name == prefs.NoteTypeface) m = x;
            }

            for (int x = 0; x < sizes.Length; x++)
            {
                SizeButton.Items.Add(sizes[x]);
                if (sizes[x] == prefs.NoteTypeSize.ToString()) n = x;
            }

            FontButton.SelectedIndex = m;
            SizeButton.SelectedIndex = n;

            FontButton.SelectedIndexChanged += FontButtonSelectedIndexChanged;
            SizeButton.SelectedIndexChanged += SizeButtonSelectedIndexChanged;
            rtbTemplate.SelectionChanged += SelectionChanged;
            rtbTemplate.Select();

            startDelayTimer.Interval = 3000;
            startDelayTimer.Tick += StartTimerElapsed;
            startDelayTimer.Start();

            _changed = false;
        }

        private void StartTimerElapsed(object sender, EventArgs e)
        {
            // this awful hack is required to prevent the form startup paint delay caused by the iSpellCheck code.
            // it enables the spell check 3 seconds after the form has loaded. Yuck.
            startDelayTimer.Enabled = false;

            var scSettings = new SpellCheckSettings
            {
                AllowF7 = true,
                AllowAdditions = true,
                AllowIgnore = true,
                AllowRemovals = true,
                AllowInMenuDefs = true,
                AllowChangeTo = true
            };

            var dictionary = new FlatFileDictionary(UserDic, false);
            rtbTemplate.SpellCheck().CurrentDictionary = dictionary;

            rtbTemplate.EnableSpellCheck();
        }

        private void AddControls()
        {
            TemplateRuler.Dock = DockStyle.Top;
            TemplateRuler.BackColor = Color.Transparent;
            TemplateRuler.Font = new Font("Arial", 7.25F);
            TemplateRuler.LeftHangingIndent = 0;
            TemplateRuler.LeftIndent = 0;
            TemplateRuler.LeftMargin = 0;
            TemplateRuler.Location = new Point(30, 78);
            TemplateRuler.Name = "Ruler";
            TemplateRuler.NoMargins = true;
            TemplateRuler.RightIndent = 20;
            TemplateRuler.RightMargin = 5;
            TemplateRuler.Size = new Size(654, 20);
            TemplateRuler.TabIndex = 8;
            TemplateRuler.TabsEnabled = true;
            TemplateRuler.TabAdded += RulerTabAdded;
            TemplateRuler.TabRemoved += RulerTabRemoved;
            TemplateRuler.TabChanged += RulerTabChanged;
            TemplateRuler.BothLeftIndentsChanged += RulerBothLeftIndentsChanged;
            TemplateRuler.RightIndentChanging += RulerRightIndentChanging;
            TemplateRuler.LeftHangingIndentChanging += RulerLeftHangingIndentChanging;
            TemplateRuler.LeftIndentChanging += RulerLeftIndentChanging;
            TemplateRuler.Visible = true;

            rtbTemplate.Dock = DockStyle.Fill;
            rtbTemplate.Name = "NewTemplate";
            rtbTemplate.ScrollBars = RichTextBoxScrollBars.Both;
            rtbTemplate.AcceptsTab = true;
            rtbTemplate.HideSelection = false;
            rtbTemplate.AutoWordSelection = false;
            rtbTemplate.TextChanged += RtbTemplateOnTextChanged;
            rtbTemplate.Clear();
            rtbTemplate.TabIndex = 1;

            panel3.Controls.AddRange(new Control[] { rtbTemplate, TemplateRuler });
        }

        private void RtbTemplateOnTextChanged(object sender, EventArgs eventArgs)
        {
            _changed = true;
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            using (var dlgSave = new SaveFileDialog())
            try
            {
                dlgSave.FileName = _templateFileName;
                dlgSave.InitialDirectory = _templatePath;
                dlgSave.DefaultExt = "rtf";
                dlgSave.Title = "Save File As";
                dlgSave.Filter = "RTF Files (*.rtf)|*.rtf";
                if (dlgSave.ShowDialog() == DialogResult.OK)
                {
                    rtbTemplate.SaveFile(dlgSave.FileName, RichTextBoxStreamType.RichText);
                    _changed = false;
                }
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
            }
        }

        private void TemplateFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(rtbTemplate.Text) && _changed)
            {
                var dr = MessageBox.Show("The template details you have entered will be lost.\r\nAre you sure you want to cancel?", "New Template", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                    rtbTemplate = null;
                else
                    e.Cancel = true;
            }
        }

        private void ToolStripCutButtonClick(object sender, EventArgs e)
        {
            if (rtbTemplate.SelectedText.Length > 0)
                rtbTemplate.Cut();
        }

        private void ToolStripCopyButtonClick(object sender, EventArgs e)
        {
            if (rtbTemplate.SelectedText.Length > 0)
                rtbTemplate.Copy();
        }

        private void ToolStripPasteButtonClick(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Bitmap) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Dib) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.EnhancedMetafile) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Html) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.MetafilePict) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.OemText) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Rtf) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Tiff) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.UnicodeText))
            {
                rtbTemplate.Paste();
            }
        }

        private void BoldToolStripMenuItemClick(object sender, EventArgs e)
        {
            toolBarBold.PerformClick();
        }

        private void ItalicToolStripMenuItemClick(object sender, EventArgs e)
        {
            toolBarItalic.PerformClick();
        }

        private void UnderlineToolStripMenuItemClick(object sender, EventArgs e)
        {
            toolBarUnder.PerformClick();
        }

        private void TextLeftClick(object sender, EventArgs e)
        {
            if (TextLeft.Checked)
            {
                TextCenter.Checked = false;
                TextRight.Checked = false;
                rtbTemplate.SelectionAlignment = HorizontalAlignment.Left;
            }
        }

        private void TextCenterClick(object sender, EventArgs e)
        {
            if (TextCenter.Checked)
            {
                TextLeft.Checked = false;
                TextRight.Checked = false;
                rtbTemplate.SelectionAlignment = HorizontalAlignment.Center;
            }
        }

        private void TextRightClick(object sender, EventArgs e)
        {
            if (TextRight.Checked)
            {
                TextLeft.Checked = false;
                TextCenter.Checked = false;
                rtbTemplate.SelectionAlignment = HorizontalAlignment.Right;
            }
        }

        private void BulletListClick(object sender, EventArgs e)
        {
            if (BulletList.Checked)
            {
                rtbTemplate.SelectionBullet = true;
                rtbTemplate.SelectionIndent += 12;
            }
            else
            {
                rtbTemplate.SelectionBullet = false;
                rtbTemplate.SelectionIndent -= 12;
            }
        }

        private void OutdentClick(object sender, EventArgs e)
        {
            rtbTemplate.SelectionIndent -= 50;
        }

        private void IndentClick(object sender, EventArgs e)
        {
            rtbTemplate.SelectionIndent += 50;
        }

        private void FontButtonSelectedIndexChanged(object sender, EventArgs e)
        {
            if (rtbTemplate.SelectionFont == null)
                rtbTemplate.SelectionLength = rtbTemplate.SelectionLength;

            var currentFont = rtbTemplate.SelectionFont;
            var newFont = FontButton.SelectedItem.ToString();
            try
            {
                rtbTemplate.SelectionFont = new Font(newFont, currentFont.Size, currentFont.Style);
            }
            catch (NullReferenceException)
            {
                var selStart = rtbTemplate.SelectionStart;
                var len = rtbTemplate.SelectionLength;
                const int tempStart = 0;
                var rtbTemp = new RichTextBox { Rtf = rtbTemplate.SelectedRtf };

                for (int i = 0; i < len; ++i)
                {
                    rtbTemp.Select(tempStart + i, 1);
                    rtbTemp.SelectionFont = new Font(newFont, rtbTemp.SelectionFont.Size, rtbTemp.SelectionFont.Style);
                }

                rtbTemp.Select(tempStart, len);
                rtbTemplate.SelectedRtf = rtbTemp.SelectedRtf;
                rtbTemplate.Select(selStart, len);
            }
            catch (ArgumentException g)
            {
                MessageBox.Show(g.Message);
            }
        }

        private void SizeButtonSelectedIndexChanged(object sender, EventArgs e)
        {
            if (rtbTemplate.SelectionFont == null)
                rtbTemplate.SelectionLength = rtbTemplate.SelectionLength;

            Font currentFont = rtbTemplate.SelectionFont;
            var newSize = Convert.ToInt16(SizeButton.SelectedItem.ToString());

            try
            {
                rtbTemplate.SelectionFont = new Font(currentFont.Name, newSize, currentFont.Style);
            }
            catch (NullReferenceException)
            {
                var selStart = rtbTemplate.SelectionStart;
                var len = rtbTemplate.SelectionLength;
                const int tempStart = 0;
                var rtbTemp = new RichTextBox { Rtf = rtbTemplate.SelectedRtf };

                for (int i = 0; i < len; ++i)
                {
                    rtbTemp.Select(tempStart + i, 1);
                    rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont.Name, newSize, rtbTemp.SelectionFont.Style);
                }

                rtbTemp.Select(tempStart, len);
                rtbTemplate.SelectedRtf = rtbTemp.SelectedRtf;
                rtbTemplate.Select(selStart, len);
            }
            catch (ArgumentException g)
            {
                MessageBox.Show(g.Message);
            }
        }

        private void FontColourClick(object sender, EventArgs e)
        {
            var newColor = new ColorDialog();
            newColor.CustomColors = Functions.GetCustomColors(CaseNotesDB);

            if (newColor.ShowDialog() == DialogResult.OK)
            {
                rtbTemplate.SelectionColor = newColor.Color;
                var colors = (int[])newColor.CustomColors.Clone();
                prefs.CustomColors = Functions.SetCustomColors(colors, CaseNotesDB);
            }
        }

        private void TsbNotesRulerClick(object sender, EventArgs e)
        {
            TemplateRuler.Visible = !TemplateRuler.Visible;
        }

        private void TsbNotesSpellClick(object sender, EventArgs e)
        {
            var scSettings = new SpellCheckSettings
            {
                AllowF7 = true,
                AllowAdditions = true,
                AllowIgnore = true,
                AllowRemovals = true,
                AllowInMenuDefs = true,
                AllowChangeTo = true
            };

            var dictionary = new FlatFileDictionary(UserDic, false);
            rtbTemplate.SpellCheck().CurrentDictionary = dictionary;

            if (rtbTemplate.IsSpellCheckEnabled())
                rtbTemplate.DisableSpellCheck();
            else
                rtbTemplate.EnableSpellCheck();
        }

        private void FormatTextButton(object sender, EventArgs e)
        {
            if (rtbTemplate.SelectedText != null && rtbTemplate.SelectedText != "")
            {
                FontStyle req;
                if (sender == toolBarBold) 
                    req = FontStyle.Bold;
                else if (sender == toolBarItalic) 
                    req = FontStyle.Italic;
                else 
                    req = FontStyle.Underline;
                Font f = rtbTemplate.SelectionFont;
                FontStyle s;
                if (f != null)
                {
                    s = rtbTemplate.SelectionFont.Style;
                    if ((f.Style & req) != 0) 
                        s = s & (~req);
                    else 
                        s = s | req;
                    rtbTemplate.SelectionFont = new Font(f.FontFamily, f.Size, s);
                }
                else
                {
                    int start = rtbTemplate.SelectionStart; int l = rtbTemplate.SelectionLength;
                    for (int i = start; i < start + l; i++)
                    {
                        rtbTemplate.SelectionStart = i; rtbTemplate.SelectionLength = 1;
                        f = rtbTemplate.SelectionFont;
                        s = rtbTemplate.SelectionFont.Style;
                        if ((f.Style & req) != 0) 
                            s = s & (~req);
                        else 
                            s = s | req;
                        rtbTemplate.SelectionFont = new Font(f.FontFamily, f.Size, s);
                    }
                    rtbTemplate.SelectionStart = start; rtbTemplate.SelectionLength = l;
                }
            }
            else
            {
                FontStyle req;
                if (sender == toolBarBold)
                    req = FontStyle.Bold;
                else if (sender == toolBarItalic)
                    req = FontStyle.Italic;
                else
                    req = FontStyle.Underline;

                FontStyle s;
                s = rtbTemplate.SelectionFont.Style;
                if ((s & req) != 0)
                    s = s & (~req);
                else 
                    s = s | req;
                rtbTemplate.SelectionFont = new Font(rtbTemplate.SelectionFont.FontFamily, rtbTemplate.SelectionFont.Size, s);
            }
        }

        private void RulerTabAdded(TextRuler.TabEventArgs args)
        {
            try
            {
                rtbTemplate.SelectionTabs = TemplateRuler.TabPositionsInPixels.ToArray();
            }
            catch (Exception fail)
            {
                MessageBox.Show("TabAdded:\r\n" + fail.Message);
            }
        }

        private void RulerTabChanged(TextRuler.TabEventArgs args)
        {
            try
            {
                rtbTemplate.SelectionTabs = TemplateRuler.TabPositionsInPixels.ToArray();
            }
            catch (Exception fail)
            {
                MessageBox.Show("TabChanged:\r\n" + fail.Message);
            }
        }

        private void RulerTabRemoved(TextRuler.TabEventArgs args)
        {
            try
            {
                rtbTemplate.SelectionTabs = TemplateRuler.TabPositionsInPixels.ToArray();
            }
            catch (Exception fail)
            {
                MessageBox.Show("TabRemoved:\r\n" + fail.Message);
            }
        }

        private void RulerBothLeftIndentsChanged(int leftIndent, int hangingIndent)
        {
            try
            {
                rtbTemplate.SelectionIndent = (int)(leftIndent * TemplateRuler.DotsPerMillimeter);
                rtbTemplate.SelectionHangingIndent = (int)(hangingIndent * TemplateRuler.DotsPerMillimeter) - (int)(leftIndent * TemplateRuler.DotsPerMillimeter);
            }
            catch (Exception fail)
            {
                MessageBox.Show("RulerBothLeftIndentsChanged:\r\n" + fail.Message);
            }
        }

        private void RulerRightIndentChanging(int newValue)
        {
            try
            {
                rtbTemplate.SelectionRightIndent = (int)(newValue * TemplateRuler.DotsPerMillimeter);
            }
            catch (Exception fail)
            {
                MessageBox.Show("RulerRightIndentChanging:\r\n" + fail.Message);
            }
        }

        private void RulerLeftIndentChanging(int newValue)
        {
            try
            {
                rtbTemplate.SelectionIndent = (int)(newValue * TemplateRuler.DotsPerMillimeter);
                rtbTemplate.SelectionHangingIndent = (int)(TemplateRuler.LeftHangingIndent * TemplateRuler.DotsPerMillimeter) - (int)(newValue * TemplateRuler.DotsPerMillimeter);
            }
            catch (Exception fail)
            {
                MessageBox.Show("RulerLeftIndentChanging:\r\n" + fail.Message);
            }
        }

        private void RulerLeftHangingIndentChanging(int newValue)
        {
            try
            {
                rtbTemplate.SelectionHangingIndent = (int)(newValue * TemplateRuler.DotsPerMillimeter) - (int)(TemplateRuler.LeftIndent * TemplateRuler.DotsPerMillimeter);
            }
            catch (Exception fail)
            {
                MessageBox.Show("RulerLeftHangingIndentChanging:\r\n" + fail.Message);
            }
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            Font currentFont = rtbTemplate.SelectionFont;

            try
            {
                TemplateRuler.SetTabPositionsInPixels(rtbTemplate.SelectionTabs);

                if (rtbTemplate.SelectionLength < rtbTemplate.TextLength - 1)
                {
                    TemplateRuler.LeftIndent = (int)(rtbTemplate.SelectionIndent / TemplateRuler.DotsPerMillimeter);
                    TemplateRuler.LeftHangingIndent = (int)((float)rtbTemplate.SelectionHangingIndent / TemplateRuler.DotsPerMillimeter) + TemplateRuler.LeftIndent;
                    TemplateRuler.RightIndent = (int)(rtbTemplate.SelectionRightIndent / TemplateRuler.DotsPerMillimeter);
                }
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                FontButton.SelectedItem = currentFont.Name;
                SizeButton.SelectedItem = currentFont.Size.ToString();
                toolBarBold.Checked = currentFont.Bold;
                toolBarItalic.Checked = currentFont.Italic;
                toolBarUnder.Checked = currentFont.Underline;
            }
            catch (NullReferenceException)
            {
            }
        }

    }
}
