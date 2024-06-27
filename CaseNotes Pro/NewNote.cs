using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using i00SpellCheck;
using Timer = System.Windows.Forms.Timer;

namespace FirstResponse.CaseNotes
{
    public partial class NewNote : Form
    {
        private readonly string CaseNotesDB;
        private readonly string CaseFileDB;
        private readonly string TemplateLocation;
//        private static string format = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern + " HH:mm:ss";
        private static string format = "yyyy-MMM-dd HH:mm:ss"; 
        private string UserDic = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.LastIndexOf("\\")) + "\\dic.dic";
        private readonly string Pass;
        public string Occurred = DateTime.Now.ToString(format);
        public RichTextBox NoteRichText = new RichTextBox();
        private TextRuler NotesRuler = new TextRuler();
        private Timer startDelayTimer = new Timer();
        private Preferences prefs = new Preferences();
        
        public NewNote(string caseNotesDB, string caseFileDB, string pass)
        {
            InitializeComponent();
            AddControls();

            CaseNotesDB = caseNotesDB;
            CaseFileDB = caseFileDB;
            Pass = pass;

            prefs.ReadLocation(caseNotesDB, null);
            prefs.ReadSystemPrefs(caseFileDB, Pass, "open");

            Location = prefs.NoteLocation;
            Size = prefs.NoteSize;
            TemplateLocation = prefs.TemplateLocation;
            
            NoteRichText.Font = new Font(prefs.NoteTypeface, prefs.NoteTypeSize, FontStyle.Regular);
            NoteRichText.Text = null;

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
            TemplateButton.SelectedIndexChanged += TemplateButtonClick;
            NoteRichText.SelectionChanged += SelectionChanged;

            var template = "";
            string[] fileAarray = Directory.GetFiles(TemplateLocation, "*.rtf");
            foreach (var entry in fileAarray)
            {
                template = entry.Substring(entry.LastIndexOf("\\") + 1);
                template = template.Replace(".rtf", "");
                TemplateButton.Items.Add(template);
            }

            dateTimePicker.Format = DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = format;

            startDelayTimer.Interval = 500;
            startDelayTimer.Tick += StartTimerElapsed;
            startDelayTimer.Start();

            NoteRichText.Select();
        }

        private void StartTimerElapsed(object sender, EventArgs e)
        {
            // this awful hack is required to prevent the form startup paint delay caused by the iSpellCheck code.
            // it enables the spell check 3 seconds after the form has loaded. Yuck.
            startDelayTimer.Enabled = false;

            var dictionary = new FlatFileDictionary(UserDic, false);
            NoteRichText.SpellCheck().CurrentDictionary = dictionary;           
            NoteRichText.EnableSpellCheck();
        }

        private void AddControls()
        {
            NotesRuler.Dock = DockStyle.Top;
            NotesRuler.BackColor = Color.Transparent;
            NotesRuler.Font = new Font("Arial", 7.25F);
            NotesRuler.LeftHangingIndent = 0;
            NotesRuler.LeftIndent = 0;
            NotesRuler.LeftMargin = 0;
            NotesRuler.Location = new Point(30, 78);
            NotesRuler.Name = "Ruler";
            NotesRuler.NoMargins = true;
            NotesRuler.RightIndent = 20;
            NotesRuler.RightMargin = 5;
            NotesRuler.Size = new Size(654, 20);
            NotesRuler.TabIndex = 8;
            NotesRuler.TabsEnabled = true;
            NotesRuler.TabAdded += RulerTabAdded;
            NotesRuler.TabRemoved += RulerTabRemoved;
            NotesRuler.TabChanged += RulerTabChanged;
            NotesRuler.BothLeftIndentsChanged += RulerBothLeftIndentsChanged;
            NotesRuler.RightIndentChanging += RulerRightIndentChanging;
            NotesRuler.LeftHangingIndentChanging += RulerLeftHangingIndentChanging;
            NotesRuler.LeftIndentChanging += RulerLeftIndentChanging;
            NotesRuler.Visible = true;

            NoteRichText.Dock = DockStyle.Fill;
            NoteRichText.Name = "NewNote";
            NoteRichText.ScrollBars = RichTextBoxScrollBars.Both;
            NoteRichText.AcceptsTab = true;
            NoteRichText.HideSelection = false;
            NoteRichText.AutoWordSelection = false;
            NoteRichText.Clear();
            NoteRichText.TabIndex = 1;

            panel1.Controls.AddRange(new Control[] { NoteRichText, NotesRuler });
        }

        private void BtnSaveButtonClick(object sender, EventArgs e)
        {
            if (NoteRichText.Text.Length == 0)
            {
                if (MessageBox.Show("Your Case Note is empty - is that what you intended?", "Empty Case Note", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    Clipboard.Clear();
                    NoteRichText.SelectionStart = NoteRichText.TextLength;

                    NoteRichText.AppendText("\r\n\r\n");
                    NoteRichText.SelectionLength = 2;
                    NoteRichText.SelectionIndent = 0;
                    NoteRichText.SelectAll();
                    NoteRichText.Cut();

                    var notePrefs = new Preferences { NoteLocation = Location, NoteSize = Size };
                    notePrefs.WriteNoteLocation(CaseNotesDB, Pass);

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            else
            {
                Clipboard.Clear();
                NoteRichText.SelectionStart = NoteRichText.TextLength;
                NoteRichText.AppendText("\r\n\r\n");
                NoteRichText.SelectionLength = 2;
                NoteRichText.SelectionIndent = 0;
                NoteRichText.ForeColor = Color.Black;
                NoteRichText.SelectAll();
                NoteRichText.Cut();
                Occurred = dateTimePicker.Value.ToString("yyyy-MMM-dd HH:mm:ss");

                var notePrefs = new Preferences { NoteLocation = Location, NoteSize = Size };
                notePrefs.WriteNoteLocation(CaseNotesDB, null);

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void BtnCancelButtonClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NoteRichText.Text))
            {
                var dr = MessageBox.Show("The note details you have entered will be lost.\r\nAre you sure you want to cancel?","New Note", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    NoteRichText = null;
                    NoteRichText.DisableSpellCheck();
                    Close();
                }
            }
            else
            {
                NoteRichText = null;
                NoteRichText.DisableSpellCheck();
                Close();                
            }
        }

        private void ToolStripCutButtonClick(object sender, EventArgs e)
        {
            if (NoteRichText.SelectedText.Length > 0)
                NoteRichText.Cut();
        }

        private void ToolStripCopyButtonClick(object sender, EventArgs e)
        {
            if (NoteRichText.SelectedText.Length > 0)
                NoteRichText.Copy();
        }

        private void ToolStripPasteButtonClick(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Bitmap) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.CommaSeparatedValue) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Dib) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Dif) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.EnhancedMetafile) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.FileDrop) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Html) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Locale) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.MetafilePict) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.OemText) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Palette) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.PenData) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Riff) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Rtf) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Serializable) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.StringFormat) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.SymbolicLink) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.Tiff) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.UnicodeText) |
                Clipboard.GetDataObject().GetDataPresent(DataFormats.WaveAudio))
            {
                NoteRichText.Paste();
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

        private void FormatTextButton(object sender, EventArgs e)
        {
            if (NoteRichText.SelectedText != null && NoteRichText.SelectedText != "")
            {
                FontStyle req;
                if (sender == toolBarBold) { req = FontStyle.Bold; }
                else if (sender == toolBarItalic) { req = FontStyle.Italic; }
                else { req = FontStyle.Underline; }
                Font f = NoteRichText.SelectionFont;
                FontStyle s;
                if (f != null)
                {
                    s = NoteRichText.SelectionFont.Style;
                    if ((f.Style & req) != 0) { s = s & (~req); }
                    else { s = s | req; }
                    NoteRichText.SelectionFont = new Font(f.FontFamily, f.Size, s);
                }
                else
                {
                    int start = NoteRichText.SelectionStart; int l = NoteRichText.SelectionLength;
                    for (int i = start; i < start + l; i++)
                    {
                        NoteRichText.SelectionStart = i; NoteRichText.SelectionLength = 1;
                        f = NoteRichText.SelectionFont;
                        s = NoteRichText.SelectionFont.Style;
                        if ((f.Style & req) != 0) { s = s & (~req); }
                        else { s = s | req; }
                        NoteRichText.SelectionFont = new Font(f.FontFamily, f.Size, s);
                    }
                    NoteRichText.SelectionStart = start; NoteRichText.SelectionLength = l;
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
                s = NoteRichText.SelectionFont.Style;

                if ((s & req) != 0)
                    s = s & (~req);
                else
                    s = s | req;

                NoteRichText.SelectionFont = new Font(NoteRichText.SelectionFont.FontFamily, NoteRichText.SelectionFont.Size, s);
            }            
        }

        private void TextLeftClick(object sender, EventArgs e)
        {
            if (TextLeft.Checked)
            {
                TextCenter.Checked = false;
                TextRight.Checked = false;
                NoteRichText.SelectionAlignment = HorizontalAlignment.Left;
            }
        }

        private void TextCenterClick(object sender, EventArgs e)
        {
            if (TextCenter.Checked)
            {
                TextLeft.Checked = false;
                TextRight.Checked = false;
                NoteRichText.SelectionAlignment = HorizontalAlignment.Center;
            }
        }

        private void TextRightClick(object sender, EventArgs e)
        {
            if (TextRight.Checked)
            {
                TextLeft.Checked = false;
                TextCenter.Checked = false;
                NoteRichText.SelectionAlignment = HorizontalAlignment.Right;
            }
        }

        private void FontButtonSelectedIndexChanged(object sender, EventArgs e)
        {
            if (NoteRichText.SelectionFont == null)
                NoteRichText.SelectionLength = NoteRichText.SelectionLength;

            var currentFont = NoteRichText.SelectionFont;
            var newFont = FontButton.SelectedItem.ToString();
            try
            {
                NoteRichText.SelectionFont = new Font(newFont, currentFont.Size, currentFont.Style);
            }
            catch (NullReferenceException)
            {
                var selStart = NoteRichText.SelectionStart;
                var len = NoteRichText.SelectionLength;
                const int tempStart = 0;
                var rtbTemp = new RichTextBox {Rtf = NoteRichText.SelectedRtf};

                for (int i = 0; i < len; ++i)
                {
                    rtbTemp.Select(tempStart + i, 1);
                    rtbTemp.SelectionFont = new Font(newFont, rtbTemp.SelectionFont.Size, rtbTemp.SelectionFont.Style);
                }

                rtbTemp.Select(tempStart, len);
                NoteRichText.SelectedRtf = rtbTemp.SelectedRtf;
                NoteRichText.Select(selStart, len);
            }
            catch (ArgumentException g)
            {
                MessageBox.Show(g.Message);
            }
        }

        private void SizeButtonSelectedIndexChanged(object sender, EventArgs e)
        {
            if (NoteRichText.SelectionFont == null)
                NoteRichText.SelectionLength = NoteRichText.SelectionLength;

            Font currentFont = NoteRichText.SelectionFont;
            var newSize = Convert.ToInt16(SizeButton.SelectedItem.ToString());

            try
            {
                NoteRichText.SelectionFont = new Font(currentFont.Name, newSize, currentFont.Style);
            }
            catch (NullReferenceException)
            {
                var selStart = NoteRichText.SelectionStart;
                var len = NoteRichText.SelectionLength;
                const int tempStart = 0;
                var rtbTemp = new RichTextBox {Rtf = NoteRichText.SelectedRtf};

                for (int i = 0; i < len; ++i)
                {
                    rtbTemp.Select(tempStart + i, 1);
                    rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont.Name, newSize, rtbTemp.SelectionFont.Style);
                }

                rtbTemp.Select(tempStart, len);
                NoteRichText.SelectedRtf = rtbTemp.SelectedRtf;
                NoteRichText.Select(selStart, len);
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

            if (NoteRichText.SelectionFont == null)
                NoteRichText.SelectionLength = NoteRichText.SelectionLength;
            else
                newColor.Color = NoteRichText.SelectionColor;

            if (newColor.ShowDialog() == DialogResult.OK)
            {
                NoteRichText.SelectionColor = newColor.Color;
                var colors = (int[])newColor.CustomColors.Clone();
                prefs.CustomColors = Functions.SetCustomColors(colors, CaseNotesDB);
            }
        }

        private void TsbHighlightClick(object sender, EventArgs e)
        {
            if (NoteRichText.SelectionFont == null)
                NoteRichText.SelectionLength = NoteRichText.SelectionLength;
            else if (NoteRichText.SelectionBackColor != Color.Yellow)
                NoteRichText.SelectionBackColor = Color.Yellow;
            else
                NoteRichText.SelectionBackColor = NoteRichText.BackColor;
        }

        private void BulletListClick(object sender, EventArgs e)
        {
            if (BulletList.Checked)
            {
                NoteRichText.SelectionBullet = true;
                NoteRichText.SelectionIndent += 12;
            }
            else
            {
                NoteRichText.SelectionBullet = false;
                NoteRichText.SelectionIndent -= 12;
            }
        }

        private void OutdentClick(object sender, EventArgs e)
        {
            NoteRichText.SelectionIndent -= 50;
        }

        private void IndentClick(object sender, EventArgs e)
        {
            NoteRichText.SelectionIndent += 50;
        }

        private void TemplateButtonClick(object sender, EventArgs e)
        {
            var template = "";
            string[] fileAarray = Directory.GetFiles(TemplateLocation, "*.rtf");
            foreach (var entry in fileAarray)
            {
                template = entry.Substring(entry.LastIndexOf("\\") + 1);
                template = template.Replace(".rtf", "");
                if (TemplateButton.Text == template)
                {
                    var tempRTF = new RichTextBox();
                    tempRTF.LoadFile(entry);
                    tempRTF.SelectAll();
                    tempRTF.Copy();
                    NoteRichText.Paste();
                    break;
                }
            }
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            Font currentFont = NoteRichText.SelectionFont;

            try
            {
                NotesRuler.SetTabPositionsInPixels(NoteRichText.SelectionTabs);

                if (NoteRichText.SelectionLength < NoteRichText.TextLength - 1)
                {
                    NotesRuler.LeftIndent = (int)(NoteRichText.SelectionIndent / NotesRuler.DotsPerMillimeter);
                    NotesRuler.LeftHangingIndent = (int)((float)NoteRichText.SelectionHangingIndent / NotesRuler.DotsPerMillimeter) + NotesRuler.LeftIndent;
                    NotesRuler.RightIndent = (int)(NoteRichText.SelectionRightIndent / NotesRuler.DotsPerMillimeter);
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

        private void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            btnSaveButton.PerformClick();
        }

        private void RulerTabAdded(TextRuler.TabEventArgs args)
        {

            try
            {
                NoteRichText.SelectionTabs = NotesRuler.TabPositionsInPixels.ToArray();
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
                NoteRichText.SelectionTabs = NotesRuler.TabPositionsInPixels.ToArray();
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
                NoteRichText.SelectionTabs = NotesRuler.TabPositionsInPixels.ToArray();
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
                NoteRichText.SelectionIndent = (int)(leftIndent * NotesRuler.DotsPerMillimeter);
                NoteRichText.SelectionHangingIndent = (int)(hangingIndent * NotesRuler.DotsPerMillimeter) - (int)(leftIndent * NotesRuler.DotsPerMillimeter);
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
                NoteRichText.SelectionRightIndent = (int)(newValue * NotesRuler.DotsPerMillimeter);
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
                NoteRichText.SelectionIndent = (int)(newValue * NotesRuler.DotsPerMillimeter);
                NoteRichText.SelectionHangingIndent = (int)(NotesRuler.LeftHangingIndent * NotesRuler.DotsPerMillimeter) - (int)(newValue * NotesRuler.DotsPerMillimeter);
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
                NoteRichText.SelectionHangingIndent = (int)(newValue * NotesRuler.DotsPerMillimeter) - (int)(NotesRuler.LeftIndent * NotesRuler.DotsPerMillimeter);
            }
            catch (Exception fail)
            {
                MessageBox.Show("RulerLeftHangingIndentChanging:\r\n" + fail.Message);
            }
        }

        private void TsbNotesRulerClick(object sender, EventArgs e)
        {
            NotesRuler.Visible = !NotesRuler.Visible;
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
            NoteRichText.SpellCheck().CurrentDictionary = dictionary;

            if (NoteRichText.IsSpellCheckEnabled())
                NoteRichText.DisableSpellCheck();
            else
                NoteRichText.EnableSpellCheck();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(NoteRichText.Text))
                {
                    var dr = MessageBox.Show("The note details you have entered will be lost.\r\nAre you sure you want to cancel?","New Note", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        NoteRichText = null;
                    }
                    else
                        e.Cancel = true;
                }
                else
                    NoteRichText = null;
            }
            catch (Exception fail)
            { }
        }

    }
}
