using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace FirstResponse.CaseNotes
{
    public partial class UpdatePrefs : Form
    {
        private string CaseNotesDB = "";
        private bool CaseOpen;

        public UpdatePrefs(string caseNotesDB, bool caseOpen)
        {
            InitializeComponent();
            CaseNotesDB = caseNotesDB;
            CaseOpen = caseOpen;
            var update = false;
            if (!MainForm.Professional)
                tabControl.TabPages.RemoveAt(2);

            var prefs = new Preferences();
            prefs.ReadSystemPrefs(caseNotesDB, null, "open");
            prefs.ReadUserPrefs(caseNotesDB, null);

            #region Font Data

            FontFamily[] ff = FontFamily.Families;
            var m = 0;
            var n = 0;

            string[] items = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            string[] sizes = { "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72" };

            NumPrefItems.Items.Clear();

            int count = MainForm.Professional ? 10 : 4;
            if (MainForm.Professional)
            {
                label49.Text = "You have the following Templates already defined:"; 
                label6.Text = "Number of Tab windows to display:";
                NumTabs.Left = 240;
            }
            else
            {
                label49.Text = "You have the following CaseNotes template defined (Lite version limited to 1):";
                label6.Text = "Number of Tab windows to display (Lite version limited to 4):";
                NumTabs.Left = 309;
            }

            for (int x = 0; x < 10; x++)
            {
                NumPrefItems.Items.Add(items[x]);
            }

            NumTabs.Items.Add("0");
            for (int x = 0; x < count; x++)
            {
                NumTabs.Items.Add(items[x]);
            }

            NumPrefItems.SelectedIndex = prefs.MetaDataItems -1;
            NumTabs.SelectedIndex = prefs.TabItems;

            for (int x = 0; x < ff.Length; x++)
            {
                MetaCombo.Items.Add(ff[x].Name);
                if (ff[x].Name == prefs.MetadataTypeface) m = x;

                NoteCombo.Items.Add(ff[x].Name);
                if (ff[x].Name == prefs.NoteTypeface) n = x;
            }

            MetaCombo.SelectedIndex = m;
            NoteCombo.SelectedIndex = n;

            for (int x = 0; x < sizes.Length; x++)
            {
                MetaSizeCombo.Items.Add(sizes[x]);
                if (sizes[x] == prefs.MetadataTypeSize.ToString()) m = x;

                NoteSizeCombo.Items.Add(sizes[x]);
                if (sizes[x] == prefs.NoteTypeSize.ToString()) n = x;
            }

            MetaSizeCombo.SelectedIndex = m;
            NoteSizeCombo.SelectedIndex = n;

            #endregion

            #region MetaData Items

            if (prefs.MetaDataItems > 0)
            {
                textBox1.Text = prefs.MetaDescArray[0];
                textBox2.Text = prefs.MetaValueArray[0];
                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }

            if (prefs.MetaDataItems > 1)
            {
                textBox3.Text = prefs.MetaDescArray[1];
                textBox4.Text = prefs.MetaValueArray[1];
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            }

            if (prefs.MetaDataItems > 2)
            {
                textBox5.Text = prefs.MetaDescArray[2];
                textBox6.Text = prefs.MetaValueArray[2];
                textBox5.Enabled = true;
                textBox6.Enabled = true;
            }

            if (prefs.MetaDataItems > 3)
            {
                textBox7.Text = prefs.MetaDescArray[3];
                textBox8.Text = prefs.MetaValueArray[3];
                textBox7.Enabled = true;
                textBox8.Enabled = true;
            }

            if (prefs.MetaDataItems > 4)
            {
                textBox9.Text = prefs.MetaDescArray[4];
                textBox10.Text = prefs.MetaValueArray[4];
                textBox9.Enabled = true;
                textBox10.Enabled = true;
            }

            if (prefs.MetaDataItems > 5)
            {
                textBox11.Text = prefs.MetaDescArray[5];
                textBox12.Text = prefs.MetaValueArray[5];
                textBox11.Enabled = true;
                textBox12.Enabled = true;
            }

            if (prefs.MetaDataItems > 6)
            {
                textBox13.Text = prefs.MetaDescArray[6];
                textBox14.Text = prefs.MetaValueArray[6];
                textBox13.Enabled = true;
                textBox14.Enabled = true;
            }

            if (prefs.MetaDataItems > 7)
            {
                textBox15.Text = prefs.MetaDescArray[7];
                textBox16.Text = prefs.MetaValueArray[7];
                textBox15.Enabled = true;
                textBox16.Enabled = true;
            }

            if (prefs.MetaDataItems > 8)
            {
                textBox17.Text = prefs.MetaDescArray[8];
                textBox18.Text = prefs.MetaValueArray[8];
                textBox17.Enabled = true;
                textBox18.Enabled = true;
            }

            if (prefs.MetaDataItems > 9)
            {
                textBox19.Text = prefs.MetaDescArray[9];
                textBox20.Text = prefs.MetaValueArray[9];
                textBox19.Enabled = true;
                textBox20.Enabled = true;
            }

            #endregion

            #region Tab Data Items

            if (prefs.TabItems > 0)
            {
                textBox21.Text = prefs.TabsValueArray[0];
                textBox21.Enabled = true;
            }

            if (prefs.TabItems > 1)
            {
                textBox22.Text = prefs.TabsValueArray[1];
                textBox22.Enabled = true;
            }

            if (prefs.TabItems > 2)
            {
                textBox23.Text = prefs.TabsValueArray[2];
                textBox23.Enabled = true;
            }

            if (prefs.TabItems > 3)
            {
                textBox24.Text = prefs.TabsValueArray[3];
                textBox24.Enabled = true;
            }

            if (prefs.TabItems > 4)
            {
                textBox25.Text = prefs.TabsValueArray[4];
                textBox25.Enabled = true;
            }

            if (prefs.TabItems > 5)
            {
                textBox26.Text = prefs.TabsValueArray[5];
                textBox26.Enabled = true;
            }

            if (prefs.TabItems > 6)
            {
                textBox27.Text = prefs.TabsValueArray[6];
                textBox27.Enabled = true;
            }

            if (prefs.TabItems > 7)
            {
                textBox28.Text = prefs.TabsValueArray[7];
                textBox28.Enabled = true;
            }

            if (prefs.TabItems > 8)
            {
                textBox29.Text = prefs.TabsValueArray[8];
                textBox29.Enabled = true;
            }

            if (prefs.TabItems > 9)
            {
                textBox30.Text = prefs.TabsValueArray[9];
                textBox30.Enabled = true;
            }
            #endregion

            lblLastUpdated.Text = prefs.DateLastUpdated;
            txtDefaultLocation.Text = prefs.DefaultLocation;
            txtChecklistLocation.Text = prefs.ChecklistLocation;
            txtTemplatesLocation.Text = prefs.TemplateLocation;
            txtMainTitle.Text = prefs.MainTitle;
            lbxCaseStatus.Items.Clear();
            lbxCaseStatus.Items.AddRange(prefs.CaseStatus.Split('#'));

            string temp = Application.UserAppDataPath;
            temp = temp.Substring(0, temp.LastIndexOf("\\")) + "\\Templates";

            if (txtChecklistLocation.Text == temp)
            {
                if (!Directory.Exists(temp))
                    Directory.CreateDirectory(temp);

                update = true;
            }

            if (txtTemplatesLocation.Text == temp)
            {
                if (!Directory.Exists(temp))
                    Directory.CreateDirectory(temp);
                
                update = true;
            }

            if (update)
            {
                prefs.ChecklistLocation = txtChecklistLocation.Text.Trim();
                prefs.TemplateLocation = txtTemplatesLocation.Text.Trim();
                prefs.WriteSystemPrefs(CaseNotesDB, null);
            }

            PopulateChecklist();
            PopulateTemplates();
        }

        private void PrefsCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void PrefsOkClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var dataOk = true;
            int metaDataItems = NumPrefItems.SelectedIndex + 1;
            int tabItems = NumTabs.SelectedIndex;

            if (metaDataItems < 1) metaDataItems = 1;

            #region Clear unused MeteData

            if (metaDataItems < 1)
            {
                metaDataItems = 1;
                textBox1.Text = "";
                textBox2.Text = "";
            }

            if (metaDataItems < 2)
            {
                textBox3.Text = "";
                textBox4.Text = "";
            }

            if (metaDataItems < 3)
            {
                textBox5.Text = "";
                textBox6.Text = "";
            }

            if (metaDataItems < 4)
            {
                textBox7.Text = "";
                textBox8.Text = "";
            }

            if (metaDataItems < 5)
            {
                textBox9.Text = "";
                textBox10.Text = "";
            }

            if (metaDataItems < 6)
            {
                textBox11.Text = "";
                textBox12.Text = "";
            }

            if (metaDataItems < 7)
            {
                textBox13.Text = "";
                textBox14.Text = "";
            }

            if (metaDataItems < 8)
            {
                textBox15.Text = "";
                textBox16.Text = "";
            }

            if (metaDataItems < 9)
            {
                textBox17.Text = "";
                textBox18.Text = "";
            }

            if (metaDataItems < 10)
            {
                textBox19.Text = "";
                textBox20.Text = "";
            }
            #endregion

            #region Clear unused Tabs

            if (tabItems < 1)
                textBox21.Text = "";
            else
            {
                if (textBox21.Text == "")
                {
                    MessageBox.Show("Tab 1 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (tabItems < 2)
                textBox22.Text = "";
            else
            {
                if (textBox22.Text == "")
                {
                    MessageBox.Show("Tab 2 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (tabItems < 3)
                textBox23.Text = "";
            else
            {
                if (textBox23.Text == "")
                {
                    MessageBox.Show("Tab 3 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (tabItems < 4)
                textBox24.Text = "";
            else
            {
                if (textBox24.Text == "")
                {
                    MessageBox.Show("Tab 4 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (tabItems < 5)
                textBox25.Text = "";
            else
            {
                if (textBox25.Text == "")
                {
                    MessageBox.Show("Tab 5 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (tabItems < 6)
                textBox26.Text = "";
            else
            {
                if (textBox26.Text == "")
                {
                    MessageBox.Show("Tab 6 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (tabItems < 7)
                textBox27.Text = "";
            else
            {
                if (textBox27.Text == "")
                {
                    MessageBox.Show("Tab 7 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (tabItems < 8)
                textBox28.Text = "";
            else
            {
                if (textBox28.Text == "")
                {
                    MessageBox.Show("Tab 8 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (tabItems < 9)
                textBox29.Text = "";
            else
            {
                if (textBox29.Text == "")
                {
                    MessageBox.Show("Tab 9 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }

            if (tabItems < 10)
                textBox30.Text = "";
            else
            {
                if (textBox30.Text == "")
                {
                    MessageBox.Show("Tab 10 is blank - please give it a value!", "Blank Tab Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataOk = false;
                }
            }


            #endregion

            if (!Directory.Exists(txtDefaultLocation.Text.Trim()))
            {
                MessageBox.Show("Warning! The default folder location does not exist!", "Invalid Folder Defined", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataOk = false;
            }

            if (!dataOk)
            { 
                Cursor = Cursors.Default; 
                return;
            }

            var metadataTypeSize = 0;
            var noteTypeSize = 0;

            var prefs = new Preferences();
            prefs.MetaDataItems = metaDataItems;
            prefs.TabItems = tabItems;
            prefs.DefaultLocation = txtDefaultLocation.Text.Trim();
            prefs.ChecklistLocation = txtChecklistLocation.Text.Trim();
            prefs.TemplateLocation = txtTemplatesLocation.Text.Trim(); 
            prefs.MetadataTypeface = (string) MetaCombo.SelectedItem;
            int.TryParse(MetaSizeCombo.SelectedItem.ToString(), out metadataTypeSize);
            prefs.NoteTypeface = (string) NoteCombo.SelectedItem;
            int.TryParse(NoteSizeCombo.SelectedItem.ToString(), out noteTypeSize);
            prefs.DateLastUpdated = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();
            prefs.MainTitle = txtMainTitle.Text;
            prefs.MetadataTypeSize = metadataTypeSize;
            prefs.NoteTypeSize = noteTypeSize;
            prefs.CaseStatus = "";
            for (int i = 0; i < lbxCaseStatus.Items.Count; i++)
            {
                prefs.CaseStatus += lbxCaseStatus.Items[i] + "#";
            }
            prefs.CaseStatus = prefs.CaseStatus.Substring(0, prefs.CaseStatus.Length - 1);

            prefs.WriteSystemPrefs(CaseNotesDB, null);

            var metaDescArray = new List<string>();
            var metaValueArray = new List<string>();
            var tabsValueArray = new List<string>();
           
            if (metaDataItems > 0)
            {
                metaDescArray.Add(textBox1.Text.Trim());
                metaValueArray.Add(textBox2.Text.Trim());
            }

            if (metaDataItems > 1)
            {
                metaDescArray.Add(textBox3.Text.Trim());
                metaValueArray.Add(textBox4.Text.Trim());
            }

            if (metaDataItems > 2)
            {
                metaDescArray.Add(textBox5.Text.Trim());
                metaValueArray.Add(textBox6.Text.Trim());
            }

            if (metaDataItems > 3)
            {
                metaDescArray.Add(textBox7.Text.Trim());
                metaValueArray.Add(textBox8.Text.Trim());
            }

            if (metaDataItems > 4)
            {
                metaDescArray.Add(textBox9.Text.Trim());
                metaValueArray.Add(textBox10.Text.Trim());
            }

            if (metaDataItems > 5)
            {
                metaDescArray.Add(textBox11.Text.Trim());
                metaValueArray.Add(textBox12.Text.Trim());
            }

            if (metaDataItems > 6)
            {
                metaDescArray.Add(textBox13.Text.Trim());
                metaValueArray.Add(textBox14.Text.Trim());
            }

            if (metaDataItems > 7)
            {
                metaDescArray.Add(textBox15.Text.Trim());
                metaValueArray.Add(textBox16.Text.Trim());
            }

            if (metaDataItems > 8)
            {
                metaDescArray.Add(textBox17.Text.Trim());
                metaValueArray.Add(textBox18.Text.Trim());
            }

            if (metaDataItems > 9)
            {
                metaDescArray.Add(textBox19.Text.Trim());
                metaValueArray.Add(textBox20.Text.Trim());
            }

            if (tabItems > 0)
                tabsValueArray.Add(textBox21.Text.Trim());

            if (tabItems > 1)
                tabsValueArray.Add(textBox22.Text.Trim());

            if (tabItems > 2)
                tabsValueArray.Add(textBox23.Text.Trim());

            if (tabItems > 3)
                tabsValueArray.Add(textBox24.Text.Trim());

            if (tabItems > 4)
                tabsValueArray.Add(textBox25.Text.Trim());

            if (tabItems > 5)
                tabsValueArray.Add(textBox26.Text.Trim());

            if (tabItems > 6)
                tabsValueArray.Add(textBox27.Text.Trim());

            if (tabItems > 7)
                tabsValueArray.Add(textBox28.Text.Trim());

            if (tabItems > 8)
                tabsValueArray.Add(textBox29.Text.Trim());

            if (tabItems > 9)
                tabsValueArray.Add(textBox30.Text.Trim());

            prefs.MetaDescArray = metaDescArray;
            prefs.MetaValueArray = metaValueArray;
            prefs.TabsValueArray = tabsValueArray;
            prefs.WriteUserPrefs(CaseNotesDB, null);

            Cursor = Cursors.Default;
            Close();
        } 

        private void NumPrefItemsSelectedIndexChanged(object sender, EventArgs e)
        {
            int metaDataItems = NumPrefItems.SelectedIndex +1;

            if (metaDataItems > 0)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
            }

            if (metaDataItems > 1)
            {
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            }
            else
            {
                textBox3.Enabled = false;
                textBox4.Enabled = false;
            }

            if (metaDataItems > 2)
            {
                textBox5.Enabled = true;
                textBox6.Enabled = true;
            }
            else
            {
                textBox5.Enabled = false;
                textBox6.Enabled = false;
            }

            if (metaDataItems > 3)
            {
                textBox7.Enabled = true;
                textBox8.Enabled = true;
            }
            else
            {
                textBox7.Enabled = false;
                textBox8.Enabled = false;
            }

            if (metaDataItems > 4)
            {
                textBox9.Enabled = true;
                textBox10.Enabled = true;
            }
            else
            {
                textBox9.Enabled = false;
                textBox10.Enabled = false;
            }

            if (metaDataItems > 5)
            {
                textBox11.Enabled = true;
                textBox12.Enabled = true;
            }
            else
            {
                textBox11.Enabled = false;
                textBox12.Enabled = false;
            }

            if (metaDataItems > 6)
            {
                textBox13.Enabled = true;
                textBox14.Enabled = true;
            }
            else
            {
                textBox13.Enabled = false;
                textBox14.Enabled = false;
            }

            if (metaDataItems > 7)
            {
                textBox15.Enabled = true;
                textBox16.Enabled = true;
            }
            else
            {
                textBox15.Enabled = false;
                textBox16.Enabled = false;
            }

            if (metaDataItems > 8)
            {
                textBox17.Enabled = true;
                textBox18.Enabled = true;
            }
            else
            {
                textBox17.Enabled = false;
                textBox18.Enabled = false;
            }

            if (metaDataItems > 9)
            {
                textBox19.Enabled = true;
                textBox20.Enabled = true;
            }
            else
            {
                textBox19.Enabled = false;
                textBox20.Enabled = false;
            }

        }

        private void NumTabsSelectedIndexChanged(object sender, EventArgs e)
        {
            int tabItems = NumTabs.SelectedIndex;

            if (tabItems > 0)
            {
                textBox21.Enabled = true;
            }
            else
            {
                textBox21.Enabled = false;
            }

            if (tabItems > 1)
            {
                textBox22.Enabled = true;
            }
            else
            {
                textBox22.Enabled = false;
            }

            if (tabItems > 2)
            {
                textBox23.Enabled = true;
            }
            else
            {
                textBox23.Enabled = false;
            }

            if (tabItems > 3)
            {
                textBox24.Enabled = true;
            }
            else
            {
                textBox24.Enabled = false;
            }

            if (tabItems > 4)
            {
                textBox25.Enabled = true;
            }
            else
            {
                textBox25.Enabled = false;
            }

            if (tabItems > 5)
            {
                textBox26.Enabled = true;
            }
            else
            {
                textBox26.Enabled = false;
            }

            if (tabItems > 6)
            {
                textBox27.Enabled = true;
            }
            else
            {
                textBox27.Enabled = false;
            }

            if (tabItems > 7)
            {
                textBox28.Enabled = true;
            }
            else
            {
                textBox28.Enabled = false;
            }

            if (tabItems > 8)
            {
                textBox29.Enabled = true;
            }
            else
            {
                textBox29.Enabled = false;
            }

            if (tabItems > 9)
            {
                textBox30.Enabled = true;
            }
            else
            {
                textBox30.Enabled = false;
            }

        }

        private void PrefsBrowseClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDefaultLocation.Text)) txtDefaultLocation.Text = "c:\\";

            var openDialog = new FolderBrowserDialog {Description = "Select a default folder location", SelectedPath = txtDefaultLocation.Text};

            if (openDialog.ShowDialog() == DialogResult.OK)
                txtDefaultLocation.Text = openDialog.SelectedPath;
            
        }
        
        private void BtnNewClick(object sender, EventArgs e)
        {
            var ncl = new CheckList(null, txtChecklistLocation.Text);
            ncl.ShowDialog();
            PopulateChecklist();
            ncl = null;
        }

        private void BtnEditClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(LbxChecklistList.SelectedItem.ToString()))
            {
                var ncl = new CheckList(LbxChecklistList.SelectedItem.ToString(), txtChecklistLocation.Text);
                ncl.ShowDialog();
                PopulateChecklist();
                ncl = null;
            }
        }

        private void BtnDeleteClick(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Are you sure you want to delete the " + lblName.Text + " checklist?", "Confirm Checklist Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                try
                {
                    File.Delete(txtChecklistLocation.Text.Trim() + "\\" + LbxChecklistList.SelectedItem.ToString());
                    PopulateChecklist();
                }
                catch (Exception fail)
                {
                    MessageBox.Show("The deletion failed: " + fail.Message, "Checklist Deletion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnBrowseClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtChecklistLocation.Text)) txtChecklistLocation.Text = "c:\\";

            var openDialog = new FolderBrowserDialog { Description = "Select a default folder location", SelectedPath = txtChecklistLocation.Text };

            if (openDialog.ShowDialog() == DialogResult.OK)
                txtChecklistLocation.Text = openDialog.SelectedPath;

            PopulateChecklist();

        }

        private void BtnTemplateBrowseClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTemplatesLocation.Text)) txtTemplatesLocation.Text = "c:\\";

            var openDialog = new FolderBrowserDialog { Description = "Select a default folder location", SelectedPath = txtTemplatesLocation.Text };

            if (openDialog.ShowDialog() == DialogResult.OK)
                txtTemplatesLocation.Text = openDialog.SelectedPath;

            PopulateTemplates();
        }

        private void LbxChecklistListSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(LbxChecklistList.SelectedItem.ToString()))
            {
                ReadChecklistMetadata(txtChecklistLocation.Text + "\\" + LbxChecklistList.SelectedItem.ToString());
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void LbxChecklistListDoubleClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(LbxChecklistList.SelectedItem.ToString()))
            {
                var ncl = new CheckList(LbxChecklistList.SelectedItem.ToString(), txtChecklistLocation.Text);
                ncl.ShowDialog();
                PopulateChecklist();
                ncl.Dispose();
            }
        }

        private void ReadChecklistMetadata(string fileName)
        {
            var xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreWhitespace = true;

            try
            {
                using (XmlReader reader = XmlReader.Create(fileName, xmlReaderSettings))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            if (reader.Name == "Checklist")
                            {
                                lblName.Text = reader["Name"];
                                lblDescription.Text = reader["Description"];
                                lblAuthor.Text = reader["Author"];
                                lblDateCreated.Text = reader["Created"];
                                lblDateModified.Text = reader["Modified"];
                                lblVersion.Text = reader["Version"];
                                lblName.Visible = true;
                                lblDescription.Visible = true;
                                lblAuthor.Visible = true;
                                lblDateCreated.Visible = true;
                                lblDateModified.Visible = true;
                                lblVersion.Visible = true;
                            }
                        }
                    }
                }
            }

            catch (Exception crap)
            {
                MessageBox.Show("This doesn't appear to be a valid CaseNotes Checklist file.\r\nUpdate: " + crap.Message, "Checklist XML Read Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void PopulateChecklist()
        {
            LbxChecklistList.Items.Clear();
            txtChecklistLocation.ForeColor = Color.Black;
            try
            {
                if (Directory.Exists(txtChecklistLocation.Text.Trim()))
                {
                    string[] fileAarray = Directory.GetFiles(txtChecklistLocation.Text.Trim(), "*.xml");
                    foreach (var entry in fileAarray)
                    {
                        LbxChecklistList.Items.Add(entry.Substring(entry.LastIndexOf('\\') + 1));
                    }
                }
                else
                {
                    txtChecklistLocation.ForeColor = Color.Red;
                }

                if (LbxChecklistList.Items.Count > 0)
                {
                    LbxChecklistList.SelectedIndex = 0;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                }
                else
                {
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    lblName.Visible = false;
                    lblDescription.Visible = false;
                    lblAuthor.Visible = false;
                    lblDateCreated.Visible = false;
                    lblDateModified.Visible = false;
                    lblVersion.Visible = false;
                }
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message, "Checklist Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateTemplates()
        {
            lbxTemplateList.Items.Clear();
            txtTemplatesLocation.ForeColor = Color.Black;

            try
            {
                if (Directory.Exists(txtTemplatesLocation.Text.Trim()))
                {
                    string[] fileAarray = Directory.GetFiles(txtTemplatesLocation.Text.Trim(), "*.rtf");
                    foreach (var entry in fileAarray)
                    {
                        lbxTemplateList.Items.Add(entry.Substring(entry.LastIndexOf('\\') + 1));
                        if (!MainForm.Professional && lbxTemplateList.Items.Count == 1)
                            break;
                    }
                }
                else
                {
                    txtTemplatesLocation.ForeColor = Color.Red;
                }

                if (lbxTemplateList.Items.Count > 0)
                {
                    lbxTemplateList.SelectedIndex = 0;
                    btnEditTemplate.Enabled = true;
                    btnDeleteTemplate.Enabled = true;
                }
                else
                {
                    btnEditTemplate.Enabled = false;
                    btnDeleteTemplate.Enabled = false;
                }
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message, "Template Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LbxTemplateListSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbxTemplateList.SelectedItem.ToString()))
            {
                rtbPreview.LoadFile(txtTemplatesLocation.Text + "\\" + lbxTemplateList.SelectedItem.ToString());
                btnEditTemplate.Enabled = true;
                btnDeleteTemplate.Enabled = true;
            }
            else
            {
                btnEditTemplate.Enabled = false;
                btnDeleteTemplate.Enabled = false;
                rtbPreview = null;
            }
        }

        private void LbxTemplateListDoubleClicked(object sender, EventArgs e)
        {
            btnEditTemplate.PerformClick();
        }

        private void BtnNewTemplateClick(object sender, EventArgs e)
        {
            var ntm = new Template(null, txtTemplatesLocation.Text, CaseNotesDB);
            ntm.ShowDialog();
            PopulateTemplates();
            ntm = null;
        }

        private void BtnEditTemplateClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbxTemplateList.SelectedItem.ToString()))
            {
                var ntm = new Template(lbxTemplateList.SelectedItem.ToString(), txtTemplatesLocation.Text, CaseNotesDB);
                ntm.ShowDialog();
                PopulateTemplates();
                ntm = null;
            }
        }

        private void BtnDeleteTemplateClick(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Are you sure you want to delete this template?", "Confirm Template Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                try
                {
                    File.Delete(txtTemplatesLocation.Text + "\\" + lbxTemplateList.SelectedItem.ToString());
                    PopulateTemplates();
                }
                catch (Exception fail)
                {
                    MessageBox.Show("The deletion failed: " + fail.Message, "Template Deletion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lbxCaseStatusSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxCaseStatus.SelectedItem != null)
                btnRemove.Enabled = true;
            else btnRemove.Enabled = false;
        }

        private void btnRemoveClick(object sender, EventArgs e)
        {
            if (lbxCaseStatus.SelectedItem != null)
                lbxCaseStatus.Items.RemoveAt(lbxCaseStatus.SelectedIndex);

            btnRemove.Enabled = false;
        }

        private void tbxNewStatusTextChanged(object sender, EventArgs e)
        {
            if (tbxNewStatus.Text.Length > 0)
                btnAdd.Enabled = true;
            else btnAdd.Enabled = false;
        }

        private void btnAddClick(object sender, EventArgs e)
        {
            if (lbxCaseStatus.Items.Contains(tbxNewStatus.Text))
                MessageBox.Show("This Case Status has already been defined!", "Case Status Duplication", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                lbxCaseStatus.Items.Add(tbxNewStatus.Text);
                tbxNewStatus.Text = "";
            }
        }

        private void btnUpClick(object sender, EventArgs e)
        {
            if (lbxCaseStatus.SelectedItem != null && lbxCaseStatus.SelectedIndex > 0)
            {
                lbxCaseStatus.Items.Insert(lbxCaseStatus.SelectedIndex - 1, lbxCaseStatus.Text);
                lbxCaseStatus.SelectedIndex = (lbxCaseStatus.SelectedIndex - 2);
                lbxCaseStatus.Items.RemoveAt(lbxCaseStatus.SelectedIndex + 2);
            }
        }

        private void btnDownClick(object sender, EventArgs e)
        {
            if (lbxCaseStatus.SelectedItem != null && (lbxCaseStatus.SelectedIndex < lbxCaseStatus.Items.Count -1))
            {
                lbxCaseStatus.Items.Insert(lbxCaseStatus.SelectedIndex + 2, lbxCaseStatus.Text);
                lbxCaseStatus.SelectedIndex = lbxCaseStatus.SelectedIndex + 2;
                lbxCaseStatus.Items.RemoveAt(lbxCaseStatus.SelectedIndex - 2);
            }
        }

    }
}