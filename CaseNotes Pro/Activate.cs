using System;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace FirstResponse.CaseNotes
{
    public partial class Activate : Form
    {
        public bool activated;

        public Activate()
        {
            InitializeComponent();
            GenerateRequestCode(); 
            RegistryCheck();
            //txtActivation.Text = Valid();
        }

        private void GenerateRequestCode()
        {
            var request = "";
            var processorID = "";
	        var sQuery = "SELECT ProcessorId FROM Win32_Processor";
	        var oManagementObjectSearcher = new ManagementObjectSearcher(sQuery);
	        var oCollection = oManagementObjectSearcher.Get();
	        foreach (var oManagementObject in oCollection)
	        {
                processorID = (string)oManagementObject["ProcessorId"];
	        }

            var macAddresses = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(processorID) && !string.IsNullOrEmpty(macAddresses))
            {
                for (int i = 0; i < 11; i+=2)
                {            
                    request += processorID.Substring(i, 2);
                    request += macAddresses.Substring(i, 2);
                }

                var request1 = "";
                for (int i = 0; i < 23 ; i+=5)
                {
                    if (i != 20)
                        request1 += request.Substring(i, 5) + '-';
                    else
                        request1 += request.Substring(i, 4);
                }
                request = request1;
            }

            char[] ary = request.ToCharArray();
            Array.Reverse(ary);
            var revRequest = new string(ary);

            txtRequest.Text = "C" + revRequest;
            txtActivation.Text = "";
        }

        private void BtnActivateClick(object sender, EventArgs e)
        {
            var activation = txtActivation.Text;  // BC20-9100A-0FF00-BF06B-E8CFB
            if (string.IsNullOrEmpty(activation))
            {
                MessageBox.Show("You must enter the validation code sent to you.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Valid() == activation)
            {
                RegistryUpdate();
                MessageBox.Show("Success! Your copy of CaseNotes Professional has been activated.", "Activation successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Opps! The activation string you entered isn't valid for this computer.\r\nYour copy of CaseNotes Professional has not been activated.", "Activation failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);                
            }
        }

        private string Valid()
        {
            var key = "B0FFA-D4C7E-C2A3F-A9B1B-E3835";
            var request = txtRequest.Text;

            var block1 = xor(request.Substring(0, 5), key.Substring(0, 5));
            var block2 = xor(request.Substring(6, 5), key.Substring(6, 5));
            var block3 = xor(request.Substring(12, 5), key.Substring(12, 5));
            var block4 = xor(request.Substring(18, 5), key.Substring(18, 5));
            var block5 = xor(request.Substring(24, 5), key.Substring(24, 5));

            var validation2 = block1 + block2 + block3 + block4 + block5;
            validation2 = Functions.GetMd5(validation2);

            var validation = "";
            for (int i = 0; i < 23; i += 5)
            {
                if (i != 20)
                    validation += validation2.Substring(i, 5) + '-';
                else
                    validation += validation2.Substring(i, 5);
            }

            return validation;
        }

        string xor(string text, string key)
        {
            var result = new StringBuilder();

            for (int c = 0; c < text.Length; c++)
                result.Append((char)((uint)text[c] ^ (uint)key[c % key.Length]));

            return result.ToString();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void RegistryCheck()
        {
            string temp = Application.UserAppDataRegistry.Name;
            temp = temp.Substring(18);
            temp = temp.Substring(0, temp.LastIndexOf("\\")) + "\\PersistantHandler";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(temp, true);
            
            try
            {
                var stored = key.GetValue("TraceBack").ToString().Replace('$', '-');

                if (stored != null)
                {
                    if (Valid() == (string)stored)
                       activated = true;
                }
            }
            catch (Exception fail)
            {
            }

        }

        private void RegistryUpdate()
        {
            string temp = Application.UserAppDataRegistry.Name;
            temp = temp.Substring(18);
            temp = temp.Substring(0, temp.LastIndexOf("\\")) + "\\PersistantHandler";

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(temp, true);

                if (key.GetValue("TraceBack") != null)
                    key.DeleteValue("TraceBack");
            }
            catch (Exception fail)
            {
            }

            var traceBack = Valid().Replace('-', '$');
            RegistryKey newKey = Registry.CurrentUser.CreateSubKey(temp);
            newKey.SetValue("TraceBack", traceBack, RegistryValueKind.String);
            newKey.Close();
            activated = true;
        }
    }
}
