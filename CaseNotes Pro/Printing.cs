using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes
{
    public partial class Printing : Form
    {
        private TabControl _caseTabs = new TabControl();
        public List<string> Tabs = new List<string>();
        private List<CheckBox> _checkBoxs = new List<CheckBox>();

        public Printing(TabControl CaseTabs)
        {
            InitializeComponent();
            _caseTabs = CaseTabs;
            InitGUI();
        }

        private void InitGUI()
        {
            var checkBoxs = new List<CheckBox>
                                {
                                    chkDummy,
                                    chkTab1,
                                    chkTab2,
                                    chkTab3,
                                    chkTab4,
                                    chkTab5,
                                    chkTab6,
                                    chkTab7,
                                    chkTab8,
                                    chkTab9,
                                    chkTab10
                                };

            for (int i = 1; i <= _caseTabs.TabCount-2; i++)
            {
                checkBoxs[i].Text = _caseTabs.TabPages[i].Name;
                if (checkBoxs[i].Text == "Checklist")
                    checkBoxs[i].Text = "Case Checklist";
                checkBoxs[i].Visible = true;
            }
            _checkBoxs = checkBoxs;
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnPrintClick(object sender, EventArgs e)
        {
            if (chkOverview.Checked)
                Tabs.Add("Overview");

            foreach (var chekbox in _checkBoxs)
            {
                if (chekbox.Checked && chekbox.Text == "Case Checklist")
                    Tabs.Add("Checklist");
                else if (chekbox.Checked)
                    Tabs.Add(chekbox.Text);
            }

            if (chkAuditTab.Checked)
                Tabs.Add("AuditRTF");
        }
    }
}
