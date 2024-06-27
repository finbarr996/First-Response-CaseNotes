using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using FirstResponse.CaseNotes.Classes;

namespace FirstResponse.CaseNotes
{
    public partial class CheckList : Form
    {
        Timer time = new Timer();
        public GuiController Gui = new GuiController(); 
        private bool _comboSet;
        private bool _listSet;
        private string _chkListFileName;
        private string _chkListPath;
        private bool _changed;

        public CheckList(string chkListFileName, string chkListPath)
        {
            InitializeComponent();
            time.Enabled = true;
            time.Interval = 5;
            time.Tick += (TimeTick);
            lblMulti.Visible = false;
            txtBoxMulti.Visible = false;
            checkListPanel.Controls.Clear();

            if (!string.IsNullOrEmpty(chkListPath))
                _chkListPath = chkListPath;
            if (!string.IsNullOrEmpty(chkListFileName))
                _chkListFileName = chkListPath + "\\" + chkListFileName;

        }

        private void TimeTick(object sender, EventArgs e)
        {
            //Adding XListBox
            if (Gui.lbx_varpub)
            {
                if (!Gui.lbx_close)
                {
                    var lbx = new XListBox(Gui, true);
                    lbx.Parent = checkListPanel;
                    lbx.Name = "ListBox" + Gui.a;
                    lbx.Text = "ListBox" + Gui.a;
                    lbx.Location = new Point(5, 5);
                    lbx.Size = new Size(130,43);
                    lbx.AutoSize = true;
                    Gui.ControlList.Add(lbx);
                    Gui.lbx_close = true;
                }
            }

            //Adding XCheckBox
            if (Gui.c_varpub)
            {
                if (!Gui.c_close)
                {
                    var chk = new XCheckBox(Gui);
                    chk.Parent = checkListPanel; 
                    chk.Name = "CheckBox" + Gui.c;
                    chk.Text = "CheckBox" + Gui.c;
                    chk.Location = new Point(5, 5);
                    chk.AutoSize = true;
                    Gui.ControlList.Add(chk);
                    Gui.c_close = true;
                }
            }

            //Adding XComboBox
            if (Gui.co_varpub)
            {
                if (!Gui.co_close)
                {
                    var cbo = new XComboBox(Gui, true);
                    cbo.Parent = checkListPanel; 
                    cbo.Name = "ComboBox" + Gui.co;
                    cbo.Text = "ComboBox" + Gui.co;
                    cbo.Location = new Point(5, 5);
                    cbo.Size = new Size(121,21);
                    cbo.AutoSize = true;
                    Gui.ControlList.Add(cbo);
                    Gui.co_close = true;
                }
            }

            //Adding XLabel
            if (Gui.l_varpub)
            {
                if (!Gui.l_close)
                {
                    var lbl = new XLabel(Gui);
                    lbl.Parent = checkListPanel;
                    lbl.Name = "Label" + Gui.l;
                    lbl.Text = "Label" + Gui.l;
                    lbl.Location = new Point(5, 5);
                    lbl.AutoSize = true;
                    Gui.ControlList.Add(lbl);
                    Gui.l_close = true;
                }
            }

            //Adding XPictureBox
            if (Gui.pic_varpub)
            {
                if (!Gui.pic_close)
                {
                    var picx = new XPictureBox(Gui);
                    picx.Parent = checkListPanel;
                    picx.Name = "PictureBox" + Gui.pic;
                    picx.Text = "PictureBox" + Gui.pic;
                    picx.Location = new Point(5, 5);
                    picx.AutoSize = true;
                    Gui.ControlList.Add(picx);
                    Gui.pic_close = true;
                }
            }

            //Adding XRadioButton
            if (Gui.rad_varpub)
            {
                if (!Gui.rad_close)
                {
                    var radio = new XRadioButton(Gui);
                    radio.Parent = checkListPanel;
                    radio.Name = "RadioButton" + Gui.rad;
                    radio.Text = "RadioButton" + Gui.rad;
                    radio.Location = new Point(5, 5);
                    radio.AutoSize = true;
                    Gui.ControlList.Add(radio);
                    Gui.rad_close = true;
                }
            }

            //Adding XTextbox
            if (Gui.txt_varpub)
            {
                if (!Gui.txt_close)
                {
                    var txt1 = new XTextBox(Gui);
                    txt1.Parent = checkListPanel;
                    txt1.Name = "TextBox" + Gui.txt;
                    txt1.Text = "TextBox" + Gui.txt;
                    txt1.Location = new Point(5, 5);
                    Gui.ControlList.Add(txt1);
                    Gui.txt_close = true;
                }
            }

            foreach (Control v in checkListPanel.Controls)
            {
                Gui.Controller.Add(v);
                if (v.Focused)
                {
                    Gui.SelectedControl = v;
                    Gui.control = v;
                }
            }

            if (!string.IsNullOrEmpty(_chkListFileName))
            {
                time.Enabled = false; 
                btnOpen.PerformClick();
                _chkListFileName = null;
                time.Enabled = true;
            }

            if (Gui.control != null)
            {
                txtName.Text = Gui.control.Name;
                txtLocation.Text = Gui.control.Location.X.ToString() + "," + Gui.control.Location.Y.ToString();
                txtSize.Text = Gui.control.Size.Width.ToString() + "," + Gui.control.Size.Height.ToString();
                txtText.Text = Gui.control.Text;

                if (Gui.control.Name.Contains("Label"))
                {
                    _comboSet = false;
                    _listSet = false;
                    lblMulti.Visible = true;
                    lblMulti.Text = "Font Size:";
                    lblMulti.Left = 26;
                    txtBoxMulti.Visible = true;
                    txtBoxMulti.Multiline = false;
                    btnComboUpdate.Visible = false;
                    btnBrowse.Visible = false;
                    txtBoxMulti.Text = Gui.control.Font.Size.ToString();
                }

                if (Gui.control.Name.Contains("PictureBox"))
                {
                    _comboSet = false;
                    _listSet = false;
                    lblMulti.Visible = true;
                    lblMulti.Text = "Image Source:";
                    lblMulti.Left = 5;
                    txtBoxMulti.Visible = true;
                    txtBoxMulti.Multiline = false;
                    btnComboUpdate.Visible = false;
                    btnBrowse.Visible = true;
                    btnBrowse.Top = 190;
                    if (Gui.control.AccessibleName != null)
                        txtBoxMulti.Text = Gui.control.AccessibleName;
                    else
                        txtBoxMulti.Text = "c:\\";
                }

                if (Gui.control.Name.Contains("ComboBox"))
                {
                    _listSet = false;
                    lblMulti.Visible = true;
                    lblMulti.Text = "Item List:";
                    lblMulti.Left = 20;
                    txtBoxMulti.Height = 170;
                    txtBoxMulti.Visible = true;
                    txtBoxMulti.Multiline = true;
                    btnComboUpdate.Visible = true;
                    btnBrowse.Visible = false;

                    if (!_comboSet)
                    {
                        txtBoxMulti.Text = "";
                        var clone = new ComboBox();
                        clone = (ComboBox) Gui.control;

                        foreach (var entry in clone.Items)
                        {
                            txtBoxMulti.Text += entry + "\r\n";
                        }
                        _comboSet = true;
                    }
                }

                if (Gui.control.Name.Contains("ListBox"))
                {
                    _comboSet = false;
                    lblMulti.Visible = true;
                    lblMulti.Text = "Item List:";
                    lblMulti.Left = 20;
                    txtBoxMulti.Height = 170;
                    txtBoxMulti.Visible = true;
                    txtBoxMulti.Multiline = true;
                    btnComboUpdate.Visible = true;
                    btnBrowse.Visible = false;

                    if (!_listSet)
                    {
                        txtBoxMulti.Text = "";
                        var clone = new ListBox();
                        clone = (ListBox)Gui.control;

                        foreach (var entry in clone.Items)
                        {
                            txtBoxMulti.Text += entry + "\r\n";
                        }
                        _listSet = true;
                    }
                }
                if (!Gui.control.Name.Contains("ComboBox") && !Gui.control.Name.Contains("Picture") &&
                    !Gui.control.Name.Contains("Label") && !Gui.control.Name.Contains("ListBox"))
                {
                    lblMulti.Visible = false;
                    txtBoxMulti.Visible = false;
                    btnComboUpdate.Visible = false;
                    btnBrowse.Visible = false;
                    _comboSet = false;
                    _listSet = false;
                }
            }
            else
            {
                txtName.Text = "";
                txtLocation.Text = "";
                txtSize.Text = "";
                txtText.Text = "";
                lblMulti.Visible = false;
                txtBoxMulti.Visible = false;
                btnComboUpdate.Visible = false;
                btnBrowse.Visible = false;
                _comboSet = false;
                _listSet = false;
            }
        }

        private void PicLabelClick(object sender, EventArgs e)
        {
            Gui.LabelAdded(true);
            Gui.l_close = false;
            Gui.l_atleastonce = true;
        }
        
        private void PicCheckBoxClick(object sender, EventArgs e)
        {
            Gui.CheckBoxAdded(true);
            Gui.c_close = false;
            Gui.c_atleastonce = true;
        }

        private void PicComboBoxClick(object sender, EventArgs e)
        {
            Gui.ComboBoxAdded(true);
            Gui.co_close = false;
            Gui.co_atleastonce = true;
        }

        private void PicListBoxClick(object sender, EventArgs e)
        {
            Gui.ListBoxAdded(true);
            Gui.lbx_close = false;
            Gui.lbx_atleastonce = true;
        }

        private void PicPictureBoxClick(object sender, EventArgs e)
        {
            Gui.PictureBoxAdded(true);
            Gui.pic_close = false;
            Gui.pic_atleastonce = true;
        }

        private void PicRadioButtonClick(object sender, EventArgs e)
        {
            Gui.RadioButtonAdded(true);
            Gui.rad_close = false;
            Gui.rad_atleastonce = true;
        }

        private void PicTextBoxClick(object sender, EventArgs e)
        {
            Gui.TextBoxAdded(true);
            Gui.txt_close = false;
            Gui.txt_atleastonce = true;
        }

        private void BtnSaveClick(object sender, EventArgs e)
        {
            foreach (var entry in Gui.ControlList)
            {
                if (entry.Name.Contains("PictureBox"))
                {
                    if (string.IsNullOrEmpty(entry.AccessibleName) || entry.AccessibleName == "Default")
                    {
                        MessageBox.Show("You must provide a valid path to a graphic file for the PictureBox '" + entry.Name + "'.", "Checklist Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            string WindowSize = Size.ToString();
            WindowSize = WindowSize.Replace("{Width=", "");
            WindowSize = WindowSize.Replace(" Height=", "");
            WindowSize = WindowSize.Replace("}", "");
            
            var success = Gui.XMLBuild(_chkListPath, WindowSize);

            if (success)
            {
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

                _changed = false;
            }
        }

        private void BtnOpenClick(object sender, EventArgs e)
        {
            XMLRead();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            if (_changed)
            {
                var dr = MessageBox.Show("The Checklist edits you have made will be lost if you don't save.\r\nAre you sure you want to cancel?", "Unsaved Checklist Edits", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    Gui.Reset();
                    Dispose(true);

                    txtName.Text = "";
                    txtLocation.Text = "1,1";
                    txtSize.Text = "1,1";
                    txtText.Text = "";
                    _changed = false;
                    Close();
                }
            }
            else
            {
                Gui.Reset();
                Dispose(true);

                txtName.Text = "";
                txtLocation.Text = "1,1";
                txtSize.Text = "1,1";
                txtText.Text = "";
                _changed = false;
                Close();
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_changed)
                {
                    var dr = MessageBox.Show("The Checklist edits you have made will be lost if you don't save.\r\nAre you sure you want to cancel?","Unsaved Checklist Edits", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        Gui.Reset();
                        Dispose(true);

                        txtName.Text = "";
                        txtLocation.Text = "1,1";
                        txtSize.Text = "1,1";
                        txtText.Text = "";
                    }
                    else
                        e.Cancel = true;
                }
            }
            catch (Exception fail)
            { }
        }

        private void TxtTextTextChanged(object sender, EventArgs e)
        {
            if (Gui.control != null)
            {
               Gui.control.Text = txtText.Text; 
               _changed = true;
            }
        }

        private void TxtSizeTextChanged(object sender, EventArgs e)
        {
            int x = 1, y = 1;
            var pos = txtSize.Text.IndexOf(",");

            try
            {
                int.TryParse(txtSize.Text.Substring(0, pos), out x);
                int.TryParse(txtSize.Text.Substring(pos + 1), out y);
            }
            catch (Exception)
            {
                if (x == 0) x = 1;
                if (y == 0) y = 1;
            }

            if (Gui.control != null)
            {
                Gui.control.Size = new Size(x, y);
                _changed = true;
            }
        }

        private void TxtLocationTextChanged(object sender, EventArgs e)
        {
            int x = 1, y = 1;
            var pos = txtLocation.Text.IndexOf(",");

            try
            {
                int.TryParse(txtLocation.Text.Substring(0, pos), out x);
                int.TryParse(txtLocation.Text.Substring(pos + 1), out y);
            }
            catch (Exception)
            {
                if (x == 0) x = 1;
                if (y == 0) y = 1;
            }

            if (Gui.control != null)
            {
                Gui.control.Location = new Point(x, y);
                _changed = true;
            }
        }

        private void TxtNameTextChanged(object sender, EventArgs e)
        {
            if (Gui.control != null)
            {
                Gui.control.Name = txtName.Text;
                _changed = true;
            }    
        }

        private void TxtBoxMultiTextChanged(object sender, EventArgs e)
        {
            if (Gui.control.Name.Contains("Label"))
            {
                float size = 8.25F;
                float.TryParse(txtBoxMulti.Text, out size);
                if (size < 1) size = 8.25F;
                Gui.control.Font = new Font(Gui.control.Font.FontFamily, size);
                _changed = true;
            }
        }

        private void BtnComboUpdateClick(object sender, EventArgs e)
        {
            if (Gui.control.Name.Contains("ComboBox"))
            {
                var clone = new ComboBox();
                clone = (ComboBox) Gui.control;
                clone.Items.Clear();

                string txt = txtBoxMulti.Text;
                string[] list = txt.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var entry in list)
                {
                    clone.Items.Add(entry);
                }

                Gui.control = clone;
                Gui.control.Text = clone.Items[0].ToString();
                _changed = true;
            }

            if (Gui.control.Name.Contains("ListBox"))
            {
                var clone = new ListBox();
                clone = (ListBox)Gui.control;
                clone.Items.Clear();

                string txt = txtBoxMulti.Text;
                string[] list = txt.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var entry in list)
                {
                    clone.Items.Add(entry);
                }

                Gui.control = clone;
                Gui.control.Text = clone.Items[0].ToString();
            }        
        }

        private void BtnBrowseClick(object sender, EventArgs e)
        {
            var graphic = new OpenFileDialog();
            graphic.InitialDirectory = txtBoxMulti.Text;
            graphic.Multiselect = false;
            graphic.AddExtension = true;
            graphic.CheckPathExists = true;
            graphic.Title = "Select a graphic to display";
            graphic.Filter = "Graphics Files|*.jpg;*.jpeg;*.png;*.bmp";

            var dr = graphic.ShowDialog();
            if (dr == DialogResult.OK)
            {
                var clone = new PictureBox();
                clone = (PictureBox) Gui.control;
                clone.Image = new Bitmap(graphic.FileName);
                clone.AccessibleName = graphic.FileName;
                Gui.control = clone;
                txtBoxMulti.Text = graphic.FileName;
                _changed = true;
            }
        }

        private void XMLRead()
        {
            var picture = false;
            var ocl = new OpenFileDialog();
            ocl.DefaultExt = "xml";
            ocl.Filter = "XML Files|*.xml";
            ocl.CheckFileExists = true;
            ocl.Multiselect = false;

            if (string.IsNullOrEmpty(_chkListFileName))
            {
                var dr = ocl.ShowDialog();
                if (dr == DialogResult.Cancel)
                    return;
            }
            else
                ocl.FileName = _chkListFileName;

            _chkListFileName = null;

            var param = new List<string>();
            var PicParam = new List<string>();
            var xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreWhitespace = true;
            Gui.Reset();
            checkListPanel.Controls.Clear();
            
            try
            {
                using (XmlReader reader = XmlReader.Create(ocl.FileName, xmlReaderSettings))
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
                                Gui.Size = reader["Size"];
                                Size = GetWindowSize(Gui.Size);

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
                _changed = false;
            }
            catch (Exception crap)
            {
                if (picture)
                    MessageBox.Show("An expected picture or graphic described in your Checklist cannot be found.\r\n\r\n" + PicParam[4], "Checklist XML Read Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show("This doesn't appear to be a valid CaseNotes Checklist file.", "Checklist XML Read Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private Size GetWindowSize(string GuiSize)
        {
            string[] sizeParts = GuiSize.Split(',');
            int height = int.Parse(sizeParts[0]);
            int width = int.Parse(sizeParts[1]);
            Size mySize = new Size(height, width);

            return mySize;
        }

        private void AddLabel(List<String> param)
        {
            var lbl = new XLabel(Gui);
            lbl.Parent = checkListPanel;
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
            lbl.Visible = true;

            lbl.Text = param[3];

            float size = 8.25F;
            float.TryParse(param[4], out size);
            if (size < 1) size = 8.25F;
            lbl.Font = new Font(lbl.Font.FontFamily, size);  

            Gui.ControlList.Add(lbl);
            Gui.l_close = true;
            _changed = true;
        }

        private void AddCombo(List<String> param)
        {
            var count = 0;
            var cbo = new XComboBox(Gui, false);
            cbo.Parent = checkListPanel;
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

            for (int i = 5; i < count+5; i++)
            {
                cbo.Items.Add(param[i]);
            }

            Gui.ControlList.Add(cbo);
            Gui.co_close = true;
            _changed = true;
        }

        private void AddList(List<String> param)
        {
            var count = 0;
            var lbx = new XListBox(Gui, false);
            lbx.Parent = checkListPanel;
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
            lbx.AutoSize = true;

            lbx.Text = param[3];
            int.TryParse(param[4], out count);

            for (int i = 5; i < count + 5; i++)
            {
                lbx.Items.Add(param[i]);
            }

            Gui.ControlList.Add(lbx);
            Gui.lbx_close = true;
            _changed = true;
        }

        private void AddTextBox(List<String> param)
        {
            var txb = new XTextBox(Gui);
            txb.Parent = checkListPanel;
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

            Gui.ControlList.Add(txb);
            Gui.txt_close = true;
            _changed = true;
        }

        private void AddPicture(List<String> param)
        {
            var pic = new XPictureBox(Gui);
            pic.Parent = checkListPanel;
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

            Gui.ControlList.Add(pic);
            Gui.pic_close = true;
            _changed = true;
        }

        private void AddRadio(List<String> param)
        {
            var rdb = new XRadioButton(Gui);
            rdb.Parent = checkListPanel;
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

            Gui.ControlList.Add(rdb);
            Gui.rad_close = true;
            _changed = true;
        }

        private void AddCheck(List<String> param)
        {
            var cbx = new XCheckBox(Gui);
            cbx.Parent = checkListPanel;
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

            Gui.ControlList.Add(cbx);
            Gui.c_close = true;
            _changed = true;
        }

        private void CheckListPanelClick(object sender, EventArgs e)
        {
            Gui.SelectedControl = null;
            Gui.control = null;
            txtName.Text = "";
            txtLocation.Text = "";
            txtSize.Text = "";
            txtText.Text = "";
            lblMulti.Visible = false;
            txtBoxMulti.Visible = false;
            btnComboUpdate.Visible = false;
            btnBrowse.Visible = false;
            _comboSet = false;
            _listSet = false;
        }

    }
}
