using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace FirstResponse.CaseNotes
{
    public class GuiController
    {
        public Control control;
        public List<Control> Controller = new List<Control>();
        public List<Control> ControlList = new List<Control>();
        public Control SelectedControl;

        public string Name;
        public string Description;
        public string Author;
        public string Created;
        public string Modified;
        public string Version;
        public string Size;

        //Button
        public bool b_varpub;
        public int a;
        public bool b_close;
        public bool b_atleastonce;

        //CheckBox
        public bool c_varpub;
        public int c;
        public bool c_close;
        public bool c_atleastonce;

        //Combobox
        public bool co_varpub;
        public int co;
        public bool co_close;
        public bool co_atleastonce;

        //Label
        public bool l_varpub;
        public int l;
        public bool l_close;
        public bool l_atleastonce;

        //Listbox
        public bool lbx_varpub;
        public int lbx;
        public bool lbx_close;
        public bool lbx_atleastonce;

        //PictureBox
        public bool pic_varpub;
        public int pic;
        public bool pic_close;
        public bool pic_atleastonce;

        //RadioButton
        public bool rad_varpub;
        public int rad;
        public bool rad_close;
        public bool rad_atleastonce;

        //Textbox
        public bool txt_varpub;
        public int txt;
        public bool txt_close;
        public bool txt_atleastonce;

        //Button
        public void ButtonAdded(bool action)
        {
            b_varpub = action;
            b_close = true;
            ButtonAddNum();
        }

        public int ButtonAddNum()
        {
            a++;
            return a;
        }

        //CheckBox
        public void CheckBoxAdded(bool action)
        {
            c_varpub = action;
            c_close = true;
            CheckBoxAddNum();
        }

        public int CheckBoxAddNum()
        {
            c++;
            return c;
        }

        //Combobox
        public void ComboBoxAdded(bool action)
        {
            co_varpub = action;
            co_close = true;
            ComboBoxAddNum();
        }

        public int ComboBoxAddNum()
        {
            co++;
            return co;
        }

        //Label
        public void LabelAdded(bool action)
        {
            l_varpub = action;
            l_close = true;
            LabelAddNum();
        }

        public int LabelAddNum()
        {
            l++;
            return l;
        }

        //Listbox
        public void ListBoxAdded(bool action)
        {
            lbx_varpub = action;
            lbx_close = true;
            ListboxAddNum();
        }

        public int ListboxAddNum()
        {
            lbx++;
            return lbx;
        }

        //Picturebox
        public void PictureBoxAdded(bool action)
        {
            pic_varpub = action;
            pic_close = true;
            PictureboxAddNum();
        }

        public int PictureboxAddNum()
        {
            pic++;
            return pic;
        }

        //RadioButton
        public void RadioButtonAdded(bool action)
        {
            rad_varpub = action;
            rad_close = true;
            RadiobuttonAddNum();
        }

        public int RadiobuttonAddNum()
        {
            rad++;
            return rad;
        }

        //Textbox
        public void TextBoxAdded(bool action)
        {
            txt_varpub = action;
            txt_close = true;
            TextboxAddNum();
        }

        public int TextboxAddNum()
        {
            txt++;
            return txt;
        }

        public void CreateUniqueList(List<Control> oDup, ref List<Control> oUniq)
        {
            for (int x = 0; x < oDup.Count; x++)
            {
                var bDuplicate = false;
                foreach (object oItem in oUniq)
                {
                    if (oItem == oDup[x])
                    {
                        bDuplicate = true;
                        oDup.RemoveAt(x);
                        break;
                    }
                }
                if (!bDuplicate)
                    oUniq.Add(oDup[x]);
            }
        }

        public bool XMLBuild(string location, string size)
        {
            if (ControlList.Count == 0)
            {
                MessageBox.Show("Your Checklist is empty or there are no changes to save.", "Save Checklist",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            try
            {
                var meta = new CheckListMetadata();
                if (!string.IsNullOrEmpty(Name))
                    meta.CheckName = Name;

                if (!string.IsNullOrEmpty(Description))
                    meta.Description = Description;

                if (!string.IsNullOrEmpty(Author))
                    meta.Author = Author;

                if (!string.IsNullOrEmpty(Created))
                    meta.DateCreated = Created;
                else
                    meta.DateCreated = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();

                meta.DateModified = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();

                if (!string.IsNullOrEmpty(size))
                    meta.Size = size;
                else
                    meta.Size = "1290,785";

                if (!string.IsNullOrEmpty(Version))
                {
                    float temp = 0;
                    float.TryParse(Version, out temp);
                    temp += 0.1F;
                    meta.Version = temp.ToString();
                }
                else
                    meta.Version = "1.0";

                var dr = meta.ShowDialog();
                if (dr == DialogResult.Cancel)
                    return false;

                var scl = new SaveFileDialog();
                scl.DefaultExt = "xml";
                scl.AddExtension = true;
                scl.OverwritePrompt = true;
                scl.Filter = "XML Files|*.xml";
                scl.Title = "Save CaseNotes Checklist";
                scl.FileName = meta.CheckName;
                scl.InitialDirectory = location;

                dr = scl.ShowDialog();
                if (dr == DialogResult.Cancel)
                    return false;

                Name = meta.CheckName;
                Description = meta.Description;
                Author = meta.Author;
                Version = meta.Version;
                Modified = meta.DateModified;
                Created = meta.DateCreated;
                Size = meta.Size;

                var fs = new FileStream(scl.FileName, FileMode.Create);
                var xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.NewLineOnAttributes = true;
                xmlWriterSettings.Indent = true;
                XmlWriter wxml = XmlWriter.Create(fs, xmlWriterSettings);

                wxml.WriteStartDocument();
                wxml.WriteStartElement("Checklist");
                wxml.WriteAttributeString("Name", meta.CheckName);
                wxml.WriteAttributeString("Description", meta.Description);
                wxml.WriteAttributeString("Author", meta.Author);
                wxml.WriteAttributeString("Created", meta.DateCreated);
                wxml.WriteAttributeString("Modified", meta.DateModified);
                wxml.WriteAttributeString("Version", meta.Version);
                wxml.WriteAttributeString("Size", meta.Size);

                foreach (Control v in ControlList)
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
                fs.Close();
            }

            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
            }

            return true;
        }        

        public void Reset()
        {
            control = null;
            Controller.Clear();
            ControlList.Clear();
            SelectedControl = null;

            Name = null;
            Description = null;
            Author = null;
            Created = null;
            Modified = null;
            Version = null;

            b_varpub = false;
            a = 0;
            b_close = false;
            b_atleastonce = false;

            c_varpub = false;
            c = 0;
            c_close = false;
            c_atleastonce = false;

            co_varpub = false;
            co = 0;
            co_close = false;
            co_atleastonce = false;

            l_varpub = false;
            l = 0;
            l_close = false;
            l_atleastonce = false;

            lbx_varpub = false;
            lbx = 0;
            lbx_close = false;
            lbx_atleastonce = false;

            pic_varpub = false;
            pic = 0;
            pic_close = false;
            pic_atleastonce = false;

            rad_varpub = false;
            rad = 0;
            rad_close = false;
            rad_atleastonce = false;

            txt_varpub = false;
            txt = 0;
            txt_close = false;
            txt_atleastonce = false;            
        }
    }
}
