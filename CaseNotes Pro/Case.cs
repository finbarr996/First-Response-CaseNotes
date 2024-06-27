using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes
{
    public partial class Case : Form
    {
        public string CaseFileDB = "";
        public static int MetaDataItems = 0;
        public static int TabsItems = 0;
        public bool Result;
        public bool CaseFileDirty;
        public string CaseName = "";
        public string FilePass = "";
        public List<string> MetaDataArray { get; set; }
        public List<string> CaseNoteArray { get; set; }
        public List<string> AuditArray { get; set; }
        public List<string> TabMetaDataArray { get; set; }
        public List<string> TabDataArray { get; set; }
        public string ChecklistPath = "";
        public string ChecklistName = "";
        public string ChecklistXML = "";
        private string MainTitle = "";

        public Case()
        {
            InitializeComponent();
        }

        public Case(string caseFileDB, string action, string pass = "")
        {
            InitializeComponent();
            chkChecklistNeeded.Visible = MainForm.Professional;

            lblCreated.Text = "";
            CaseFileDB = caseFileDB;
            if(!string.IsNullOrEmpty(pass)) FilePass = pass;
            
            switch (action)
            {
                case "new":
                    Result = NewCase(action);
                    break;

                case "open":
                    Result = OpenCase(action);
                    break;

                case "mru":
                    Result = OpenCase(action);
                    break;

                case "modify":
                    Result = NewCase(action);
                    break;
            }
        }

        private bool NewCase(string action)
        {
            var ok = false;
            var tabs = 0;
            switch (action)
            {
                case "new":
                    Text = "Create New Case";
                    btnSaveCase.Text = "Create";
                    ok = true;
                    break;

                case "modify":
                    Text = "Modify your Case";
                    btnSaveCase.Text = "Save";
                    ok = ReadMetaData(false);
                    if (ok)
                        tabs = ReadTabMetaData(false);
                    break;
            }

            if (!ok || tabs == -1)
            {
                Result = false;
                return Result;
            }

            var prefs = new Preferences();

            Task sys = Task.Factory.StartNew(() =>
                                        {
                                            prefs.ReadSystemPrefs(CaseFileDB, FilePass, action);
                                        });

            Task user = Task.Factory.StartNew(() =>
                                        {
                                            prefs.ReadUserPrefs(CaseFileDB, FilePass);
                                        });

            Task.WaitAll(sys, user);
            MetaDataItems = prefs.MetaDataItems;
            if (action == "new")
                TabsItems = prefs.TabItems;

            StatusCombo.Items.AddRange(prefs.CaseStatus.Split('#'));
            StatusCombo.Text = action == "new" ? StatusCombo.Items[0].ToString() : MetaDataArray[3];
            MainTitle = prefs.MainTitle;

            if (!CaseFileDB.EndsWith("CaseNotes.db3"))
            {
                var checks = ReadChecklist();
                if (checks == 0)
                {
                    chkChecklistNeeded.Checked = false;
                    chkChecklistNeeded.Enabled = true;
                }
                else
                {
                    chkChecklistNeeded.Checked = true;
                    chkChecklistNeeded.Enabled = false;
                }
            }

            if (!string.IsNullOrEmpty(FilePass))
            {
                chkEncrypt.Checked = true;
                PassText.Enabled = false;
                ConfirmText.Enabled = false;
                PassLabel.Enabled = true;
                ConfirmLabel.Enabled = true;
                PassText.Text = FilePass;
                ConfirmText.Text = FilePass;
            }
            else
            {
                chkEncrypt.Checked = false;
                PassText.Enabled = false;
                ConfirmText.Enabled = false;
                PassLabel.Enabled = false;
                ConfirmLabel.Enabled = false; 
                PassText.Text = FilePass;
                ConfirmText.Text = FilePass;
            }

            if (action == "new")
            {
                var tabMetaDataArray = new List<string>();

                for (var i = 1; i <= TabsItems; i++)
                {
                    tabMetaDataArray.Add(i.ToString());
                    tabMetaDataArray.Add(prefs.TabsValueArray[i - 1]);
                }

                TabMetaDataArray = tabMetaDataArray;
            }

            chkEncrypt.CheckedChanged += ChkEncryptCheckedChanged;
            NumTabs.SelectedIndexChanged += NumTabsChanged;
            groupBox1.Height = textBox5.Top + textBox1.Height + 23;

            if (MetaDataItems > 0)
            {
                label1.Text = prefs.MetaDescArray[0];
                textBox1.Text = action == "new" ? prefs.MetaValueArray[0] : MetaDataArray[5];
                label1.Visible = true;
                textBox1.Visible = true;
                //groupBox1.Height = textBox1.Top + textBox1.Height + 23;
            }

            if (MetaDataItems > 1)
            {
                label2.Text = prefs.MetaDescArray[1];
                textBox2.Text = action == "new" ? prefs.MetaValueArray[1] : MetaDataArray[7];
                label2.Visible = true;
                textBox2.Visible = true;
                //groupBox1.Height = textBox2.Top + textBox1.Height + 23;
            }

            if (MetaDataItems > 2)
            {
                label3.Text = prefs.MetaDescArray[2];
                textBox3.Text = action == "new" ? prefs.MetaValueArray[2] : MetaDataArray[9];
                label3.Visible = true;
                textBox3.Visible = true;
                //groupBox1.Height = textBox3.Top + textBox1.Height + 23;
            }

            if (MetaDataItems > 3)
            {
                label4.Text = prefs.MetaDescArray[3];
                textBox4.Text = action == "new" ? prefs.MetaValueArray[3] : MetaDataArray[11];
                label4.Visible = true;
                textBox4.Visible = true;
                //groupBox1.Height = textBox4.Top + textBox1.Height + 23;
            }

            if (MetaDataItems > 4)
            {
                label5.Text = prefs.MetaDescArray[4];
                textBox5.Text = action == "new" ? prefs.MetaValueArray[4] : MetaDataArray[13];
                label5.Visible = true;
                textBox5.Visible = true;
                //groupBox1.Height = textBox5.Top + textBox1.Height + 23;
            }

            if (MetaDataItems > 5)
            {
                label6.Text = prefs.MetaDescArray[5];
                textBox6.Text = action == "new" ? prefs.MetaValueArray[5] : MetaDataArray[15];
                label6.Visible = true;
                textBox6.Visible = true;
                groupBox1.Height = textBox6.Top + textBox1.Height + 23;
            }

            if (MetaDataItems > 6)
            {
                label7.Text = prefs.MetaDescArray[6];
                textBox7.Text = action == "new" ? prefs.MetaValueArray[6] : MetaDataArray[17];
                label7.Visible = true;
                textBox7.Visible = true;
                groupBox1.Height = textBox7.Top + textBox1.Height + 23;
            }

            if (MetaDataItems > 7)
            {
                label8.Text = prefs.MetaDescArray[7];
                textBox8.Text = action == "new" ? prefs.MetaValueArray[7] : MetaDataArray[19];
                label8.Visible = true;
                textBox8.Visible = true;
                groupBox1.Height = textBox8.Top + textBox1.Height + 23;
            }

            if (MetaDataItems > 8)
            {
                label9.Text = prefs.MetaDescArray[8];
                textBox9.Text = action == "new" ? prefs.MetaValueArray[8] : MetaDataArray[21];
                label9.Visible = true;
                textBox9.Visible = true;
                groupBox1.Height = textBox9.Top + textBox1.Height + 23;
            }

            if (MetaDataItems > 9)
            {
                label10.Text = prefs.MetaDescArray[9];
                textBox10.Text = action == "new" ? prefs.MetaValueArray[9] : MetaDataArray[23];
                label10.Visible = true;
                textBox10.Visible = true;
                groupBox1.Height = textBox10.Top + textBox1.Height + 23;
            }

            CaseTabControl.Height = groupBox1.Height + 40;
            btnSaveCase.Top = CaseTabControl.Height + 13;
            btnCancel.Top = CaseTabControl.Height + 13;
            Height = CaseTabControl.Height + 70;

            NumTabs.SelectedIndex = TabsItems;
            DisplayTabs();
            
            return Result;
        }

        private bool OpenCase(string action)
        {
            var prefs = new Preferences();

            if (!File.Exists(CaseFileDB))
            {
                MessageBox.Show("The '" + CaseFileDB + "' cannot be found.", "CaseFile missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (action == "open")
            {
                prefs.ReadSystemPrefs(CaseFileDB, FilePass, "open");

                var openFileDialog = new OpenFileDialog();
                openFileDialog.CheckFileExists = true;
                openFileDialog.DefaultExt = ".db3";
                openFileDialog.Filter = "CaseNotes database files | *.db3";
                openFileDialog.InitialDirectory = prefs.DefaultLocation;
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "Select a CaseNotes file to open";
                var result = openFileDialog.ShowDialog();
                if (result == DialogResult.Cancel)
                    return false;

                CaseFileDB = openFileDialog.FileName;
            }
            
            var RW = true;
            try
            {
                RW = prefs.ReadSystemPrefs(CaseFileDB, FilePass, "open");
            }

            catch (Exception fail)
            {
                if (fail.Message.Contains("Attempt to write a read-only database"))
                {
                    MessageBox.Show("The file you have selected cannot be opened for read-write access.\r\nPlease note that CaseNotes is NOT a multi-user application!", "Case File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (fail.Message.Contains("File opened that is not a database file"))
                {
                    var pass = new Password();
                    var badPaswword = true;
                    var counter = 0;

                    do
                    {
                        counter++;
                        pass.txtPassword.Text = "";
                        var dr = pass.ShowDialog();
                        FilePass = dr == DialogResult.OK ? pass.DBPassword : null;

                        if (FilePass == null)
                            return false;

                        try
                        {
                            prefs.ReadSystemPrefs(CaseFileDB, FilePass, "open");
                            badPaswword = false;
                        }
                        catch (Exception moreFail)
                        {
                            if (moreFail.Message.Contains("File opened that is not a database file"))
                            {
                                MessageBox.Show("The password you have entered is not correct or this is not a valid CaseFile.", "Incorrect password entered.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            if (counter >= 3)
                            {
                                MessageBox.Show("You have entered an incorrect password three times.\r\nTo prevent brute force password attacks CaseNotes will now reset.", "Password attempts exceeded.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }
                        }
                    } while (badPaswword);
                }
            }

            if (!RW)
            {
                MessageBox.Show("The file you have selected cannot be opened for read-write access.\r\nPlease note that CaseNotes is NOT a multi-user application!", "Case File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            RW = prefs.ReadSystemPrefs(CaseFileDB, FilePass, "open");

            if (!RW)
            {
                MessageBox.Show("The file you have selected cannot be opened for read-write access.\r\nPlease note that CaseNotes is NOT a multi-user application!", "Case File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            Cursor = Cursors.WaitCursor;

            try
            {

                prefs.ReadSystemPrefs(CaseFileDB, FilePass, "open");
                //    WriteAuditData("Success", "Case preferences read from the case file.");
                //else
                //    WriteAuditData("Failure", "Case preferences failed to read from the case file.");
            }
            catch (Exception fail)
            {
                if (fail.Message.Contains("no such table: Audit"))
                {
                    MessageBox.Show("The file you have selected is not a valid CaseNotes case file.", "Case File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (fail.Message.Contains("Attempt to write a read-only database"))
                {
                    MessageBox.Show("The file you have selected cannot be opened for read-write access.\r\nPlease note that CaseNotes is NOT a multi-user application!", "Case File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;                    
                }
            }

            MetaDataItems = prefs.MetaDataItems;
            TabsItems = prefs.TabItems;
            MainTitle = prefs.MainTitle;
            var count = 0;
            ReadMetaData(false);
            //    WriteAuditData("Success", "Case metadata read from the case file.");
            //else
            //    WriteAuditData("Failure", "Case metadata failed to read from the case file.");

            ReadTabMetaData(false);
            //    WriteAuditData("Success", "User tab metadata read from the case file.");
            //else
            //    WriteAuditData("Failure", "User tab metadata failed to read from the case file.");

            var checks = ReadChecklist();
            if (checks > 0)
            {
                //WriteAuditData("Success", "Checklist read from the case file.");
                chkChecklistNeeded.Checked = true;
                chkChecklistNeeded.Enabled = false;
            }
            else
            {
                if (checks < 0)
                {
                    //WriteAuditData("Failure", "Checklist failed to read correctly from the case file - Hash match failure.");
                    chkChecklistNeeded.Checked = true;
                    chkChecklistNeeded.Enabled = false;
                }
                if (checks == 0)
                {
                    //WriteAuditData("Success", "No Checklist designated for this case file.");
                    chkChecklistNeeded.Checked = false;
                    chkChecklistNeeded.Enabled = true;
                }
            }

            Cursor = Cursors.Default; 
            return true;
        }

        private void BtnSaveCaseClick(object sender, EventArgs e)
        {
            var DataOK = ValidateTabs();

            if (!DataOK)
                return;

            Cursor = Cursors.WaitCursor;
            if (btnSaveCase.Text == "Create")
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Warning! The '" + label1.Text + "' cannot be left blank!", "Case Reference Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.Text = "undefined";
                    textBox1.Focus();
                    Cursor = Cursors.Default;
                    return;
                }

                var sCaseRef = textBox1.Text.Replace("\\", "-");
                sCaseRef = sCaseRef.Replace("/", "-");

                var prefs = new Preferences();
                prefs.ReadSystemPrefs(CaseFileDB, FilePass, "open");
                prefs.ReadUserPrefs(CaseFileDB, FilePass);

                if (prefs.DefaultLocation.EndsWith("\\"))
                    CaseName = prefs.DefaultLocation + sCaseRef;
                else
                    CaseName = prefs.DefaultLocation + "\\" + sCaseRef;

                CaseName = CaseName.Replace("/", "-");

                TabsItems = NumTabs.SelectedIndex;
                prefs.TabItems = TabsItems;
                MetaDataItems = prefs.MetaDataItems;
                MainTitle = prefs.MainTitle;

                if (chkEncrypt.Checked & PassText.Text.Trim() != ConfirmText.Text.Trim())
                {
                    MessageBox.Show("Warning! The passwords you have entered do not match!", "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    PassText.Focus();
                    Cursor = Cursors.Default;
                    return;
                }
                if (chkEncrypt.Checked & PassText.Text.Length <= 0)
                {
                    MessageBox.Show("Warning! The password must be at least 1 character long!", "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    PassText.Focus();
                    Cursor = Cursors.Default;
                    return;
                }
                if (chkEncrypt.Checked)
                {
                    PassText.Text = PassText.Text.Trim();
                    FilePass = PassText.Text;
                }

                try
                {
                    var saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "CaseNotes db File | *.db3";
                    saveFileDialog.AddExtension = true;
                    saveFileDialog.DefaultExt = ".db3";
                    saveFileDialog.ValidateNames = true;
                    saveFileDialog.CheckFileExists = false;
                    saveFileDialog.OverwritePrompt = true;
                    saveFileDialog.Title = "Create a new Case File";
                    saveFileDialog.InitialDirectory = prefs.DefaultLocation;
                    saveFileDialog.FileName = CaseName;

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        Cursor = Cursors.Default;
                        FilePass = "";
                        Result = false;
                        return;
                    }

                    Cursor = Cursors.WaitCursor;
                    Refresh();

                    if (saveFileDialog.FileName != "")
                    {
                        CaseName = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 4);
                        if (File.Exists(saveFileDialog.FileName))
                            File.Delete(saveFileDialog.FileName);

                        CaseFileDB = saveFileDialog.FileName;
                        var caseDB = new SQLiteDatabase {DBConnection = "Data Source=" + CaseFileDB};

                        if (!string.IsNullOrEmpty(FilePass))
                            caseDB.SQLiteSetPassword(FilePass);

                        Task sys = Task.Factory.StartNew(() => caseDB.InitialiseCase(CaseFileDB, FilePass))
                            .ContinueWith(state =>
                                                {
                                                    if (prefs.WriteSystemPrefs(CaseFileDB, FilePass))
                                                        WriteAuditData("Success", "System preferences written to the new case file.");
                                                    else
                                                        WriteAuditData("Failure", "System preferences failed to write to the new case file.");
                                                    if (prefs.WriteUserPrefs(CaseFileDB, FilePass))
                                                        WriteAuditData("Success", "User preferences written to the new case file.");
                                                    else
                                                        WriteAuditData("Failure", "User preferences failed to write to the new case file.");
                                                });

                        Task.WaitAll(sys);
                        CaseFileDB = saveFileDialog.FileName;

                    }
                    else
                    {
                        MessageBox.Show("The file name you have selected is not valid.", "Create a new Case File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Cursor = Cursors.Default;
                        return;
                    }
                }

                catch (InvalidOperationException)
                {
                    MessageBox.Show("The file name you have selected is not valid.", "Create a new Case File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Cursor = Cursors.Default;
                    return;
                }
            }

            Cursor = Cursors.WaitCursor;
            Refresh();

            if (chkEncrypt.Checked & PassText.Text.Length <= 0)
            {
                MessageBox.Show("Warning! The password must be at least 1 character long!", "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PassText.Focus();
                Cursor = Cursors.Default;
                return;
            }

            if (!string.IsNullOrEmpty(PassText.Text.Trim()) && !string.IsNullOrEmpty(ConfirmText.Text.Trim()) && string.IsNullOrEmpty(FilePass))
            {
                if (PassText.Text.Trim() == ConfirmText.Text.Trim())
                {
                    FilePass = PassText.Text.Trim();

                    var strip = new SQLiteConnection();
                    strip.ConnectionString = "Data Source=" + CaseFileDB;
                    strip.Open();
                    strip.ChangePassword(FilePass);
                    strip.Close();

                    MessageBox.Show("The database has been encrypted with the password supplied.", "Database encrypted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Warning! The passwords you have entered do not match!", "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Cursor = Cursors.Default;
                    PassText.Focus();
                    return;
                }
            }

            if (WriteMetaData())
                WriteAuditData("Success", "Case metadata written to the case file.");
            else
                WriteAuditData("Failure", "Case metadata failed to write to the case file.");

            ReadMetaData(false);

            if (WriteTabMetaData())
                WriteAuditData("Success", "User tab metadata written to the case file.");
            else
                WriteAuditData("Failure", "User tab metadata failed to write to the case file.");

            ReadTabMetaData(false);
            TabsItems = NumTabs.SelectedIndex;

            var sysPrefs = new Preferences();
            sysPrefs.ReadSystemPrefs(CaseFileDB, FilePass, "open");
            sysPrefs.TabItems = TabsItems;
            sysPrefs.WriteSystemPrefs(CaseFileDB, FilePass);

            if (chkChecklistNeeded.Checked && chkChecklistNeeded.Enabled)
            {
                sysPrefs.ReadSystemPrefs(CaseFileDB, FilePass, "open");

                var scl = new SelectCheckList(sysPrefs.ChecklistLocation);
                DialogResult dr = scl.ShowDialog();

                if (dr == DialogResult.Cancel)
                    chkChecklistNeeded.Checked = false;
                else
                {
                    ChecklistPath = scl.SelectedChecklistPath;
                    ChecklistName = scl.SelectedChecklistName;
                    if (!string.IsNullOrEmpty(ChecklistPath))
                        ChecklistXML = File.ReadAllText(ChecklistPath);
                }
            }

            if (chkChecklistNeeded.Checked)
            {
                if (WriteChecklist())
                    WriteAuditData("Success", "Case checklist written to the case file.");
                else
                    WriteAuditData("Failure", "Case checklist failed to write to the case file.");
            }
            ReadChecklist();

            Cursor = Cursors.Default;
            Result = true;

            if (DataOK)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        
        private void BtnCancelClick(object sender, EventArgs e)
        {
            ResetCaseData();
            Result = true;
            Cursor = Cursors.Default;
            Close();
        }

        private void ResetCaseData()
        {
            CaseFileDB = "";
            MetaDataItems = 0;
            TabsItems = 0;
            CaseName = "";

            if (MetaDataArray != null) MetaDataArray.Clear();
            if (CaseNoteArray != null) CaseNoteArray.Clear();
            if (AuditArray != null) AuditArray.Clear();
            if (TabMetaDataArray != null) TabMetaDataArray.Clear();
            if (TabDataArray != null) TabDataArray.Clear();
            ChecklistPath = "";
            ChecklistName = "";
            ChecklistXML = "";        
        }

        private void ChkEncryptCheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FilePass))
            {
                var pass = new Password();
                pass.Text = "Confirm existing password";
                pass.label2.Text = "To remove or change the password, please enter the exisitng one.";
                var dr = pass.ShowDialog();
                if (dr != DialogResult.OK || pass.DBPassword != FilePass)
                {
                    chkEncrypt.CheckedChanged -= ChkEncryptCheckedChanged;
                    chkEncrypt.Checked = true;
                    chkEncrypt.CheckedChanged += ChkEncryptCheckedChanged;
                    MessageBox.Show("The password you supplied was incorrect.", "Authentication error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                else
                {
                    var strip = new SQLiteConnection();
                    if (string.IsNullOrEmpty(FilePass))
                        strip.ConnectionString = "Data Source=" + CaseFileDB;
                    else
                        strip.ConnectionString = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

                    strip.Open();
                    strip.ChangePassword("");
                    FilePass = "";
                    strip.Close();
                    MessageBox.Show("The database password and encryption have been removed.", "Password removed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            
            PassLabel.Enabled = chkEncrypt.Checked;
            PassText.Enabled = chkEncrypt.Checked;
            ConfirmLabel.Enabled = chkEncrypt.Checked;
            ConfirmText.Enabled = chkEncrypt.Checked;

            if (!chkEncrypt.Checked)
            {
                PassText.Text = null;
                ConfirmText.Text = null;
                FilePass = null;
            }
        }

        private void NumTabsChanged(object sender, EventArgs e)
        {

            textBox20.Enabled = false;
            textBox21.Enabled = false;
            textBox22.Enabled = false;
            textBox23.Enabled = false;
            textBox24.Enabled = false;
            textBox25.Enabled = false;
            textBox26.Enabled = false;
            textBox27.Enabled = false;
            textBox28.Enabled = false;
            textBox29.Enabled = false;

            if (NumTabs.SelectedIndex < TabsItems && Text.Contains("Modify your Case"))
                NumTabs.SelectedIndex = TabsItems;
            else
                if (NumTabs.SelectedIndex > 0) TabsItems = NumTabs.SelectedIndex;

            if (NumTabs.SelectedIndex > 0)
                textBox20.Enabled = true;
            if (NumTabs.SelectedIndex > 1)
                textBox21.Enabled = true;
            if (NumTabs.SelectedIndex > 2)
                textBox22.Enabled = true;
            if (NumTabs.SelectedIndex > 3)
                textBox23.Enabled = true;
            if (NumTabs.SelectedIndex > 4)
                textBox24.Enabled = true;
            if (NumTabs.SelectedIndex > 5)
                textBox25.Enabled = true;
            if (NumTabs.SelectedIndex > 6)
                textBox26.Enabled = true;
            if (NumTabs.SelectedIndex > 7)
                textBox27.Enabled = true;
            if (NumTabs.SelectedIndex > 8)
                textBox28.Enabled = true;
            if (NumTabs.SelectedIndex > 9)
                textBox29.Enabled = true;
        }

        public bool WriteChecklist()
        {
            var result = 0;
            var temp = "";
            try
            {
                temp = File.ReadAllText(ChecklistPath);
            }
            catch (System.IO.FileNotFoundException)
            {}
            
            byte[] checklistXML;
            string hash;

            if (string.IsNullOrEmpty(ChecklistXML))
            {
                if (Convert.ToChar(temp.Substring(0, 1)) > 128)
                    temp = temp.Substring(1);

                checklistXML = Functions.String2Byte(temp);
                hash = Functions.GetMd5(temp);
            }
            else
            {
                if (Convert.ToChar(ChecklistXML.Substring(0, 1)) > 128)
                    ChecklistXML = ChecklistXML.Substring(1);

                checklistXML = Functions.String2Byte(ChecklistXML);
                hash = Functions.GetMd5(ChecklistXML);
            }

            checklistXML = Functions.Compress(checklistXML);

            string dbConnection;
            if (string.IsNullOrEmpty(FilePass))
                dbConnection = "Data Source=" + CaseFileDB;
            else
                dbConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            using (var connection = new SQLiteConnection(dbConnection))
            using (var command = new SQLiteCommand(connection))
            {
                connection.Open();
                command.CommandText = "INSERT INTO Checklist (ChecklistName, ChecklistPath, ChecklistXML, Hash) VALUES (@ChecklistName, @ChecklistPath, @ChecklistXML, @Hash)";
                command.Parameters.Add("@ChecklistName", DbType.String).Value = ChecklistName;
                command.Parameters.Add("@ChecklistPath", DbType.String).Value = ChecklistPath;
                command.Parameters.Add("@ChecklistXML", DbType.Binary).Value = checklistXML;
                command.Parameters.Add("@Hash", DbType.String).Value = hash;

                result = command.ExecuteNonQuery();
                connection.Close();
            }

            return (result > 0);
        
        }

        public int ReadChecklist()
        {
            var check = new SQLiteDatabase();
            if(string.IsNullOrEmpty(FilePass))
                check.DBConnection = "Data Source=" + CaseFileDB;
            else
                check.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            var dt = check.GetDataTable("select * from Checklist;");
            string storedHash = "", calculatedHash = "";
            int result = 0;

            foreach (DataRow dr in dt.Rows)
            {
                ChecklistName = dr[1].ToString();
                ChecklistPath = dr[2].ToString();
                ChecklistXML = Functions.Byte2String(Functions.Decompress((byte[]) dr[3]));
                if (Convert.ToChar(ChecklistXML.Substring(0,1)) > 128)
                    ChecklistXML = ChecklistXML.Substring(1);
                storedHash = dr[4].ToString();
            }

            result = dt.Rows.Count;

            if (result > 0)
            {
                calculatedHash = Functions.GetMd5(ChecklistXML);
                if (calculatedHash != storedHash)
                    result = -1;
            }

            return result;            
        }

        public bool WriteMetaData()
        {
            var meta = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                meta.DBConnection = "Data Source=" + CaseFileDB;
            else
                meta.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            meta.ClearTable("MetaData");

            var data = new Dictionary<string, string>();

            data.Add("MetaKey", "Title"); 
            data.Add("MetaValue", MainTitle);
            data.Add("Hash", Functions.GetMd5(MainTitle));
            var hashes = Functions.GetMd5(MainTitle);
            meta.Insert("MetaData", data);
            data.Clear();

            data.Add("MetaKey", "Status");
            data.Add("MetaValue", StatusCombo.SelectedItem.ToString());
            data.Add("Hash", Functions.GetMd5(StatusCombo.SelectedItem.ToString()));
            hashes += Functions.GetMd5(StatusCombo.SelectedItem.ToString());
            meta.Insert("MetaData", data); 
            data.Clear();

            if (MetaDataItems > 0)
            {
                data.Add("MetaKey", label1.Text);
                data.Add("MetaValue", textBox1.Text.Trim());
                data.Add("Hash", Functions.GetMd5(textBox1.Text.Trim()));
                hashes += Functions.GetMd5(textBox1.Text.Trim());
                meta.Insert("MetaData", data);
                data.Clear();
            }

            if (MetaDataItems > 1)
            {
                data.Add("MetaKey", label2.Text);
                data.Add("MetaValue", textBox2.Text.Trim());
                data.Add("Hash", Functions.GetMd5(textBox2.Text.Trim()));
                hashes += Functions.GetMd5(textBox2.Text.Trim()); 
                meta.Insert("MetaData", data);
                data.Clear();
            }

            if (MetaDataItems > 2)
            {
                data.Add("MetaKey", label3.Text );
                data.Add("MetaValue", textBox3.Text.Trim());
                data.Add("Hash", Functions.GetMd5(textBox3.Text.Trim()));
                hashes += Functions.GetMd5(textBox3.Text.Trim()); 
                meta.Insert("MetaData", data);
                data.Clear();
            }

            if (MetaDataItems > 3)
            {
                data.Add("MetaKey", label4.Text );
                data.Add("MetaValue", textBox4.Text.Trim());
                data.Add("Hash", Functions.GetMd5(textBox4.Text.Trim()));
                hashes += Functions.GetMd5(textBox4.Text.Trim()); 
                meta.Insert("MetaData", data);
                data.Clear();
            }

            if (MetaDataItems > 4)
            {
                data.Add("MetaKey", label5.Text );
                data.Add("MetaValue", textBox5.Text.Trim());
                data.Add("Hash", Functions.GetMd5(textBox5.Text.Trim()));
                hashes += Functions.GetMd5(textBox5.Text.Trim()); 
                meta.Insert("MetaData", data);
                data.Clear();
            }

            if (MetaDataItems > 5)
            {
                data.Add("MetaKey", label6.Text );
                data.Add("MetaValue", textBox6.Text.Trim());
                data.Add("Hash", Functions.GetMd5(textBox6.Text.Trim()));
                hashes += Functions.GetMd5(textBox6.Text.Trim()); 
                meta.Insert("MetaData", data);
                data.Clear();
            }

            if (MetaDataItems > 6)
            {
                data.Add("MetaKey", label7.Text );
                data.Add("MetaValue", textBox7.Text.Trim());
                data.Add("Hash", Functions.GetMd5(textBox7.Text.Trim()));
                hashes += Functions.GetMd5(textBox7.Text.Trim()); 
                meta.Insert("MetaData", data);
                data.Clear();
            }

            if (MetaDataItems > 7)
            {
                data.Add("MetaKey", label8.Text );
                data.Add("MetaValue", textBox8.Text.Trim());
                data.Add("Hash", Functions.GetMd5(textBox8.Text.Trim()));
                hashes += Functions.GetMd5(textBox8.Text.Trim()); 
                meta.Insert("MetaData", data);
                data.Clear();
            }

            if (MetaDataItems > 8)
            {
                data.Add("MetaKey", label9.Text );
                data.Add("MetaValue", textBox9.Text.Trim());
                data.Add("Hash", Functions.GetMd5(textBox9.Text.Trim()));
                hashes += Functions.GetMd5(textBox9.Text.Trim()); 
                meta.Insert("MetaData", data);
                data.Clear();
            }

            if (MetaDataItems > 9)
            {
                data.Add("MetaKey", label10.Text );
                data.Add("MetaValue", textBox10.Text.Trim());
                data.Add("Hash", Functions.GetMd5(textBox10.Text.Trim()));
                hashes += Functions.GetMd5(textBox10.Text.Trim()); 
                meta.Insert("MetaData", data);
                data.Clear();
            }

            var timeStamp = DateTime.Now.ToString("yyyy-MMM-dd") + " - " + DateTime.Now.ToString("HH:mm:ss"); 
            data.Add("MetaKey", "Case Created:");
            if (string.IsNullOrEmpty(lblCreated.Text))
            {
                data.Add("MetaValue", timeStamp);
                data.Add("Hash", Functions.GetMd5(timeStamp));
                hashes += Functions.GetMd5(timeStamp);
            }
            else
            {
                data.Add("MetaValue", lblCreated.Text);
                data.Add("Hash", Functions.GetMd5(lblCreated.Text));
                hashes += Functions.GetMd5(lblCreated.Text);
            }
            meta.Insert("MetaData", data);
            data.Clear();

            data.Add("MetaKey", "Last Modified:");
            data.Add("MetaValue", timeStamp);
            data.Add("Hash", Functions.GetMd5(timeStamp));
            hashes += Functions.GetMd5(timeStamp);

            meta.Insert("MetaData", data);
            data.Clear();

            int result;
            var MetaHash = Functions.String2Byte(Functions.GetMd5(hashes));
            MetaHash = Functions.Compress(MetaHash);

            try
            {
                var commandValue = "";
                var dt = meta.GetDataTable("select * from Hash;");
                if (dt.Rows.Count == 0)
                    commandValue = "INSERT INTO Hash (MetaHash) VALUES (@MetaHash)";
                else
                    commandValue = "UPDATE Hash SET MetaHash = @MetaHash";

                string dbConnection;
                if (string.IsNullOrEmpty(FilePass))
                    dbConnection = "Data Source=" + CaseFileDB;
                else
                    dbConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

                using (var connection = new SQLiteConnection(dbConnection))
                using (var command = new SQLiteCommand(connection))
                {
                    connection.Open();

                    command.CommandText = commandValue;
                    command.Parameters.Add("@MetaHash", DbType.Binary).Value = MetaHash;

                    result = command.ExecuteNonQuery();
                    connection.Close();
                }

                return (result > 0);
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
                return false;
            }
        }

        public bool CaseUpdated()
        {
            var metaData = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                metaData.DBConnection = "Data Source=" + CaseFileDB;
            else
                metaData.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            var data = new Dictionary<string, string>();
            var timeStamp = DateTime.Now.ToString("yyyy-MMM-dd") + " - " + DateTime.Now.ToString("HH:mm:ss");

            data.Add("MetaKey", "Last Modified:");
            data.Add("MetaValue", timeStamp);
            data.Add("Hash", Functions.GetMd5(timeStamp));

            var result = metaData.Update("MetaData", data, "MetaKey = 'Last Modified:'");
            MetaDataArray[MetaDataArray.Count - 1] = timeStamp;

            return result;
        }

        public bool ReadMetaData(bool display)
        {
            var metaData = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                metaData.DBConnection = "Data Source=" + CaseFileDB;
            else
                metaData.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            try
            {
                var dt = metaData.GetDataTable("select * from MetaData;");
                var metaDataArray = new List<string>();
                var MetaOK = true;

                foreach (DataRow dr in dt.Rows)
                {
                    metaDataArray.Add(dr[1].ToString());
                    metaDataArray.Add(dr[2].ToString());
                    var hash = Functions.GetMd5(dr[2].ToString());
                    if (hash != dr[3].ToString())
                        MetaOK = false;
                }

                lblCreated.Text = metaDataArray[metaDataArray.Count -3];
                lblCreated.Visible = false;

                MetaDataArray = metaDataArray;
                CaseName = metaDataArray[5];
               
                #region Update this form's display

                if (display)
                {
                    MessageBox.Show("Displaying MetaData!");

                    if (!string.IsNullOrEmpty(FilePass))
                    {
                        chkEncrypt.Checked = true;
                        PassText.Enabled = true;
                        ConfirmText.Enabled = true;
                        PassText.Text = FilePass;
                        ConfirmText.Text = FilePass;
                    }
                    else
                    {
                        chkEncrypt.Checked = false;
                        PassText.Enabled = false;
                        ConfirmText.Enabled = false;
                        PassText.Text = FilePass;
                        ConfirmText.Text = FilePass;                    
                    }

                    if (MetaDataItems > 0)
                    {
                        label1.Text = metaDataArray[4];
                        textBox1.Text = metaDataArray[5];
                        label1.Visible = true;
                        textBox1.Visible = true;
                    }

                    if (MetaDataItems > 1)
                    {
                        label2.Text = metaDataArray[6];
                        textBox2.Text = metaDataArray[7];
                        label2.Visible = true;
                        textBox2.Visible = true;
                    }

                    if (MetaDataItems > 2)
                    {
                        label3.Text = metaDataArray[8];
                        textBox3.Text = metaDataArray[9];
                        label3.Visible = true;
                        textBox3.Visible = true;
                    }

                    if (MetaDataItems > 3)
                    {
                        label4.Text = metaDataArray[10];
                        textBox4.Text = metaDataArray[11];
                        label4.Visible = true;
                        textBox4.Visible = true;
                    }

                    if (MetaDataItems > 4)
                    {
                        label5.Text = metaDataArray[12];
                        textBox5.Text = metaDataArray[13];
                        label5.Visible = true;
                        textBox5.Visible = true;
                    }

                    if (MetaDataItems > 5)
                    {
                        label6.Text = metaDataArray[14];
                        textBox6.Text = metaDataArray[15];
                        label6.Visible = true;
                        textBox6.Visible = true;
                    }

                    if (MetaDataItems > 6)
                    {
                        label7.Text = metaDataArray[16];
                        textBox7.Text = metaDataArray[17];
                        label7.Visible = true;
                        textBox7.Visible = true;
                    }

                    if (MetaDataItems > 7)
                    {
                        label8.Text = metaDataArray[18];
                        textBox8.Text = metaDataArray[19];
                        label8.Visible = true;
                        textBox8.Visible = true;
                    }

                    if (MetaDataItems > 8)
                    {
                        label9.Text = metaDataArray[20];
                        textBox9.Text = metaDataArray[21];
                        label9.Visible = true;
                        textBox9.Visible = true;
                    }

                    if (MetaDataItems > 9)
                    {
                        label10.Text = metaDataArray[22];
                        textBox10.Text = metaDataArray[23];
                        label10.Visible = true;
                        textBox10.Visible = true;
                    }
                }
                #endregion

                return (MetaOK);
            }
            catch (Exception fail)
            {
                if (fail.Message.Contains("no such table"))
                {
                    MessageBox.Show("Your Case File can no longer be accessed (has it moved?)\r\nYou should close this case file.", "Case File Access Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                else
                {
                    MessageBox.Show(fail.Message);
                    return false;                    
                }
            }
        }

        public void PopulateTabArray()
        {
            TabMetaDataArray.Clear();

            if (TabsItems > 0)
            {
                TabMetaDataArray.Add("1");
                TabMetaDataArray.Add(textBox20.Text.Trim());
            }

            if (TabsItems > 1)
            {
                TabMetaDataArray.Add("2");
                TabMetaDataArray.Add(textBox21.Text.Trim());
            }

            if (TabsItems > 2)
            {
                TabMetaDataArray.Add("3");
                TabMetaDataArray.Add(textBox22.Text.Trim());
            }

            if (TabsItems > 3)
            {
                TabMetaDataArray.Add("4");
                TabMetaDataArray.Add(textBox23.Text.Trim());
            }

            if (TabsItems > 4)
            {
                TabMetaDataArray.Add("5");
                TabMetaDataArray.Add(textBox24.Text.Trim());
            }

            if (TabsItems > 5)
            {
                TabMetaDataArray.Add("6");
                TabMetaDataArray.Add(textBox25.Text.Trim());
            }

            if (TabsItems > 6)
            {
                TabMetaDataArray.Add("7");
                TabMetaDataArray.Add(textBox26.Text.Trim());
            }

            if (TabsItems > 7)
            {
                TabMetaDataArray.Add("8");
                TabMetaDataArray.Add(textBox27.Text.Trim());
            }

            if (TabsItems > 8)
            {
                TabMetaDataArray.Add("9");
                TabMetaDataArray.Add(textBox28.Text.Trim());
            }

            if (TabsItems > 9)
            {
                TabMetaDataArray.Add("10");
                TabMetaDataArray.Add(textBox29.Text.Trim());
            }

        }

        public bool ValidateTabs()
        {
            var dataOk = true;

            if (TabsItems > 0)
            {
                if (string.IsNullOrEmpty(textBox20.Text.Trim()))
                {
                    MessageBox.Show("Tab 1 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false; 
                }
            }

            if (TabsItems > 1)
            {
                if (string.IsNullOrEmpty(textBox21.Text.Trim()))
                {
                    MessageBox.Show("Tab 2 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (TabsItems > 2)
            {
                if (string.IsNullOrEmpty(textBox22.Text.Trim()))
                {
                    MessageBox.Show("Tab 3 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (TabsItems > 3)
            {
                if (string.IsNullOrEmpty(textBox23.Text.Trim()))
                {
                    MessageBox.Show("Tab 4 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (TabsItems > 4)
            {
                if (string.IsNullOrEmpty(textBox24.Text.Trim()))
                {
                    MessageBox.Show("Tab 5 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (TabsItems > 5)
            {
                if (string.IsNullOrEmpty(textBox25.Text.Trim()))
                {
                    MessageBox.Show("Tab 6 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (TabsItems > 6)
            {
                if (string.IsNullOrEmpty(textBox26.Text.Trim()))
                {
                    MessageBox.Show("Tab 7 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (TabsItems > 7)
            {
                if (string.IsNullOrEmpty(textBox27.Text.Trim()))
                {
                    MessageBox.Show("Tab 8 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (TabsItems > 8)
            {
                if (string.IsNullOrEmpty(textBox28.Text.Trim()))
                {
                    MessageBox.Show("Tab 9 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (TabsItems > 9)
            {
                if (string.IsNullOrEmpty(textBox29.Text.Trim()))
                {
                    MessageBox.Show("Tab 10 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            return dataOk;
        }

        public bool WriteTabMetaData()
        {
            PopulateTabArray();
            var tabs = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                tabs.DBConnection = "Data Source=" + CaseFileDB;
            else
                tabs.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            tabs.ClearTable("TabMetaData");

            var data = new Dictionary<string, string>();
            var result = false;

            for (int i = 0; i < TabMetaDataArray.Count; i+=2 )
            {
                data.Add("TabNum", TabMetaDataArray[i]);
                data.Add("TabKey", TabMetaDataArray[i+1]);
                result = tabs.Insert("TabMetaData", data);
                data.Clear();
            }

            if (TabMetaDataArray.Count == 0)
                return true;
            return result;
        }

        public int ReadTabMetaData(bool display)
        {
            var tabData = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                tabData.DBConnection = "Data Source=" + CaseFileDB;
            else
                tabData.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            try
            {
                var dt = tabData.GetDataTable("select * from TabMetaData;");
                var tabMetaDataArray = new List<string>();

                foreach (DataRow dr in dt.Rows)
                {
                    tabMetaDataArray.Add(dr[1].ToString());
                    tabMetaDataArray.Add(dr[2].ToString());
                }

                TabsItems = dt.Rows.Count;
                TabMetaDataArray = tabMetaDataArray;

                if (display)
                    DisplayTabs();

                return (dt.Rows.Count);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private void DisplayTabs()
        {
            if (TabsItems > 0)
            {
                textBox20.Text = TabMetaDataArray[1];
                textBox20.Enabled = true;
            }

            if (TabsItems > 1)
            {
                textBox21.Text = TabMetaDataArray[3];
                textBox21.Enabled = true;
            }

            if (TabsItems > 2)
            {
                textBox22.Text = TabMetaDataArray[5];
                textBox22.Enabled = true;
            }

            if (TabsItems > 3)
            {
                textBox23.Text = TabMetaDataArray[7];
                textBox23.Enabled = true;
            }

            if (TabsItems > 4)
            {
                textBox24.Text = TabMetaDataArray[9];
                textBox24.Enabled = true;
            }

            if (TabsItems > 5)
            {
                textBox25.Text = TabMetaDataArray[11];
                textBox25.Enabled = true;
            }

            if (TabsItems > 6)
            {
                textBox26.Text = TabMetaDataArray[13];
                textBox26.Enabled = true;
            }

            if (TabsItems > 7)
            {
                textBox27.Text = TabMetaDataArray[15];
                textBox27.Enabled = true;
            }

            if (TabsItems > 8)
            {
                textBox28.Text = TabMetaDataArray[17];
                textBox28.Enabled = true;
            }

            if (TabsItems > 9)
            {
                textBox29.Text = TabMetaDataArray[19];
                textBox29.Enabled = true;
            }
        }

        public bool WriteTabData()
        {
            var result = 0;
            var tabs = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                tabs.DBConnection = "Data Source=" + CaseFileDB;
            else
                tabs.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            tabs.ClearTable("TabData");

            try
            {
                for (var i = 0; i < (TabsItems * 3); i += 3)
                {
                    int tabNum;
                    int.TryParse(TabDataArray[i], out tabNum);
                    var tabKey = Functions.String2Byte(TabDataArray[i + 1]);
                    var tabValue = Functions.String2Byte(TabDataArray[i + 2]);
                    tabValue = Functions.Compress(tabValue);

                    string dbConnection;
                    if (string.IsNullOrEmpty(FilePass))
                        dbConnection = "Data Source=" + CaseFileDB;
                    else
                        dbConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

                    using (var connection = new SQLiteConnection(dbConnection))
                    using (var command = new SQLiteCommand(connection))
                    {
                        connection.Open();

                        command.CommandText =
                            "INSERT INTO TabData (TabNum, TabKey, TabValue) VALUES (@TabNum, @TabKey, @TabValue)";
                        command.Parameters.Add("@TabNum", DbType.String).Value = tabNum;
                        command.Parameters.Add("@TabKey", DbType.Binary).Value = tabKey;
                        command.Parameters.Add("@TabValue", DbType.Binary).Value = tabValue;

                        result = command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            { }

            catch(Exception fail)
            {
                //MessageBox.Show("Case: WriteTabData\r\n" + fail.Message);
            }

            TabDataArray.Clear();
            return (result > 0);
        }

        public bool ReadTabData()
        {
            var tab = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                tab.DBConnection = "Data Source=" + CaseFileDB;
            else
                tab.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            var dt = tab.GetDataTable("select * from TabData;");
            var tabDataArray = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                tabDataArray.Add(dr[1].ToString());
                tabDataArray.Add(Functions.Byte2String((byte[]) dr[2]));
                tabDataArray.Add (Functions.Byte2String(Functions.Decompress((byte[])dr[3])));
            }
            TabDataArray = tabDataArray;

            return (dt.Rows.Count > 0);
        }

        public bool WriteAuditData(string eventType, string eventDescription)
        {
            var result = 0;
            var userName = "Undefined";
            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (windowsIdentity != null) userName = windowsIdentity.Name;
            var machineName = System.Environment.MachineName;

            if (eventDescription.Length > 2) eventDescription += " User: '" + userName + "' logged in to: '" + machineName + "'";

            var date = DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss");
            var type = Functions.String2Byte(eventType);
            var description = Functions.String2Byte(eventDescription);
            var hash = Functions.GetMd5(date + eventType + eventDescription);

            string dbConnection;
            if (string.IsNullOrEmpty(FilePass))
                dbConnection = "Data Source=" + CaseFileDB;
            else
                dbConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            using (var connection = new SQLiteConnection(dbConnection))
            using (var command = new SQLiteCommand(connection))
            {
                connection.Open();

                command.CommandText = "INSERT INTO Audit (Date, EventType, EventDescription, Hash) VALUES (@Date, @EventType, @EventDescription, @Hash)";
                command.Parameters.Add("@Date", DbType.String).Value = date;
                command.Parameters.Add("@EventType", DbType.Binary).Value = type;
                command.Parameters.Add("@EventDescription", DbType.Binary).Value = description;
                command.Parameters.Add("@Hash", DbType.String).Value = hash;

                result = command.ExecuteNonQuery();
                connection.Close();
            }

            var audit = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                audit.DBConnection = "Data Source=" + CaseFileDB;
            else
                audit.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            var dt = audit.GetDataTable("select * from Audit;");
            var auditArray="";
            var hashes = "";

            foreach (DataRow dr in dt.Rows)
            {
                auditArray+=(dr[1] + "  " + Functions.Byte2String((byte[]) dr[2]) + ": " + Functions.Byte2String((byte[]) dr[3]));
                hashes += dr[4];
            }

            var auditHash = Functions.String2Byte(Functions.GetMd5(hashes));
            auditHash = Functions.Compress(auditHash);

            try
            {
                var commandValue = "";
                dt = audit.GetDataTable("select * from Hash;");
                if (dt.Rows.Count == 0)
                    commandValue = "INSERT INTO Hash (AuditHash) VALUES (@auditHash)";
                else
                    commandValue = "UPDATE Hash SET AuditHash = @auditHash";

                using (var connection = new SQLiteConnection(dbConnection))
                using (var command = new SQLiteCommand(connection))
                {
                    connection.Open();

                    command.CommandText = commandValue;
                    command.Parameters.Add("@AuditHash", DbType.Binary).Value = auditHash;

                    result = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
                return false;
            }

            return (result > 0);
        }

        public bool ReadAuditData()
        {
            var audit = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                audit.DBConnection = "Data Source=" + CaseFileDB;
            else
                audit.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            var dt = audit.GetDataTable("select * from Audit;");
            var auditArray = new List<string>();
            var auditOK = true;

            foreach (DataRow dr in dt.Rows)
            {
                auditOK = Functions.GetMd5(dr[1] + Functions.Byte2String((byte[])dr[2]) + Functions.Byte2String((byte[])dr[3])) == dr[4].ToString();
                if (auditOK) auditArray.Add(dr[1] + "  " + Functions.Byte2String((byte[])dr[2]) + ": " + Functions.Byte2String((byte[])dr[3]));
                else
                {
                    auditArray.Add(dr[1] + "  Warning: " + Functions.Byte2String((byte[])dr[3]));
                    auditArray.Add(new string(' ', 22) + "Failure - The Audit entry above failed HASH verification.");
                }
            }

            AuditArray = auditArray;

            return (auditOK);
        }

        public bool VerifyMetaData()
        {
            var metaData = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                metaData.DBConnection = "Data Source=" + CaseFileDB;
            else
                metaData.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            var dt = metaData.GetDataTable("select * from MetaData;");
            var MetaOK = true;
            var hashes = "";

            foreach (DataRow dr in dt.Rows)
            {
                var hash = Functions.GetMd5(dr[2].ToString());
                hashes += Functions.GetMd5(dr[2].ToString());
                if (hash != dr[3].ToString())
                    MetaOK = false;
            }

            byte[] MetaBlob;
            dt = metaData.GetDataTable("select * from Hash;");
            foreach (DataRow dr in dt.Rows)
            {
                MetaBlob = Functions.Decompress((byte[])dr[0]);
                var hash = Functions.GetMd5(hashes);
                if (hash != Functions.Byte2String(MetaBlob))
                    MetaOK = false;
            }

            return MetaOK;
        }

        public bool VerifyAuditData()
        {
            var auditData = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                auditData.DBConnection = "Data Source=" + CaseFileDB;
            else
                auditData.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            var dt = auditData.GetDataTable("select * from Audit;");
            var auditArray = new List<string>();
            var audit1OK = true;
            var audit2OK = true;
            var hashes = "";

            foreach (DataRow dr in dt.Rows)
            {
                audit1OK = Functions.GetMd5(dr[1] + Functions.Byte2String((byte[])dr[2]) + Functions.Byte2String((byte[])dr[3])) == dr[4].ToString();
                auditArray.Add(dr[1] + "  " + Functions.Byte2String((byte[])dr[2]) + ": " + Functions.Byte2String((byte[])dr[3]));
                hashes += dr[4].ToString();
                if (!audit1OK) break;
            }

            dt = auditData.GetDataTable("select * from Hash;");
            foreach (DataRow dr in dt.Rows)
            {
                var auditBlob = Functions.Decompress((byte[])dr[2]);
                var hash = Functions.GetMd5(hashes);
                audit2OK = (hash == Functions.Byte2String(auditBlob));
            }

            return audit1OK && audit2OK;
        }

        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            Result = true;
        }

        private void NumTabsTextChanged(object sender, EventArgs e)
        {
            int t;
            if (Int32.TryParse(NumTabs.Text, out t))
            {
                if (t < TabsItems) NumTabs.Text = TabsItems.ToString();
            }
            else NumTabs.Text = TabsItems.ToString();
            
        }
    }
}