using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using i00SpellCheck;
using FirstResponse.CaseNotes.Properties;
using Microsoft.Win32;

namespace FirstResponse.CaseNotes
{
    public partial class MainForm : Form
    {
        #region Global Declarations

        public static readonly string CaseNotesDB = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.LastIndexOf("\\")) + "\\CaseNotes.db3";
        public static readonly string UserDic = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.LastIndexOf("\\")) + "\\dic.dic";
        public static string CaseFileDB = "";
        public static bool TabsDone;
        public static string Args;
        public static string dbVer;
        public static bool MetaValid;
        public static bool NotesValid;
        public static bool AuditValid;
        public static bool CaseValid;
        public static bool Professional;
        public static bool UserInit;
 
        public static Case CaseFile;
        public static Preferences Prefs;

        private ContextMenu CNpopUpMenu;
        private Panel clPanel = new Panel();
        private List<Control> _controlList = new List<Control>();
        private GuiController Gui;
        private bool User1Dirty;
        private bool User2Dirty;
        private bool User3Dirty;
        private bool User4Dirty;
        private bool User5Dirty;
        private bool User6Dirty;
        private bool User7Dirty;
        private bool User8Dirty;
        private bool User9Dirty;
        private bool User10Dirty;
        private bool CheckListDirty;
        
        protected MruStripMenuInline MruMenu;
        private readonly string _mruRegKey = Application.UserAppDataRegistry.ToString();

        #endregion

        public MainForm()
        {
            InitializeComponent();
            Activation();
            InitialSettings();
        }

        private void InitialSettings()
        {
            Text = "CaseNotes Professional version " + Assembly.GetExecutingAssembly().GetName().Version;
            UpdateStatus("Welcome to CaseNotes!");

            var appDataPath = Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.LastIndexOf("\\"));

            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            var prefs = new Preferences();
            prefs.ReadLocation(CaseNotesDB, null);
            prefs.ReadSystemPrefs(CaseNotesDB, null, "open");
            Prefs = prefs;
            dbVer = Functions.ConfigureDatabase(CaseNotesDB, null);

            clPanel.Visible = false;
            Location = Prefs.AppLocation;
            Size = Prefs.AppSize;
            tabControl.Visible = false;
            DisableControls();
            DisableRtfControls();

            CNpopUpMenu = new ContextMenu();
            CNpopUpMenu.MenuItems.Add("Copy", CNpopup);
            CNpopUpMenu.MenuItems.Add("-");
            CNpopUpMenu.MenuItems.Add("Select All", CNpopup);
            RtfMain.ContextMenu = CNpopUpMenu;

            MruMenu = new MruStripMenuInline(fileToolStripMenuItem, menuRecentFile, OnMruFile, _mruRegKey + "\\MRU");
            MruMenu.LoadFromRegistry();

            var fonts = new FontStyle[] { FontStyle.Bold, FontStyle.Italic, FontStyle.Underline };
            tsbBold.Tag = fonts[0];
            tsbItalic.Tag = fonts[1];
            tsbUnderline.Tag = fonts[2];

            var ff = FontFamily.Families;
            int m = 0;
            int n = 0;

            string[] sizes = { "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72" };

            tsbFontButton.Items.Clear();
            tsbSizeButton.Items.Clear();

            for (int x = 0; x < ff.Length; x++)
            {
                tsbFontButton.Items.Add(ff[x].Name);
                if (ff[x].Name == Prefs.NoteTypeface) m = x;
            }

            for (int x = 0; x < sizes.Length; x++)
            {
                tsbSizeButton.Items.Add(sizes[x]);
                if (sizes[x] == prefs.NoteTypeSize.ToString()) n = x;
            }

            UserInit = true;
            tsbFontButton.SelectedIndex = m;
            tsbSizeButton.SelectedIndex = n;
            RtfMain.LinkClicked += MyRTFLinkClicked;
            UserSettings();

            //BetaWarning();
        }

        private void BetaWarning()
        {
            TimeSpan duration = Convert.ToDateTime("25-Feb-2017 23:59:59") - DateTime.Now;
            if (duration.Days < 0)
            {
                MessageBox.Show("This pre-release version of CaseNotes has expired.\nPlease contact FirstResponse to obtain the final release." + "\n\nCaseNotes will now close.", "Pre-release Code Expired", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
            }
            else
            {
                if (duration.Days < 20)
                    MessageBox.Show("This pre-release version of CaseNotes will expire in " + duration.Days + " days.", "pre-release Code Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //else
                //    MessageBox.Show("This is a Beta version of CaseNotes Professional.\n\nPlease ensure you keep backups of your work and do not use on production cases!", "Beta Code Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UserSettings()
        {
            string temp = Application.UserAppDataPath;
            temp = temp.Substring(0, temp.LastIndexOf("\\")) + "\\Templates";

            var dict = Resources.Default_Dictionary;
            using (TextWriter writer = File.CreateText(UserDic))
            {
                writer.WriteLine(dict);
            }

            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
                var text = Resources.Default_Checklist;
                using (TextWriter writer = File.CreateText(temp + "\\Default Checklist.xml"))
                {
                    writer.WriteLine(text);
                }

                text = Resources.Default_Template;
                using (TextWriter writer = File.CreateText(temp + "\\Default Template.rtf"))
                {
                    writer.WriteLine(text);
                }
            }
        }

        private void Activation()
        {
            //var activating = new Activate();
            //Professional = activating.activated;
            Professional = true;
        }

        private void TsbNewCaseClick(object sender, EventArgs e)
        {
            RtfMain.Enabled = false;
            var newCase = new Case(CaseNotesDB, "new");
            var result = newCase.ShowDialog();

            while (newCase.Result == false)
            {
                result = newCase.ShowDialog();
            }

            if (result == DialogResult.Cancel)
            {
                RtfMain.Enabled = true;
                Cursor = Cursors.Default;
                return;
            }

            if (CaseFile != null)
                closeToolStripMenuItem.PerformClick();

            CaseFile = newCase;
            CaseFileDB = CaseFile.CaseFileDB;
            MruMenu.AddFile(CaseFile.CaseFileDB);
            var prefs = new Preferences();
            prefs.ReadSystemPrefs(CaseFile.CaseFileDB, CaseFile.FilePass, "open");
            Prefs = prefs;

            DisplayMetaData();
            CaseFile.WriteAuditData("Success", "The case file has been created successfully.");
            DisplayAuditData();
            tabControl.Visible = true;
            EnableControls();
        }

        private void TsbOpenCaseClick(object sender, EventArgs e)
        {
            if (CaseFile != null)
            {
                UserInit = false;
                tsbSaveCase.PerformClick();
            }
            Cursor = Cursors.WaitCursor;
            RtfMain.Enabled = false;
            var method = "open";
            var existingCaseFile = "";

            if (!string.IsNullOrEmpty(Args))
            {
                CaseFileDB = Args;
                Args = "";
                method = "mru";
            }
            else
            {
                existingCaseFile = CaseFileDB;
                CaseFileDB = CaseNotesDB;
            }

            var openCase = new Case(CaseFileDB, method);
            if (!openCase.Result)
            {
                RtfMain.Enabled = true;
                Cursor = Cursors.Default;
                CaseFileDB = existingCaseFile;
                return;
            }

            if (CaseFile != null)
                closeToolStripMenuItem.PerformClick();

            CaseFile = openCase;
            CaseFileDB = CaseFile.CaseFileDB;
            MruMenu.AddFile(CaseFile.CaseFileDB);
            CaseFileDB = CaseFile.CaseFileDB;
            AuditValid = CaseFile.VerifyAuditData(); 
            var prefs = new Preferences();
            prefs.ReadSystemPrefs(CaseFile.CaseFileDB, CaseFile.FilePass, "open");
            prefs.ReadUserPrefs(CaseFile.CaseFileDB, CaseFile.FilePass);
            MetaValid = CaseFile.ReadMetaData(false);
            Prefs = prefs;

            DisplayMetaData();

            CaseFile.WriteAuditData("=======", " ");
            CaseValid = true;

            if (MetaValid)
                CaseFile.WriteAuditData("Success", "The case Metadata has verified.");
            else
            {
                CaseValid = false;
                CaseFile.WriteAuditData("*******", " "); 
                CaseFile.WriteAuditData("Failure - Metadata", "The case file Metadata has failed to verify correctly.");
                UpdateStatus("Your case file metadata has failed to verify correctly.");
                MessageBox.Show("Tamper Warning!\r\nYour case file Metadata has failed to verify correctly.", "CaseNotes Tamper Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            if (NotesValid)
                CaseFile.WriteAuditData("Success", "The case Note data verified.");
            else
            {
                CaseValid = false;
                CaseFile.WriteAuditData("*******", " "); 
                CaseFile.WriteAuditData("Failure - Notes data", "The case file Notes data has failed to verify correctly.");
                UpdateStatus("Your case file Notes data has failed to verify correctly.");
                MessageBox.Show("Tamper Warning!\r\nYour case file Notes data has failed to verify correctly.", "CaseNotes Tamper Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                
            }
            

            if (AuditValid)
                CaseFile.WriteAuditData("Success", "The case Audit logs have verified.");
            else
            {
                CaseValid = false;
                CaseFile.WriteAuditData("*******", " "); 
                CaseFile.WriteAuditData("Failure - Audit data", "The case file Audit data has failed to verify correctly.");
                CaseFile.WriteAuditData("Failure -           ", "Note that this warning will only appear once and the Audit log will re-validate from this point.");
                UpdateStatus("Your case file Audit data has failed to verify correctly.");
                MessageBox.Show("Tamper Warning!\r\nYour case file Audit data has failed to verify correctly.", "CaseNotes Tamper Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                
            }

            if (CaseValid)
            {
                CaseFile.WriteAuditData("Success", "The case file has been opened and verified successfully.");
                UpdateStatus("Your case file has verified successfully.");                
            }
            else
            {
                CaseFile.WriteAuditData("Failure", "The case file has been opened but has failed to verify successfully.");
                UpdateStatus("Your case file has failed to verify successfully.");                
            }

            DisplayAuditData();
            tabControl.Visible = true;
            EnableControls();
            Cursor = Cursors.Default;
        }

        private void DisplayMetaData()
        {
            RtfMain.Clear();
            RtfMain.SelectionStart = RtfMain.SelectionLength;
            RtfMain.SelectionFont = new Font(Prefs.MetadataTypeface, Prefs.MetadataTypeSize, FontStyle.Underline | FontStyle.Bold);
            var titleLength = 0;
            
            for (int i = 4; i <= CaseFile.MetaDataArray.Count - 5; i += 2)
            {
                if (CaseFile.MetaDataArray[i].Length > titleLength)
                    titleLength = CaseFile.MetaDataArray[i].Length;
            }

            RtfMain.SelectionTabs = new int[] { titleLength + 100 + (Prefs.MetadataTypeSize*5), 200, 300, 400 };
            RtfMain.SelectedText += CaseFile.MetaDataArray[1] + "\r\n";

            try
            {
                for (int i = 4; i <= CaseFile.MetaDataArray.Count - 5; i += 2)
                {
                    if (CaseFile.MetaDataArray[i].Length > titleLength)
                        titleLength = CaseFile.MetaDataArray[i].Length;

                    RtfMain.SelectionFont = new Font(Prefs.MetadataTypeface, Prefs.MetadataTypeSize, FontStyle.Bold);
                    RtfMain.SelectedText += CaseFile.MetaDataArray[i] + "\t";

                    RtfMain.SelectionFont = new Font(Prefs.MetadataTypeface, Prefs.MetadataTypeSize, FontStyle.Regular);
                    RtfMain.SelectedText += CaseFile.MetaDataArray[i + 1] + "\r\n";
                }

                RtfMain.SelectionFont = new Font(Prefs.MetadataTypeface, Prefs.MetadataTypeSize, FontStyle.Bold);
                RtfMain.SelectedText += "\r\nCase Started:\t";
                RtfMain.SelectionFont = new Font(Prefs.MetadataTypeface, Prefs.MetadataTypeSize, FontStyle.Regular);
                RtfMain.SelectedText += CaseFile.MetaDataArray[CaseFile.MetaDataArray.Count - 3];
                RtfMain.SelectionFont = new Font(Prefs.MetadataTypeface, Prefs.MetadataTypeSize, FontStyle.Bold);
                RtfMain.SelectedText += "\r\nLast Modified:\t";
                RtfMain.SelectionFont = new Font(Prefs.MetadataTypeface, Prefs.MetadataTypeSize, FontStyle.Regular);
                RtfMain.SelectedText += CaseFile.MetaDataArray[CaseFile.MetaDataArray.Count - 1] + "\r\n";
                RtfMain.SelectionFont = new Font(Prefs.MetadataTypeface, Prefs.MetadataTypeSize, FontStyle.Bold);
                RtfMain.SelectedText += "\r\nCase Status:\t";
                RtfMain.SelectionFont = new Font(Prefs.MetadataTypeface, Prefs.MetadataTypeSize, FontStyle.Regular);
                RtfMain.SelectedText += CaseFile.MetaDataArray[3] + "\r\n";

                if (!TabsDone) CreateTabs();

                DisplayNotesData();
                DisplayTabData();

                User1Dirty = false;
                User2Dirty = false;
                User3Dirty = false;
                User4Dirty = false;
                User5Dirty = false;
                User6Dirty = false;
                User7Dirty = false;
                User8Dirty = false;
                User9Dirty = false;
                User10Dirty = false;
                CheckListDirty = false;

                CheckDirty();
            }

            catch (Exception fail)
            {
                MessageBox.Show("DisplayMetaData - fail at Creating Tabs\r\n" + fail.Message);
            }
        }

        private void CreateTabs()
        {
            var tabWindows = Prefs.TabItems;

            var tabTitles = new string[tabWindows];
            if (tabWindows > 0) tabTitles[0] = CaseFile.TabMetaDataArray[1];
            if (tabWindows > 1) tabTitles[1] = CaseFile.TabMetaDataArray[3];
            if (tabWindows > 2) tabTitles[2] = CaseFile.TabMetaDataArray[5];
            if (tabWindows > 3) tabTitles[3] = CaseFile.TabMetaDataArray[7];
            if (tabWindows > 4) tabTitles[4] = CaseFile.TabMetaDataArray[9];
            if (tabWindows > 5) tabTitles[5] = CaseFile.TabMetaDataArray[11];
            if (tabWindows > 6) tabTitles[6] = CaseFile.TabMetaDataArray[13];
            if (tabWindows > 7) tabTitles[7] = CaseFile.TabMetaDataArray[15];
            if (tabWindows > 8) tabTitles[8] = CaseFile.TabMetaDataArray[17];
            if (tabWindows > 9) tabTitles[9] = CaseFile.TabMetaDataArray[19];

            TabPage notesTab = tabControl.SelectedTab;

            try
            {
                for (int i = 0; i < tabWindows; i++)
                {
                    var myTabPage = new TabPage(tabTitles[i]);
                    tabControl.TabPages.Add(myTabPage);
                    tabControl.SelectedTab = myTabPage;
                    myTabPage.Name = tabTitles[i];

                    var myRuler  = new TextRuler();
                    myRuler.Dock = DockStyle.Top;
                    myRuler.BackColor = Color.Transparent;
                    myRuler.Font = new Font("Arial", 7.25F);
                    myRuler.LeftHangingIndent = 0;
                    myRuler.LeftIndent = 0;
                    myRuler.LeftMargin = 0;
                    myRuler.Location = new Point(30, 78);
                    myRuler.Name = "Ruler";
                    myRuler.NoMargins = true;
                    myRuler.RightIndent = 20;
                    myRuler.RightMargin = 5;
                    myRuler.Size = new Size(654, 20);
                    myRuler.TabIndex = 8;
                    myRuler.TabsEnabled = true;
                    myRuler.TabAdded += RulerTabAdded;
                    myRuler.TabRemoved += RulerTabRemoved;
                    myRuler.TabChanged += RulerTabChanged;
                    myRuler.BothLeftIndentsChanged += RulerBothLeftIndentsChanged;
                    myRuler.RightIndentChanging += RulerRightIndentChanging;
                    myRuler.LeftHangingIndentChanging += RulerLeftHangingIndentChanging;
                    myRuler.LeftIndentChanging += RulerLeftIndentChanging;
                    
                    var myRTF = new RichTextBox();
                    myRTF.Dock = DockStyle.Fill;
                    myRTF.Name = "RTF" + i.ToString(CultureInfo.InvariantCulture);
                    myRTF.Font = new Font(Prefs.NoteTypeface, Prefs.NoteTypeSize, FontStyle.Regular);
                    myRTF.ScrollBars = RichTextBoxScrollBars.Both;
                    myRTF.AcceptsTab = true;
                    myRTF.HideSelection = false;
                    myRTF.AutoWordSelection = false;
                    myRTF.Clear();
                    myRTF.SelectionStart = myRTF.TextLength;
                    myRTF.SelectionFont = new Font("Arial", 10, FontStyle.Underline | FontStyle.Bold);
                    myRTF.SelectedText += tabTitles[i] + "\r\n\r\n";
                    myRTF.SelectionFont = new Font(Prefs.NoteTypeface, Prefs.NoteTypeSize, FontStyle.Regular);
                    myRTF.TextChanged += TextChanged;
                    myRTF.SelectionChanged += SelectionChanged;
                    myRTF.LinkClicked += MyRTFLinkClicked;
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
                    myRTF.SpellCheck().CurrentDictionary = dictionary;
                    myRTF.EnableSpellCheck();

                    myTabPage.Controls.AddRange(new Control[] { myRTF, myRuler });
                }
            }
            catch (Exception fail)
            {
                MessageBox.Show("An unexpected error has ocurred setting up your user tabs! The error reported is: \r\n"
                                + fail.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                if (!string.IsNullOrEmpty(CaseFile.ChecklistName))
                {
                    var clTabPage = new TabPage("Case Checklist");
                    tabControl.TabPages.Add(clTabPage);
                    tabControl.SelectedTab = clTabPage;
                    clTabPage.Name = "Checklist";
                    clPanel.BackColor = Color.White;
                    clPanel.Parent = clTabPage;
                    clPanel.Dock = DockStyle.Fill;
                    clPanel.Visible = true;
                    clPanel.BorderStyle = BorderStyle.Fixed3D;
                    ReadChecklistXML();
                }
            }
            catch (Exception fail)
            {
                MessageBox.Show("CreateTabs - fail at Creating Checklist Tab\r\n" + fail.Message);                
            }

            try
            {
                var adTabPage = new TabPage("Audit Log");
                tabControl.TabPages.Add(adTabPage);
                tabControl.SelectedTab = adTabPage;
                adTabPage.Name = "AuditRTF";
                var adRTF = new RichTextBox();
                adRTF.Dock = DockStyle.Fill;
                adRTF.Name = adTabPage.Name;
                adRTF.Font = new Font("Courier New", 10, FontStyle.Underline | FontStyle.Bold);
                adRTF.SelectedText += "CaseNotes Audit Log" + "\r\n\r\n";
                adRTF.SelectionFont = new Font(Prefs.NoteTypeface, Prefs.NoteTypeSize, FontStyle.Regular);
                adRTF.ReadOnly = true;
                adRTF.BackColor = Color.White;
                adRTF.ScrollBars = RichTextBoxScrollBars.Both;
                adTabPage.Controls.AddRange(new Control[] {adRTF});

                tabControl.SelectedTab = notesTab;
                TabsDone = true;
            }

            catch (Exception fail)
            {
                MessageBox.Show("CreateTabs - fail at Creating Audit Tab\r\n" + fail.Message);
            }
        }

        static void MyRTFLinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void RulerTabAdded(TextRuler.TabEventArgs args)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];
            var userRuler = (TextRuler)userTab.Controls[1]; 
            
            try
            {
                userRTF.SelectionTabs = userRuler.TabPositionsInPixels.ToArray();
            }
            catch (Exception fail)
            {
                MessageBox.Show("TabAdded:\r\n" + fail.Message);
            }
        }

        private void RulerTabChanged(TextRuler.TabEventArgs args)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];
            var userRuler = (TextRuler)userTab.Controls[1]; 

            try
            {
                userRTF.SelectionTabs = userRuler.TabPositionsInPixels.ToArray();
            }
            catch (Exception fail)
            {
                MessageBox.Show("TabChanged:\r\n" + fail.Message);
            }
        }

        private void RulerTabRemoved(TextRuler.TabEventArgs args)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];
            var userRuler = (TextRuler)userTab.Controls[1]; 

            try
            {
                userRTF.SelectionTabs = userRuler.TabPositionsInPixels.ToArray();
            }
            catch (Exception fail)
            {
                MessageBox.Show("TabRemoved:\r\n" + fail.Message);
            }
        }

        private void RulerBothLeftIndentsChanged(int leftIndent, int hangingIndent)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];
            var userRuler = (TextRuler)userTab.Controls[1];

            try
            {
                userRTF.SelectionIndent = (int) (leftIndent*userRuler.DotsPerMillimeter);
                userRTF.SelectionHangingIndent = (int) (hangingIndent*userRuler.DotsPerMillimeter) - (int) (leftIndent*userRuler.DotsPerMillimeter);
            }
            catch (Exception fail)
            {
                MessageBox.Show("RulerBothLeftIndentsChanged:\r\n" + fail.Message);
            }
        }

        private void RulerRightIndentChanging(int newValue)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];
            var userRuler = (TextRuler)userTab.Controls[1];

            try
            {
                userRTF.SelectionRightIndent = (int) (newValue*userRuler.DotsPerMillimeter);
            }
            catch (Exception fail)
            {
                MessageBox.Show("RulerRightIndentChanging:\r\n" + fail.Message);
            }
        }

        private void RulerLeftIndentChanging(int newValue)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];
            var userRuler = (TextRuler)userTab.Controls[1];

            try
            {
                userRTF.SelectionIndent = (int)(newValue * userRuler.DotsPerMillimeter);
                userRTF.SelectionHangingIndent = (int)(userRuler.LeftHangingIndent * userRuler.DotsPerMillimeter) - (int)(newValue * userRuler.DotsPerMillimeter);
            }
            catch (Exception fail)
            {
                MessageBox.Show("RulerLeftIndentChanging:\r\n" + fail.Message);
            }
        }

        private void RulerLeftHangingIndentChanging(int newValue)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];
            var userRuler = (TextRuler)userTab.Controls[1];

            try
            {
                userRTF.SelectionHangingIndent = (int)(newValue * userRuler.DotsPerMillimeter) - (int)(userRuler.LeftIndent * userRuler.DotsPerMillimeter);
            }
            catch (Exception fail)
            {
                MessageBox.Show("RulerLeftHangingIndentChanging:\r\n" + fail.Message);
            }
        }

        private void DeleteTabs()
        {
            try
            {
                for (int i = tabControl.TabPages.Count - 1; i > 0; i--)
                {
                    tabControl.TabPages.RemoveAt(1);
                }

                if (tabControl.TabCount > 1)
                    tabControl.TabPages.RemoveAt(1);

                TabsDone = false;
            }
            catch (Exception fail)
            {
                MessageBox.Show("DeleteTabs - fail at Deleting Tabs\r\n" + fail.Message);
            }
        }

        private void DisplayNotesData()
        {
            try
            {
                var note = new Note();
                Task notes = Task.Factory.StartNew(() =>
                                                    {
                                                        NotesValid = note.ReadNotes(CaseFileDB, CaseFile.FilePass);
                                                    });

                Task.WaitAll(notes);

                var noteArray = note.NoteArray;
                var tempRtf = new RichTextBox();

                foreach (var caseNote in noteArray)
                {
                    var format = "yyyy-MMM-dd HH:mm:ss";
                    var temp = Convert.ToDateTime(caseNote.Entered);
                    var timeStamp = temp.ToString(format);

                    tempRtf.Rtf = Functions.Byte2String(caseNote.CaseNote);
                    tempRtf.SelectionStart = 0;
                    tempRtf.SelectionIndent = 0;
                    tempRtf.SelectionBullet = false;
                    tempRtf.SelectionColor = Color.Black;
                    tempRtf.SelectionFont = new Font(Prefs.MetadataTypeface, Prefs.MetadataTypeSize, FontStyle.Underline | FontStyle.Bold);
                    tempRtf.SelectedText += "\r\n" + timeStamp;
                    tempRtf.SelectionFont = new Font(Prefs.MetadataTypeface, Prefs.MetadataTypeSize, FontStyle.Regular);
                    tempRtf.SelectedText += "\r\n\r\n";

                    Clipboard.Clear();
                    tempRtf.SelectAll();
                    tempRtf.Cut();

                    IDataObject iData = new DataObject();
                    iData = Clipboard.GetDataObject();

                    RtfMain.ReadOnly = false;
                    RtfMain.SelectionStart = RtfMain.Rtf.Length;
                    RtfMain.Paste();
                    RtfMain.SelectionFont = new Font(Prefs.MetadataTypeface, Prefs.MetadataTypeSize, FontStyle.Underline | FontStyle.Bold);
                    RtfMain.SelectedText += "\r\n";
                    RtfMain.ReadOnly = true;
                }
                note.NoteArray.Clear();
            }

            catch (Exception)
            {
            }
        }

        private void DisplayTabData()
        {
            var dataPresent = false;

            Task tabs = Task.Factory.StartNew(() =>
                                                {
                                                    dataPresent = CaseFile.ReadTabData();
                                                });

            Task.WaitAll(tabs);

            if (dataPresent)
            {
                try
                {

                    if (Prefs.TabItems > 0)
                    {
                        var userTab = tabControl.TabPages[1];
                        var userRTF = (RichTextBox) userTab.Controls[0];
                        userRTF.Rtf = CaseFile.TabDataArray[2];
                    }

                    if (Prefs.TabItems > 1)
                    {
                        var userTab = tabControl.TabPages[2];
                        var userRTF = (RichTextBox) userTab.Controls[0];
                        userRTF.Rtf = CaseFile.TabDataArray[5];
                    }

                    if (Prefs.TabItems > 2)
                    {
                        var userTab = tabControl.TabPages[3];
                        var userRTF = (RichTextBox) userTab.Controls[0];
                        userRTF.Rtf = CaseFile.TabDataArray[8];
                    }

                    if (Prefs.TabItems > 3)
                    {
                        var userTab = tabControl.TabPages[4];
                        var userRTF = (RichTextBox) userTab.Controls[0];
                        userRTF.Rtf = CaseFile.TabDataArray[11];
                    }

                    if (Prefs.TabItems > 4)
                    {
                        var userTab = tabControl.TabPages[5];
                        var userRTF = (RichTextBox) userTab.Controls[0];
                        userRTF.Rtf = CaseFile.TabDataArray[14];
                    }

                    if (Prefs.TabItems > 5)
                    {
                        var userTab = tabControl.TabPages[6];
                        var userRTF = (RichTextBox) userTab.Controls[0];
                        userRTF.Rtf = CaseFile.TabDataArray[17];
                    }

                    if (Prefs.TabItems > 6)
                    {
                        var userTab = tabControl.TabPages[7];
                        var userRTF = (RichTextBox) userTab.Controls[0];
                        userRTF.Rtf = CaseFile.TabDataArray[20];
                    }

                    if (Prefs.TabItems > 7)
                    {
                        var userTab = tabControl.TabPages[8];
                        var userRTF = (RichTextBox) userTab.Controls[0];
                        userRTF.Rtf = CaseFile.TabDataArray[23];
                    }

                    if (Prefs.TabItems > 8)
                    {
                        var userTab = tabControl.TabPages[9];
                        var userRTF = (RichTextBox) userTab.Controls[0];
                        userRTF.Rtf = CaseFile.TabDataArray[26];
                    }

                    if (Prefs.TabItems > 9)
                    {
                        var userTab = tabControl.TabPages[10];
                        var userRTF = (RichTextBox) userTab.Controls[0];
                        userRTF.Rtf = CaseFile.TabDataArray[29];
                    }

                    CaseFile.TabDataArray.Clear();
                }

                catch (ArgumentOutOfRangeException)
                { }

                catch (Exception fail)
                {
                    MessageBox.Show("MainForm: DisplayTabData\r\n" + fail.Message);
                }
            }
        }

        private void DisplayAuditData()
        {
            var audit = Task.Factory.StartNew(() =>
                                                 {
                                                    AuditValid = CaseFile.ReadAuditData();
                                                 });

            var auditTab = tabControl.TabPages[tabControl.TabCount-1];
            var auditRTF = (RichTextBox)auditTab.Controls[0];

            Task.WaitAll(audit);
            
            auditRTF.Text = "";
            auditRTF.Font = new Font("Courier New", 10, FontStyle.Underline | FontStyle.Bold);
            auditRTF.SelectedText = "CaseNotes Audit Log" + "\r\n\r\n";
            auditRTF.SelectionFont = new Font("Courier New", 9, FontStyle.Regular);

            foreach (var entry in CaseFile.AuditArray)
            {
                auditRTF.SelectionFont = new Font("Courier New", 9, FontStyle.Regular);
                if (entry.Contains("Failure")) auditRTF.SelectionColor = Color.Red;
                else if (entry.Contains("Warning")) auditRTF.SelectionColor = Color.Orange;
                else auditRTF.SelectionColor = Color.Black;
                auditRTF.SelectedText += entry + "\r\n";
            }
            
        }
        
        private void PreferencesToolStripMenuItemClick(object sender, EventArgs e)
        {
            tsbPrefs.PerformClick();
        }

        private void TsbPrefsClick(object sender, EventArgs e)
        {
            bool caseOpen = !string.IsNullOrEmpty(CaseFileDB);
            var newPrefs = new UpdatePrefs(CaseNotesDB, caseOpen);
            newPrefs.ShowDialog();

            var prefs = new Preferences();
            prefs.ReadSystemPrefs(CaseNotesDB, null, "open");
            Prefs = prefs;
            newPrefs = null;
        }

        private void NewNoteToolStripMenuItemClick(object sender, EventArgs e)
        {
            tsbNewNote.PerformClick();
        }
        
        private void TsbNewNoteClick(object sender, EventArgs e)
        {
            var CaseOK = Functions.ValidateCaseFile(CaseFileDB, CaseFile.FilePass);
            if (CaseOK != "success")
            {
                MessageBox.Show("There is a problem with your case file.\r\n" + CaseOK + "\r\n\r\nPlease rectify this error and try again.", "Case File Access Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var newNote = new NewNote(CaseNotesDB, CaseFileDB, CaseFile.FilePass);
            var result = newNote.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                newNote.Dispose();
                return;
            }
                
            IDataObject iData = new DataObject();
            iData = Clipboard.GetDataObject();

            var note = new Note();
            note.Entered = DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss");
            note.Occurred = newNote.Occurred;

            var temp = (string)iData.GetData(DataFormats.Rtf);

            try
            {
                note.CaseNote = Functions.String2Byte(temp);
                if (note.WriteNote(CaseFileDB, CaseFile.FilePass))
                    CaseFile.WriteAuditData("Success", "A new contemporaneous note has been entered.");
                else
                    CaseFile.WriteAuditData("Failure", "A new contemporaneous note was entered, but could not be saved.");

                Clipboard.Clear();
                UserInit = false;
                tsbSaveCase.PerformClick();
                CaseFile.CaseUpdated();

                DisplayMetaData();
                DisplayAuditData();
            }
            catch (Exception)
            {
                MessageBox.Show("Your Case File can no longer be accessed (has it moved?)\r\nThe Case File will now close.", "Case File Access Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                closeToolStripMenuItem.PerformClick();
            }
        }

        private void CaseMetadataToolStripMenuItemClick(object sender, EventArgs e)
        {
            tsbMetadata.PerformClick();
        }

        private void TsbMetadataClick(object sender, EventArgs e)
        {
            var CaseOK = Functions.ValidateCaseFile(CaseFile.CaseFileDB, CaseFile.FilePass);
            if (CaseOK != "success")
            {
                MessageBox.Show("There is a problem with your case file.\r\n" + CaseOK + "\r\n\r\nPlease rectify this error and try again.", "Case File Access Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            var metaUpdate = new Case(CaseFile.CaseFileDB, "modify", CaseFile.FilePass);
            metaUpdate.FilePass = CaseFile.FilePass;
            
            var result = metaUpdate.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                CaseFile.FilePass = metaUpdate.FilePass;
                return;
            }

            CaseFile.FilePass = metaUpdate.FilePass;

            if (CaseFile != null)
            {
                UserInit = false;
                tsbSaveCase.PerformClick();
                UserInit = false;
                closeToolStripMenuItem.PerformClick();
            }

            CaseFile = metaUpdate;

            var prefs = new Preferences();
            prefs.ReadSystemPrefs(CaseFile.CaseFileDB, CaseFile.FilePass, "open");
            prefs.ReadUserPrefs(CaseFile.CaseFileDB, CaseFile.FilePass);

            Prefs = prefs;
            CaseFileDB = CaseFile.CaseFileDB;

            DisplayMetaData();
            DisplayAuditData();
            tabControl.Visible = true;
            EnableControls();
        }    

        private void EnableControls()
        {
            tsbSaveCase.Enabled = true;
            tsbPrintCase.Enabled = true;
            tsbNewNote.Enabled = true;
            tsbMetadata.Enabled = true;
            tsbFind.Enabled = true;                        
            closeToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            printToolStripMenuItem.Enabled = true;
            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = true;
            cutToolStripMenuItem.Enabled = true;
            copyToolStripMenuItem.Enabled = true;
            pasteToolStripMenuItem.Enabled = true;
            selectAllToolStripMenuItem.Enabled = true;
            caseMetadataToolStripMenuItem.Enabled = true;
            findToolStripMenuItem.Enabled = true;       
            newNoteToolStripMenuItem.Enabled = true;
            RtfMain.Visible = true;
            RtfMain.Enabled = true;
            tabControl.Visible = true;
            Text = " CaseNotes: " + CaseFile.CaseName;
            Cursor = Cursors.Default;
        }

        private void DisableControls()
        {
            tsbSaveCase.Enabled = false;
            tsbPrintCase.Enabled = false;
            tsbNewNote.Enabled = false;
            tsbMetadata.Enabled = false;
            tsbFind.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            printToolStripMenuItem.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false; 
            cutToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem.Enabled = false;
            pasteToolStripMenuItem.Enabled = false;
            selectAllToolStripMenuItem.Enabled = false;
            caseMetadataToolStripMenuItem.Enabled = false;
            findToolStripMenuItem.Enabled = false;
            newNoteToolStripMenuItem.Enabled = false;
            RtfMain.Clear();
            RtfMain.Visible = false;
            RtfMain.Enabled = false;
            tabControl.Visible = false;
            _controlList.Clear();
            clPanel.Controls.Clear();
            if (Professional)
                Text = " CaseNotes Professional version " + Assembly.GetExecutingAssembly().GetName().Version;
            else
                Text = " CaseNotes Lite version " + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void DisableRtfControls()
        {
            tsbCheckSpelling.Visible = false;
            tsbShowRuler.Visible = false;
            tsbCut.Visible = false;
            tsbCopy.Visible = false;
            tsbPaste.Visible = false;
            tsbBold.Visible = false;
            tsbItalic.Visible = false;
            tsbUnderline.Visible = false;
            tsbAlignLeft.Visible = false;
            tsbAlignCenter.Visible = false;
            tsbAlignRight.Visible = false;
            tsbBullets.Visible = false;
            tsbOutdent.Visible = false;
            tsbIndent.Visible = false;
            tsbFontButton.Visible = false;
            tsbSizeButton.Visible = false;
            tsbFontColour.Visible = false;
            tsbHighlight.Visible = false;
            toolStripButton14.Visible = false;
            toolStripButton8.Visible = false;
            toolStripButton7.Visible = false;
            toolStripButton12.Visible = false;
            toolStripSeparator5.Visible = false;
            toolStripSeparator6.Visible = false;
            toolStripSeparator7.Visible = false;
            toolStripSeparator8.Visible = false;
        }

        private void EnableRtfControls()
        {
            try
            {
                var userTab = tabControl.TabPages[tabControl.SelectedIndex];
                var userRTF = (RichTextBox)userTab.Controls[0];
                var userRuler = (TextRuler)userTab.Controls[1];

                tsbShowRuler.Checked = userRuler.Visible;
                tsbCheckSpelling.Checked = userRTF.IsSpellCheckEnabled();
            }
            catch {}

            tsbCheckSpelling.Visible = true;
            tsbShowRuler.Visible = true;
            tsbCut.Visible = true;
            tsbCopy.Visible = true;
            tsbPaste.Visible = true;
            tsbBold.Visible = true;
            tsbItalic.Visible = true;
            tsbUnderline.Visible = true;
            tsbAlignLeft.Visible = true;
            tsbAlignCenter.Visible = true;
            tsbAlignRight.Visible = true;
            tsbBullets.Visible = true;
            tsbOutdent.Visible = true;
            tsbIndent.Visible = true;
            tsbFontButton.Visible = true;
            tsbSizeButton.Visible = true;
            tsbFontColour.Visible = true;
            tsbHighlight.Visible = true;
            toolStripButton14.Visible = true;
            toolStripButton8.Visible = true;
            toolStripButton7.Visible = true;
            toolStripButton12.Visible = true;
            toolStripSeparator5.Visible = true;
            toolStripSeparator6.Visible = true;
            toolStripSeparator7.Visible = true;
            toolStripSeparator8.Visible = true;
        }

        private void UpdateStatus(string strMessage)
        {
            caseStatusStrip.Text = strMessage;
        }

        private void OnMruFile(int number, String filename)
        {
            Args = filename;
            MruMenu.SetFirstFile(number);
            tsbOpenCase.PerformClick();
        }

        private void CNpopup(object sender, EventArgs e)
        {
            var miClicked = (MenuItem)sender;
            var item = miClicked.Text;
            if (item == "Copy")
            {
                Clipboard.SetDataObject(RtfMain.SelectedText, true);
                UpdateStatus("Selected data copied to the clipboard");
            }
            if (item == "Select All")
            {
                RtfMain.Focus();
                RtfMain.SelectAll();
                UpdateStatus("Select all data");
            }
            Invalidate();
        }

        private void SelectAll(object sender, EventArgs eventArgs)
        {
            var thisTab = tabControl.SelectedTab;
            var thisRTF = (RichTextBox)thisTab.Controls[0];

            thisRTF.Focus();
            thisRTF.SelectAll();
            UpdateStatus("Select all data");
        }

        private new void TextChanged(object sender, System.EventArgs e)
        {
            var whichRTF = (RichTextBox)sender;
            int a = Convert.ToInt16(whichRTF.Name.Substring(3, 1));

            switch (a)
            {
                case 0:
                    User1Dirty = true;
                    break;
                case 1:
                    User2Dirty = true;
                    break;
                case 2:
                    User3Dirty = true;
                    break;
                case 3:
                    User4Dirty = true;
                    break;
                case 4:
                    User5Dirty = true;
                    break;
                case 5:
                    User6Dirty = true;
                    break;
                case 6:
                    User7Dirty = true;
                    break;
                case 7:
                    User8Dirty = true;
                    break;
                case 8:
                    User9Dirty = true;
                    break;
                case 9:
                    User10Dirty = true;
                    break;
            }

            CheckDirty();
        }

        private void CheckDirty()
        {
            var tabs = Prefs.TabItems;

            try
            {
                if (tabs > 0)
                {
                    var user1Tab = tabControl.TabPages[1];
                    if (User1Dirty)
                    {
                        if (!user1Tab.Text.EndsWith("*"))
                            user1Tab.Text = user1Tab.Text + "*";
                    }
                    else
                    {
                        if (user1Tab.Text.EndsWith("*"))
                            user1Tab.Text = user1Tab.Text.Substring(0, user1Tab.Text.Length - 1);
                    }
                }

                if (tabs > 1)
                {
                    var user2Tab = tabControl.TabPages[2];
                    if (User2Dirty)
                    {
                        if (!user2Tab.Text.EndsWith("*"))
                            user2Tab.Text = user2Tab.Text + "*";
                    }
                    else
                    {
                        if (user2Tab.Text.EndsWith("*"))
                            user2Tab.Text = user2Tab.Text.Substring(0, user2Tab.Text.Length - 1);
                    }
                }

                if (tabs > 2)
                {
                    var user3Tab = tabControl.TabPages[3];
                    if (User3Dirty)
                    {
                        if (!user3Tab.Text.EndsWith("*"))
                            user3Tab.Text = user3Tab.Text + "*";
                    }
                    else
                    {
                        if (user3Tab.Text.EndsWith("*"))
                            user3Tab.Text = user3Tab.Text.Substring(0, user3Tab.Text.Length - 1);
                    }
                }

                if (tabs > 3)
                {
                    var user4Tab = tabControl.TabPages[4];
                    if (User4Dirty)
                    {
                        if (!user4Tab.Text.EndsWith("*"))
                            user4Tab.Text = user4Tab.Text + "*";
                    }
                    else
                    {
                        if (user4Tab.Text.EndsWith("*"))
                            user4Tab.Text = user4Tab.Text.Substring(0, user4Tab.Text.Length - 1);
                    }
                }

                if (tabs > 4)
                {
                    var user5Tab = tabControl.TabPages[5];
                    if (User5Dirty)
                    {
                        if (!user5Tab.Text.EndsWith("*"))
                            user5Tab.Text = user5Tab.Text + "*";
                    }
                    else
                    {
                        if (user5Tab.Text.EndsWith("*"))
                            user5Tab.Text = user5Tab.Text.Substring(0, user5Tab.Text.Length - 1);
                    }
                }

                if (tabs > 5)
                {
                    var user6Tab = tabControl.TabPages[6];
                    if (User6Dirty)
                    {
                        if (!user6Tab.Text.EndsWith("*"))
                            user6Tab.Text = user6Tab.Text + "*";
                    }
                    else
                    {
                        if (user6Tab.Text.EndsWith("*"))
                            user6Tab.Text = user6Tab.Text.Substring(0, user6Tab.Text.Length - 1);
                    }
                }

                if (tabs > 6)
                {
                    var user7Tab = tabControl.TabPages[7];
                    if (User7Dirty)
                    {
                        if (!user7Tab.Text.EndsWith("*"))
                            user7Tab.Text = user7Tab.Text + "*";
                    }
                    else
                    {
                        if (user7Tab.Text.EndsWith("*"))
                            user7Tab.Text = user7Tab.Text.Substring(0, user7Tab.Text.Length - 1);
                    }
                }

                if (tabs > 7)
                {
                    var user8Tab = tabControl.TabPages[8];
                    if (User8Dirty)
                    {
                        if (!user8Tab.Text.EndsWith("*"))
                            user8Tab.Text = user8Tab.Text + "*";
                    }
                    else
                    {
                        if (user8Tab.Text.EndsWith("*"))
                            user8Tab.Text = user8Tab.Text.Substring(0, user8Tab.Text.Length - 1);
                    }
                }

                if (tabs > 8)
                {
                    var user9Tab = tabControl.TabPages[9];
                    if (User9Dirty)
                    {
                        if (!user9Tab.Text.EndsWith("*"))
                            user9Tab.Text = user9Tab.Text + "*";
                    }
                    else
                    {
                        if (user9Tab.Text.EndsWith("*"))
                            user9Tab.Text = user9Tab.Text.Substring(0, user9Tab.Text.Length - 1);
                    }
                }

                if (tabs > 9)
                {
                    var user10Tab = tabControl.TabPages[10];
                    if (User10Dirty)
                    {
                        if (!user10Tab.Text.EndsWith("*"))
                            user10Tab.Text = user10Tab.Text + "*";
                    }
                    else
                    {
                        if (user10Tab.Text.EndsWith("*"))
                            user10Tab.Text = user10Tab.Text.Substring(0, user10Tab.Text.Length - 1);
                    }
                }

                if (!string.IsNullOrEmpty(CaseFile.ChecklistName))
                {

                    var checkTab = tabControl.TabPages[tabs + 1];
                    if (CheckListDirty)
                    {
                        if (!checkTab.Text.EndsWith("*"))
                            checkTab.Text = checkTab.Text + "*";
                    }
                    else
                    {
                        if (checkTab.Text.EndsWith("*"))
                            checkTab.Text = checkTab.Text.Substring(0, checkTab.Text.Length - 1);
                    }
                }
            }

            catch{}
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];
            var currentFont = userRTF.SelectionFont;

            try
            {
                var userRuler = (TextRuler)userTab.Controls[1];

                userRuler.SetTabPositionsInPixels(userRTF.SelectionTabs);

                if (userRTF.SelectionLength < userRTF.TextLength - 1)
                {
                    userRuler.LeftIndent = (int)(userRTF.SelectionIndent / userRuler.DotsPerMillimeter);
                    userRuler.LeftHangingIndent = (int)((float)userRTF.SelectionHangingIndent / userRuler.DotsPerMillimeter) + userRuler.LeftIndent;
                    userRuler.RightIndent = (int)(userRTF.SelectionRightIndent / userRuler.DotsPerMillimeter);
                }
            }
            catch (ArgumentOutOfRangeException)
            { }
            
            try
            {
                tsbFontButton.SelectedItem = currentFont.Name;
                tsbSizeButton.SelectedItem = currentFont.Size.ToString();
                tsbBold.Checked = currentFont.Bold;
                tsbItalic.Checked = currentFont.Italic;
                tsbUnderline.Checked = currentFont.Underline;
            }
            catch (NullReferenceException)
            { }

            catch (Exception fail)
            {
                MessageBox.Show("SelectionChanged:\r\n" + fail.Message);
            }
        }

        private void TabButtonChanged(object sender, EventArgs e)
        {
            if ((tabControl.SelectedIndex == 0) || (tabControl.SelectedTab.Name == "Checklist") || (tabControl.SelectedTab.Name == "AuditRTF"))
                DisableRtfControls();
            else
                EnableRtfControls();
        }

        private void CloseToolStripMenuItemClick(object sender, EventArgs e)
        {
            UpdateStatus("");
            if (User1Dirty | User2Dirty | User3Dirty | User4Dirty | User5Dirty | User6Dirty | User7Dirty | User8Dirty | User9Dirty | User10Dirty | CheckListDirty)
            {
                var dr = MessageBox.Show("Do you want to save the changes to your case file?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    UserInit = true;
                    tsbSaveCase.PerformClick();
                }
            }
            if (UserInit) CaseFile.WriteAuditData("-------", "CaseFile closed.");
            UserInit = true;
            MruMenu.SaveToRegistry();
            CaseFileDB = "";
            CaseFile = null;
            DeleteTabs();
            DisableControls();
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateStatus("");
            if (User1Dirty | User2Dirty | User3Dirty | User4Dirty | User5Dirty | User6Dirty | User7Dirty | User8Dirty | User9Dirty | User10Dirty | CheckListDirty )
            {
                var dr = MessageBox.Show("Do you want to save the changes to your case file?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    UserInit = true;
                    tsbSaveCase.PerformClick();
                }
            }

            var prefs = new Preferences {AppLocation = Location, AppSize = Size };
            prefs.WriteMainLocation(CaseNotesDB, null);
            MruMenu.SaveToRegistry();

            Application.DoEvents();
        }

        private void ClearRecentFileListToolStripMenuItemClick(object sender, EventArgs e)
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey.DeleteSubKey(_mruRegKey + "\\MRU", false);
            MruMenu.RemoveAll();
            UpdateStatus("Your recent file list has been cleared");
        }

        private void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            UserInit = true;
            tsbSaveCase.PerformClick();
        }
        
        private void TsbSaveCaseClick(object sender, EventArgs e)
        {
            var CaseOK = Functions.ValidateCaseFile(CaseFileDB, CaseFile.FilePass);
            if (CaseOK != "success")
            {
                MessageBox.Show("There is a problem with your case file.\r\n" + CaseOK + "\r\n\r\nPlease rectify this error and try again.", "Case File Save Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (CaseFile.TabDataArray != null)
                CaseFile.TabDataArray.Clear();

            var tabDataArray = new List<string>();

            for (int i = 1; i <= tabControl.TabCount-2 ; i++)
            {
                if (tabControl.TabPages[i].Text.Contains("Audit Log") || tabControl.TabPages[i].Text.Contains("Case Checklist"))
                    break;
                
                var userTab = tabControl.TabPages[i];
                var userRTF = (RichTextBox)userTab.Controls[0];
                tabDataArray.Add(i.ToString());
                tabDataArray.Add(tabControl.TabPages[i].Text);
                tabDataArray.Add(userRTF.Rtf);
            }

            if (!string.IsNullOrEmpty(CaseFile.ChecklistName)) WriteChecklistXML();

            CaseFile.TabDataArray = tabDataArray;
            CaseFile.WriteTabData();
            User1Dirty = false;
            User2Dirty = false;
            User3Dirty = false;
            User4Dirty = false;
            User5Dirty = false;
            User6Dirty = false;
            User7Dirty = false;
            User8Dirty = false;
            User9Dirty = false;
            User10Dirty = false;
            CheckListDirty = false;
            CheckDirty();
            if (UserInit)
                CaseFile.WriteAuditData("Success", "User initiated CaseFile 'save' initiated.");
            else
                CaseFile.WriteAuditData("Success", "CaseFile 'save' initiated.");

            UserInit = true;
        }

        private void ReadChecklistXML()
        {
            Gui = new GuiController();
            Gui.ControlList.Clear();
            var param = new List<string>();
            var picture = false;
            var PicParam = new List<string>();
            var xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreWhitespace = true;

            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(CaseFile.ChecklistXML), xmlReaderSettings))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            if (reader.Name == "Checklist")
                            {
                                Gui.Name = reader["Name"];
                                Gui.Description = reader["Description"];
                                Gui.Author = reader["Author"];
                                Gui.Created = reader["Created"];
                                Gui.Modified = reader["Modified"];
                                Gui.Version = reader["Version"];
                            }

                            if (reader.Name == "Control")
                            {
                                param.Clear();
                                param.Add(reader["Name"]);
                                reader.Read();
                                reader.Read();
                                switch (reader.Value)
                                {
                                    #region XLabel

                                    case "Label":
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Location"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Size"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Text"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["FontSize"]);

                                        AddLabel(param);
                                        break;

                                    #endregion

                                    #region XComboBox

                                    case "ComboBox":
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Location"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Size"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Text"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();

                                        int count = 0;
                                        int.TryParse(reader.Value, out count);
                                        param.Add(count.ToString());

                                        for (var i = 1; i <= count; i++)
                                        {
                                            reader.Read();
                                            reader.Read();
                                            reader.Read();
                                            param.Add(reader.Value);
                                        }
                                        AddCombo(param);
                                        break;

                                    #endregion

                                    #region XListBox

                                    case "ListBox":
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Location"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Size"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Text"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();

                                        count = 0;
                                        int.TryParse(reader.Value, out count);
                                        param.Add(count.ToString());

                                        for (var i = 1; i <= count; i++)
                                        {
                                            reader.Read();
                                            reader.Read();
                                            reader.Read();
                                            param.Add(reader.Value);
                                        }
                                        AddList(param);
                                        break;

                                    #endregion

                                    #region XTextBox

                                    case "TextBox":
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Location"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Size"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Text"]);

                                        AddTextBox(param);
                                        break;

                                    #endregion

                                    #region XPictureBox

                                    case "PictureBox":
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Location"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Size"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Text"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Path"]);

                                        picture = true;
                                        PicParam = param;
                                        AddPicture(param);
                                        picture = false;
                                        break;

                                    #endregion

                                    #region XRadioButton

                                    case "RadioButton":
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Location"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Size"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Text"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Checked"]);

                                        AddRadio(param);
                                        break;

                                    #endregion

                                    #region XCheckBox

                                    case "CheckBox":
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Location"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Size"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Text"]);
                                        reader.Read();
                                        reader.Read();
                                        reader.Read();
                                        param.Add(reader.Value); //["Checked"]);

                                        AddCheck(param);
                                        break;

                                    #endregion

                                }
                            }
                        }
                    }
                }
            }

            catch (Exception crap)
            {
                if (picture)
                    MessageBox.Show("An expected picture or graphic described in your Checklist cannot be found.\r\n\r\n" + PicParam[4], "Checklist XML Read Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show("This doesn't appear to be a valid CaseNotes Checklist file.", "Checklist XML Read Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AddLabel(List<String> param)
        {
            var lbl = new Label();
            lbl.Parent = clPanel;
            lbl.Name = param[0];

            int x = 1, y = 1;
            var pos = param[1].IndexOf(",");
            int.TryParse(param[1].Substring(0, pos), out x);
            int.TryParse(param[1].Substring(pos + 1), out y);
            lbl.Location = new Point(x, y);

            x = 1; y = 1;
            pos = param[2].IndexOf(",");
            int.TryParse(param[2].Substring(0, pos), out x);
            int.TryParse(param[2].Substring(pos + 1), out y);
            lbl.Size = new Size(x, y);
            lbl.AutoSize = true;

            lbl.Text = param[3];

            float size = 8.25F;
            float.TryParse(param[4], out size);
            if (size < 1) size = 8.25F;
            lbl.Font = new Font(lbl.Font.FontFamily, size);

            _controlList.Add(lbl);
        }

        private void AddCombo(List<String> param)
        {
            var count = 0;
            var cbo = new ComboBox();
            cbo.Parent = clPanel;
            cbo.Name = param[0];

            int x = 1, y = 1;
            var pos = param[1].IndexOf(",");
            int.TryParse(param[1].Substring(0, pos), out x);
            int.TryParse(param[1].Substring(pos + 1), out y);
            cbo.Location = new Point(x, y);

            x = 1; y = 1;
            pos = param[2].IndexOf(",");
            int.TryParse(param[2].Substring(0, pos), out x);
            int.TryParse(param[2].Substring(pos + 1), out y);
            cbo.Size = new Size(x, y);
            cbo.AutoSize = true;

            cbo.Text = param[3];
            int.TryParse(param[4], out count);

            for (int i = 5; i < count + 5; i++)
            {
                cbo.Items.Add(param[i]);
            }

            cbo.SelectedIndexChanged += CheckListChanged;
            _controlList.Add(cbo);
        }

        private void AddList(List<String> param)
        {
            var count = 0;
            var lbx = new ListBox();
            lbx.Parent = clPanel;
            lbx.Name = param[0];

            int x = 1, y = 1;
            var pos = param[1].IndexOf(",");
            int.TryParse(param[1].Substring(0, pos), out x);
            int.TryParse(param[1].Substring(pos + 1), out y);
            lbx.Location = new Point(x, y);

            x = 1; y = 1;
            pos = param[2].IndexOf(",");
            int.TryParse(param[2].Substring(0, pos), out x);
            int.TryParse(param[2].Substring(pos + 1), out y);
            lbx.Size = new Size(x, y);

            lbx.Text = param[3];
            int.TryParse(param[4], out count);

            for (int i = 5; i < count + 5; i++)
            {
                lbx.Items.Add(param[i]);
                if (param[i] == param[3])
                    lbx.SelectedIndex = i - 5;
            }

            lbx.SelectedIndexChanged += CheckListChanged;
            _controlList.Add(lbx);
        }

        private void AddTextBox(List<String> param)
        {
            var txb = new TextBox();
            txb.Parent = clPanel;
            txb.Name = param[0];

            int x = 1, y = 1;
            var pos = param[1].IndexOf(",");
            int.TryParse(param[1].Substring(0, pos), out x);
            int.TryParse(param[1].Substring(pos + 1), out y);
            txb.Location = new Point(x, y);

            x = 1; y = 1;
            pos = param[2].IndexOf(",");
            int.TryParse(param[2].Substring(0, pos), out x);
            int.TryParse(param[2].Substring(pos + 1), out y);
            txb.Size = new Size(x, y);
            txb.AutoSize = true;

            txb.Text = param[3];

            txb.TextChanged += CheckListChanged;
            _controlList.Add(txb);
        }

        private void AddPicture(List<String> param)
        {
            var pic = new PictureBox();
            pic.Parent = clPanel;
            pic.Name = param[0];

            int x = 1, y = 1;
            var pos = param[1].IndexOf(",");
            int.TryParse(param[1].Substring(0, pos), out x);
            int.TryParse(param[1].Substring(pos + 1), out y);
            pic.Location = new Point(x, y);

            x = 1; y = 1;
            pos = param[2].IndexOf(",");
            int.TryParse(param[2].Substring(0, pos), out x);
            int.TryParse(param[2].Substring(pos + 1), out y);
            pic.Size = new Size(x, y);
            pic.AutoSize = true;
            pic.Text = param[3];
            pic.AccessibleName = param[4];
            pic.Image = new Bitmap(param[4]);

            _controlList.Add(pic);

        }

        private void AddRadio(List<String> param)
        {
            var rdb = new RadioButton();
            rdb.Parent = clPanel;
            rdb.Name = param[0];

            int x = 1, y = 1;
            var pos = param[1].IndexOf(",");
            int.TryParse(param[1].Substring(0, pos), out x);
            int.TryParse(param[1].Substring(pos + 1), out y);
            rdb.Location = new Point(x, y);

            x = 1; y = 1;
            pos = param[2].IndexOf(",");
            int.TryParse(param[2].Substring(0, pos), out x);
            int.TryParse(param[2].Substring(pos + 1), out y);
            rdb.Size = new Size(x, y);
            rdb.AutoSize = true;
            rdb.Text = param[3];
            rdb.Checked = param[4] == "True";

            rdb.CheckedChanged += CheckListChanged;
            _controlList.Add(rdb);
        }

        private void AddCheck(List<String> param)
        {
            var cbx = new CheckBox();
            cbx.Parent = clPanel;
            cbx.Name = param[0];

            int x = 1, y = 1;
            var pos = param[1].IndexOf(",");
            int.TryParse(param[1].Substring(0, pos), out x);
            int.TryParse(param[1].Substring(pos + 1), out y);
            cbx.Location = new Point(x, y);

            x = 1; y = 1;
            pos = param[2].IndexOf(",");
            int.TryParse(param[2].Substring(0, pos), out x);
            int.TryParse(param[2].Substring(pos + 1), out y);
            cbx.Size = new Size(x, y);
            cbx.AutoSize = true;

            cbx.Text = param[3];
            cbx.Checked = param[4] == "True";

            cbx.CheckedChanged += CheckListChanged;
            _controlList.Add(cbx);
        }

        private void WriteChecklistXML()
        {
            var xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.Indent = true;

            var ms = new MemoryStream();
            var wxml = XmlWriter.Create(ms, xmlWriterSettings);

            wxml.WriteStartDocument();
            wxml.WriteStartElement("Checklist");
            wxml.WriteAttributeString("Name", Gui.Name);
            wxml.WriteAttributeString("Description", Gui.Description);
            wxml.WriteAttributeString("Author", Gui.Author);
            wxml.WriteAttributeString("Created", Gui.Created);
            wxml.WriteAttributeString("Modified", Gui.Modified);
            wxml.WriteAttributeString("Version", Gui.Version);

            foreach (Control v in _controlList)
            {
                wxml.WriteStartElement("Control");
                wxml.WriteAttributeString("Name", v.Name);

                if (v.Name.Contains("Label"))
                    wxml.WriteElementString("Type", "Label");
                if (v.Name.Contains("ComboBox"))
                    wxml.WriteElementString("Type", "ComboBox");
                if (v.Name.Contains("ListBox"))
                    wxml.WriteElementString("Type", "ListBox");
                if (v.Name.Contains("PictureBox"))
                    wxml.WriteElementString("Type", "PictureBox");
                if (v.Name.Contains("CheckBox"))
                    wxml.WriteElementString("Type", "CheckBox");
                if (v.Name.Contains("RadioButton"))
                    wxml.WriteElementString("Type", "RadioButton");
                if (v.Name.Contains("TextBox"))
                    wxml.WriteElementString("Type", "TextBox");

                wxml.WriteElementString("Location", v.Location.X.ToString() + "," + v.Location.Y.ToString());
                wxml.WriteElementString("Size", v.Size.Width.ToString() + "," + v.Size.Height.ToString());
                wxml.WriteElementString("Text", v.Text);

                if (v.Name.Contains("Label"))
                {
                    wxml.WriteElementString("FontSize", v.Font.Size.ToString());
                }

                if (v.Name.Contains("RadioButton"))
                {
                    var clone = new RadioButton();
                    clone = (RadioButton)v;
                    wxml.WriteElementString("CheckState", clone.Checked.ToString());
                }            

                if (v.Name.Contains("ComboBox"))
                {
                    var clone = new ComboBox();
                    clone = (ComboBox)v;
                    wxml.WriteElementString("Count", clone.Items.Count.ToString());
                    foreach (var item in clone.Items)
                    {
                        wxml.WriteElementString("Item", item.ToString());
                    }
                }

                if (v.Name.Contains("ListBox"))
                {
                    var clone = new ListBox();
                    clone = (ListBox)v;
                    wxml.WriteElementString("Count", clone.Items.Count.ToString());
                    foreach (var item in clone.Items)
                    {
                        wxml.WriteElementString("Item", item.ToString());
                    }
                }

                if (v.Name.Contains("PictureBox"))
                {
                    if (!string.IsNullOrEmpty(v.AccessibleName))
                        wxml.WriteElementString("Path", v.AccessibleName);
                    else
                        wxml.WriteElementString("Path", "default");
                }

                if (v.Name.Contains("CheckBox"))
                {
                    var clone = new CheckBox();
                    clone = (CheckBox)v;
                    wxml.WriteElementString("CheckState", clone.Checked.ToString());
                }
                wxml.WriteEndElement();
            }

            wxml.WriteEndElement();
            wxml.WriteEndDocument();
            wxml.Flush();

            var buffer = new Byte[ms.Length];
            buffer = ms.ToArray();
            var xmlOutput = Encoding.UTF8.GetString(buffer);

            CaseFile.ChecklistXML = xmlOutput;
            CaseFile.WriteChecklist();
            CaseFile.WriteAuditData("Success", "CheckList: '" + Gui.Name + "' written to the CaseFile.");
        }

        private void CheckListChanged(object sender, EventArgs e)
        {
            CheckListDirty = true;
            CheckDirty();
        }

        private void PrintToolStripMenuItemClick(object sender, EventArgs e)
        {
            tsbPrintCase.PerformClick();
        }  

        private void TsbPrintCaseClick(object sender, EventArgs e)
        {
            var PrintIt = new Printing(tabControl);
            var dr = PrintIt.ShowDialog();

            if(dr != DialogResult.OK)
                return;

            var printTab = new TabPage();
            var printRTF = new RichTextBox();
            var printSize = Screen.PrimaryScreen.Bounds;
            CaseFile.WriteAuditData("Success", "User initiated CaseFile 'print' initiated.");

            foreach (var entry in PrintIt.Tabs)
            {
                if (entry != "Checklist")
                {
                    printTab = tabControl.TabPages[tabControl.TabPages.IndexOfKey(entry)];
                    printRTF = (RichTextBox) printTab.Controls[0];

                    var doc = new RichTextBoxDocument(printRTF)
                                  {
                                      Header = "",
                                      Footer = "Printed: " + DateTime.Today.ToShortDateString() + " at " +
                                               DateTime.Now.ToShortTimeString() + "\t\tPage [page] of [pages]"
                                  };

                    using (var printPreview = new CoolPrintPreview.CoolPrintPreviewDialog())
                    {
                        printPreview.Document = doc;
                        printPreview.Size = new Size(1024, printSize.Height - 75);
                        printPreview.Document.DefaultPageSettings.Margins.Top = 30;
                        printPreview.Document.DefaultPageSettings.Margins.Bottom = 40;
                        printPreview.Document.DefaultPageSettings.Margins.Left = 30;
                        printPreview.Document.DefaultPageSettings.Margins.Right = 30;
                        printPreview.ShowDialog(this);
                    }
                }
                else
                {
                    var doc = new RichTextBoxDocument(printRTF)
                    {
                        Header = "",
                        Footer = "Printed: " + DateTime.Today.ToShortDateString() + " at " +
                                 DateTime.Now.ToShortTimeString() + "\t\tPage [page] of [pages]"
                    };

                    doc.PrintPage += DocPrintPage;

                    using (var printPreview = new CoolPrintPreview.CoolPrintPreviewDialog())
                    {
                        printPreview.Document = doc;
                        printPreview.Size = new Size(1024, printSize.Height - 75);
                        printPreview.Document.DefaultPageSettings.Margins.Top = 30;
                        printPreview.Document.DefaultPageSettings.Margins.Bottom = 40;
                        printPreview.Document.DefaultPageSettings.Margins.Left = 30;
                        printPreview.Document.DefaultPageSettings.Margins.Right = 30;
                        printPreview.ShowDialog(this);
                    }
                }
            }
        }

        private void DocPrintPage(object sender, PrintPageEventArgs e)
        {
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            var bmp = new Bitmap(clPanel.Width, clPanel.Height);
            clPanel.DrawToBitmap(bmp, new Rectangle(0, 0, clPanel.Width, clPanel.Height));
            e.Graphics.DrawImage((Image)bmp, x, y);
        }

        private void TsbCutClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            if (userRTF.SelectedText.Length > 0)
            {
                userRTF.Cut();
            }
        }

        private void TsbCopyClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            if (userRTF.SelectedText.Length > 0)
            {
                userRTF.Copy();
            }
        }

        private void TsbPasteClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

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
                userRTF.Paste();
            }
        }

        private void BoldToolStripMenuItemClick(object sender, EventArgs e)
        {
            tsbBold.PerformClick();
        }

        private void FormatTextButton(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            if (userRTF.SelectedText != null && userRTF.SelectedText != "")
            {
                FontStyle req;
                if (sender == tsbBold) { req = FontStyle.Bold; }
                else if (sender == tsbItalic) { req = FontStyle.Italic; }
                //else if (sender == btnStrikeout) { req = FontStyle.Strikeout; }
                else { req = FontStyle.Underline; }
                Font f = userRTF.SelectionFont;
                FontStyle s;
                if (f != null)
                {
                    s = userRTF.SelectionFont.Style;
                    if ((f.Style & req) != 0) { s = s & (~req); }
                    else { s = s | req; }
                    userRTF.SelectionFont = new Font(f.FontFamily, f.Size, s);
                }
                else
                {
                    int start = userRTF.SelectionStart; int l = userRTF.SelectionLength;
                    for (int i = start; i < start + l; i++)
                    {
                        userRTF.SelectionStart = i; userRTF.SelectionLength = 1;
                        f = userRTF.SelectionFont;
                        s = userRTF.SelectionFont.Style;
                        if ((f.Style & req) != 0) { s = s & (~req); }
                        else { s = s | req; }
                        userRTF.SelectionFont = new Font(f.FontFamily, f.Size, s);
                    }
                    userRTF.SelectionStart = start; userRTF.SelectionLength = l;
                }
            }
            else
            {
                FontStyle req;
                if (sender == tsbBold)
                    req = FontStyle.Bold;
                else if (sender == tsbItalic)
                    req = FontStyle.Italic;
                else
                    req = FontStyle.Underline;

                FontStyle s;
                s = userRTF.SelectionFont.Style;

                if ((s & req) != 0)
                    s = s & (~req);
                else
                    s = s | req;
                userRTF.SelectionFont = new Font(userRTF.SelectionFont.FontFamily, userRTF.SelectionFont.Size, s);
            }
        }

        private void ItalicToolStripMenuItemClick(object sender, EventArgs e)
        {
            tsbItalic.PerformClick();
        }

        private void UnderlineToolStripMenuItemClick(object sender, EventArgs e)
        {
            tsbUnderline.PerformClick();
        }

        private void TsbAlignLeftClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            if (userRTF.SelectionFont == null)
                userRTF.SelectionLength = userRTF.SelectionLength;

            if (tsbAlignLeft.Checked)
            {
                tsbAlignCenter.Checked = false;
                tsbAlignRight.Checked = false;
                userRTF.SelectionAlignment = HorizontalAlignment.Left;
            }
        }

        private void TsbAlignCenterClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            if (userRTF.SelectionFont == null)
                userRTF.SelectionLength = userRTF.SelectionLength;

            if (tsbAlignCenter.Checked)
            {
                tsbAlignLeft.Checked = false;
                tsbAlignRight.Checked = false;
                userRTF.SelectionAlignment = HorizontalAlignment.Center;
            }
        }

        private void TsbAlignRightClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            if (userRTF.SelectionFont == null)
                userRTF.SelectionLength = userRTF.SelectionLength;

            if (tsbAlignRight.Checked)
            {
                tsbAlignCenter.Checked = false;
                tsbAlignLeft.Checked = false;
                userRTF.SelectionAlignment = HorizontalAlignment.Right;
            }
        }

        private void TsbBulletsClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            if (userRTF.SelectionFont == null)
                userRTF.SelectionLength = userRTF.SelectionLength;

            if (tsbBullets.Checked)
            {
                userRTF.SelectionBullet = true;
                userRTF.SelectionIndent += 12;
            }
            else
            {
                userRTF.SelectionBullet = false;
                userRTF.SelectionIndent -= 12;
            }
        }

        private void TsbOutdentClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            if (userRTF.SelectionFont == null)
                userRTF.SelectionLength = userRTF.SelectionLength;

            userRTF.SelectionIndent -= 50;
        }

        private void TsbIndentClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            if (userRTF.SelectionFont == null)
                userRTF.SelectionLength = userRTF.SelectionLength;

            userRTF.SelectionIndent += 50;
        }

        private void TsbSizeButtonSelectedIndexChanged(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            if (userRTF.SelectionFont == null)
                userRTF.SelectionLength = userRTF.SelectionLength;

            var currentFont = userRTF.SelectionFont;
            var newSize = Convert.ToInt16(tsbSizeButton.SelectedItem.ToString());

            if (currentFont != null) userRTF.SelectionFont = new Font(currentFont.Name, newSize, currentFont.Style);
        }

        private void TsbFontButtonSelectedIndexChanged(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            if (userRTF.SelectionFont == null)
                userRTF.SelectionLength = userRTF.SelectionLength;

            var currentFont = userRTF.SelectionFont;
            string newFont = tsbFontButton.SelectedItem.ToString();

            try
            {
                if (currentFont != null) userRTF.SelectionFont = new Font(newFont, currentFont.Size, currentFont.Style);
            }
            catch
            {
            }
        }

        private void TsbFontColourClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];
            var count = 0;

            try
            {
                var newColor = new ColorDialog();
                newColor.CustomColors = Functions.GetCustomColors(CaseNotesDB); 
                
                if (userRTF.SelectionFont == null)
                    userRTF.SelectionLength = userRTF.SelectionLength;
                else
                    newColor.Color = userRTF.SelectionColor;

                if (newColor.ShowDialog() == DialogResult.OK)
                {
                    userRTF.SelectionColor = newColor.Color;
                    var colors = (int[])newColor.CustomColors.Clone();
                    Prefs.CustomColors = Functions.SetCustomColors(colors, CaseNotesDB);
                }
            }
            catch (Exception fail)
            {
            }

        }

        private void TsbHighlightClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];
            var count = 0;

            if (userRTF.SelectionFont == null)
                userRTF.SelectionLength = userRTF.SelectionLength;
            else if (userRTF.SelectionBackColor != Color.Yellow)
                userRTF.SelectionBackColor = Color.Yellow;
            else
                userRTF.SelectionBackColor = userRTF.BackColor;
        }

        private void AboutCaseNotesToolStripMenuItemClick(object sender, EventArgs e)
        {
            tsbHelp.PerformClick();
        }

        private void TsbHelpClick(object sender, EventArgs e)
        {         
            var About = new AboutBox();
            About.ShowDialog();
        }

        private void NewToolStripMenuItemClick(object sender, EventArgs e)
        {
            tsbNewCase.PerformClick();
        }

        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            tsbOpenCase.PerformClick();
        }

        private void FindToolStripMenuItemClick(object sender, EventArgs e)
        {
            tsbFind.PerformClick();
        }

        private void TsbFindClick(object sender, EventArgs e)
        {
            try
            {
                var currentTab = tabControl.SelectedTab;
                if (currentTab.Text != "Case Checklist")
                {
                    var searchRTF = (RichTextBox) currentTab.Controls[0];

                    searchRTF.SelectionStart = 0;
                    searchRTF.SelectionLength = 0;
                    searchRTF.ReadOnly = true;

                    Form findIt = new FindDialog(searchRTF);
                    var dr = findIt.ShowDialog();

                    if (dr == DialogResult.OK || dr == DialogResult.Cancel)
                        searchRTF.ReadOnly = false;
                }
                else
                    MessageBox.Show("Please select a tab other than the checklist - this cannot be searched.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception fail)
            {
                var a = fail.Message;
            }
        }

        private void TsbShowRulerClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRuler = (TextRuler)userTab.Controls[1];

            userRuler.Visible = !userRuler.Visible;
        }

        private void TsbCheckSpellingClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];
            var scSettings = new SpellCheckSettings { };

            if (userRTF.IsSpellCheckEnabled())
                userRTF.DisableSpellCheck();
            else 
                userRTF.EnableSpellCheck();
        }

        private void LicenseActivationToolStripMenuItemClick(object sender, EventArgs e)
        {
            var activating = new Activate();
            if (!activating.activated)
                activating.ShowDialog();
            else
                MessageBox.Show("Thank you! - Your copy of CaseNotes has already been activated.", "CaseNotes Activation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Professional = activating.activated;
        }

        private void UndoToolStripMenuItemClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            userRTF.Undo();
        }

        private void RedoToolStripMenuItemClick(object sender, EventArgs e)
        {
            var userTab = tabControl.TabPages[tabControl.SelectedIndex];
            var userRTF = (RichTextBox)userTab.Controls[0];

            userRTF.Redo();
        }

        private void CheckForUpdatesToolStripMenuItemClick(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("Checking the First Response server for the latest version information...");
                Refresh();
                var client = new WebClient();
                var stream = client.OpenRead("https://first-response.co.uk/wp-content/uploads/casenotes/cnversion.txt");
                var reader = new StreamReader(stream);
                var cnVer = reader.ReadToEnd();
               
                var CurrentVer = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                if (CurrentVer != cnVer)
                    MessageBox.Show("You are currently running CaseNotes version: " + CurrentVer + ".\r\n\r\nPlease Visit https://first-response.co.uk/CaseNotes to download the latest version: " + cnVer, "CaseNotes Update Check",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Congratulations, you're all up to date with version " + CurrentVer, "CaseNotes Update Check", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception fail)
            {
                if (fail.Message.Contains("The remote name could not be resolved") || fail.Message.Contains("Unable to connect to the remote server"))
                    MessageBox.Show("Error! Couldn't connect to the First Response Server to check the version number.\r\n\r\nPlease visit https://first-response.co.uk/CaseNotes to check for updates from an internet connected machine.", "CaseNotes Update Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else MessageBox.Show(fail.Message, "CaseNotes Update Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            UpdateStatus("");
        }
    }
}
