using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace FirstResponse.CaseNotes
{
    public partial class SelectCheckList : Form
    {
        private static string _checklistLocation = "";
        public string SelectedChecklistPath = "";
        public string SelectedChecklistName = "";

        public SelectCheckList(string checklistLocation)
        {
            InitializeComponent();
            _checklistLocation = checklistLocation;
            PopulateChecklist();
        }

        private void LbxTemplateListSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbxTemplateList.SelectedItem.ToString()))
                ReadChecklistMetadata(_checklistLocation + "\\" + lbxTemplateList.SelectedItem.ToString());
        }

        private void ReadChecklistMetadata(string fileName)
        {
            var Gui = new GuiController();
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
                                Gui.Name = reader["Name"];
                                Gui.Description = reader["Description"];
                                Gui.Author = reader["Author"];
                                Gui.Created = reader["Created"];
                                Gui.Modified = reader["Modified"];
                                Gui.Version = reader["Version"];

                                lblName.Text = Gui.Name;
                                lblDescription.Text = Gui.Description;
                                lblAuthor.Text = Gui.Author;
                                lblDateCreated.Text = Gui.Created;
                                lblDateModified.Text = Gui.Modified;
                                lblVersion.Text = Gui.Version;
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
                MessageBox.Show("This doesn't appear to be a valid CaseNotes Checklist file.\r\nSelect: " + crap.Message, "Checklist XML Read Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void PopulateChecklist()
        {
            lbxTemplateList.Items.Clear();

            string[] fileAarray = Directory.GetFiles(_checklistLocation, "*.xml");

            foreach (var entry in fileAarray)
            {
                lbxTemplateList.Items.Add(entry.Substring(entry.LastIndexOf('\\') + 1));
            }

            if (lbxTemplateList.Items.Count > 0)
                lbxTemplateList.SelectedIndex = 0;
            else
            {
                btnOK.Enabled = false;
                MessageBox.Show("Warning - you don't have any valid Checklists to choose from!", "Checklist Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            if (lbxTemplateList.Items.Count > 0)
            {
                SelectedChecklistPath = _checklistLocation + "\\" + lbxTemplateList.SelectedItem.ToString();
                SelectedChecklistName = lblName.Text;
            }
            Close();
        }
    }
}
