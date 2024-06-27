using System;
using System.Globalization;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes
{
    public partial class FindDialog : Form
    {
        private string _text = "";
        static private bool _caseSensitive = false;
        private RichTextBox _txtControl;
        static private int _currentIndex = 0;
        static public string CurrentSearchString = "";

        public string FindText { get; set; }
        public bool CaseSensitive { get; set; }
        
        public FindDialog()
        {
            InitializeComponent();
        }

        public FindDialog(RichTextBox txtControl)
        {
            InitializeComponent();
            txtFind.Focus();
            _txtControl = txtControl;
            txtFind.Text = _txtControl.Text.Substring(_txtControl.SelectionStart, _txtControl.SelectionLength);
        }

        public bool FindNext(RichTextBox txtControl)
        {
            return FindNext(CurrentSearchString, _caseSensitive, txtControl);
        }

        public bool FindNext(string searchString, bool caseSensitive, RichTextBox txtControl)
        {
            txtControl.ScrollToCaret();
            if (_currentIndex > 0)
            {
                txtControl.SelectionStart = _currentIndex - 1;
                txtControl.SelectionLength = searchString.Length;
            }

            txtControl.SelectionBackColor = System.Drawing.Color.Transparent;

            CurrentSearchString = searchString;
            _currentIndex = txtControl.SelectionStart + 1;
            if (caseSensitive)
                _currentIndex = txtControl.Text.IndexOf(searchString, _currentIndex);
            else
            {
                var culture = new CultureInfo("");
                _currentIndex = culture.CompareInfo.IndexOf(txtControl.Text, searchString, _currentIndex, CompareOptions.IgnoreCase);
            }

            if (_currentIndex >= 0)
            {
                txtControl.SelectionStart = txtControl.Text.IndexOf("\n", _currentIndex) + 2;
                txtControl.SelectionLength = 0;
                txtControl.ScrollToCaret(); 
                txtControl.SelectionStart = _currentIndex;
                txtControl.SelectionLength = searchString.Length;
                txtControl.SelectionBackColor = System.Drawing.Color.Cyan;
                txtControl.SelectionLength = 0;
                _currentIndex++;
                txtControl.ScrollToCaret();
            }
            else
            {
                Hide();
                MessageBox.Show("End of the document reached.", "CaseNotes Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _currentIndex = 0;
                txtControl.SelectionLength = 0;
                return false;
            }
            return true;
        }
        
        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnFindClick(object sender, EventArgs e)
        {
            _text = txtFind.Text;
            _caseSensitive = chkCaseSensitive.Checked;
            _txtControl.Focus();
            FindNext(_text, _caseSensitive, _txtControl);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _txtControl.ScrollToCaret();
                if (_currentIndex > 0)
                {
                    _txtControl.SelectionStart = _currentIndex - 1;
                    _txtControl.SelectionLength = CurrentSearchString.Length;
                }

                _txtControl.SelectionBackColor = System.Drawing.Color.Transparent;
            }
            catch (Exception fail)
            { }
        }
    }
}
