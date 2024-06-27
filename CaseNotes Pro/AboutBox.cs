using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            labelVersion.Text = "Version: " + AssemblyVersion;
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = "Author: " + AssemblyCompany;
            Text = "About: " + AssemblyTitle;
            labelProductName.Text = AssemblyProduct;
            labelLatestVersion.Text = "Latest Version:";
            textBoxDescription.Text = "A free professional tool for securely recording contemporaneous notes in forensic data investigations and incident response work." +
                                      " CaseNotes is an invaluable tool for recording notes when dealing with systems compromises and server breaches.";
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }

                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        
        #endregion

        private void OkButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpdatesClick(object sender, EventArgs e)
        {
            try
            {
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
        }

        private void lnklblCaseNotesLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://first-response.co.uk/CaseNotes");
        }

    }
}
