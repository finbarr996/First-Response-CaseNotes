using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes
{
    public class Preferences
    {
        public Point AppLocation { get; set; }
        public Size AppSize { get; set; }
        public Point NoteLocation { get; set; }
        public Size NoteSize { get; set; }
        public bool SaveBackups { get; set; }
        public int MetaDataItems { get; set; }
        public int TabItems { get; set; }
        public string DefaultLocation { get; set; }
        public string ChecklistLocation { get; set; }
        public string TemplateLocation { get; set; }
        public string SysDefaultLocation { get; set; }
        public string SysChecklistLocation { get; set; }
        public string SysTemplateLocation { get; set; }
        public string MetadataTypeface { get; set; }
        public int MetadataTypeSize { get; set; }
        public string NoteTypeface { get; set; }
        public int NoteTypeSize { get; set; }
        public string DateLastUpdated { get; set; }
        public string MainTitle { get; set; }
        public string CustomColors { get; set; }
        public string CaseStatus { get; set; }
        
        public List<string> MetaDescArray { get; set; }
        public List<string> MetaValueArray { get; set; }
        public List<string> TabsValueArray { get; set; }

        public bool ReadLocation(string caseNotesDB, string pass)
        {
            var prefs = new SQLiteDatabase();
            if (string.IsNullOrEmpty(pass))
                prefs.DBConnection = "Data Source=" + caseNotesDB;
            else
                prefs.DBConnection = "Data Source=" + caseNotesDB + ";Password=" + pass;

            if (!File.Exists(caseNotesDB))
                prefs.InitialisePrefs(caseNotesDB);

            var dt = prefs.GetDataTable("select * from Location;");

            foreach (DataRow dr in dt.Rows)
            {
                AppLocation = Functions.String2Point(dr.ItemArray[1].ToString());
                AppSize = (Size)Functions.String2Point(dr.ItemArray[2].ToString());
                NoteLocation = Functions.String2Point(dr.ItemArray[3].ToString());
                NoteSize = (Size)Functions.String2Point(dr.ItemArray[4].ToString());
            }

           return (dt.Rows.Count > 0);
        }

        public void UpdateCaseFileLocations(string caseNotesDB, string pass)
        {
            var prefs = new SQLiteDatabase();
            if (string.IsNullOrEmpty(pass))
                prefs.DBConnection = "Data Source=" + caseNotesDB;
            else
                prefs.DBConnection = "Data Source=" + caseNotesDB + ";Password=" + pass;

            var data = new Dictionary<string, string>();
            data.Add("DefaultLocation", DefaultLocation);
            data.Add("ChecklistLocation", ChecklistLocation);
            data.Add("TemplateLocation", TemplateLocation);

            try
            {
                var result = prefs.Update("SysPrefs", data, "1=1");
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
            }
        }
        
        public bool ReadSystemPrefs(string caseNotesDB, string pass, string action)
        {
            var fi = new FileInfo(caseNotesDB);
            var prefs = new SQLiteDatabase();

            var CaseOK = Functions.ValidateCaseFile(caseNotesDB, pass);
            var dbVer = Functions.ConfigureDatabase(caseNotesDB, pass);
            if (CaseOK != "success" && dbVer != "")
            {
                MessageBox.Show("There is a problem with your preferences file: " + caseNotesDB + "\r\n" + CaseOK + "\r\n\r\nPlease rectify this error and try again.", "Preferences File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            
            if (string.IsNullOrEmpty(pass))
                prefs.DBConnection = "Data Source=" + caseNotesDB;
            else
                prefs.DBConnection = "Data Source=" + caseNotesDB + ";Password=" + pass;

            if (fi.Length < 100 || fi.IsReadOnly)
                return false;
            
            if (!File.Exists(caseNotesDB) && action == "new")
                prefs.InitialisePrefs(caseNotesDB);
            
            var dt = prefs.GetDataTable("select * from SysPrefs;");
            int metaDataItems = 0;
            int tabItems = 0;
            int metadataTypeSize = 0;
            int noteTypeSize = 0;

            if(String.IsNullOrEmpty(SysDefaultLocation))
                SysDefaultLocation = Application.UserAppDataPath;

            if (String.IsNullOrEmpty(SysChecklistLocation))
                SysChecklistLocation = Application.UserAppDataPath;

            if (String.IsNullOrEmpty(SysTemplateLocation))
                SysTemplateLocation = Application.UserAppDataPath;

            foreach (DataRow dr in dt.Rows)
            {
                int.TryParse(dr.ItemArray[1].ToString(), out metaDataItems);
                int.TryParse(dr.ItemArray[2].ToString(), out tabItems);
                DefaultLocation = dr.ItemArray[3].ToString();
                ChecklistLocation = dr.ItemArray[4].ToString();
                TemplateLocation = dr.ItemArray[5].ToString(); 
                MetadataTypeface = dr.ItemArray[6].ToString();
                int.TryParse(dr.ItemArray[7].ToString(), out metadataTypeSize);
                NoteTypeface = dr.ItemArray[8].ToString();
                int.TryParse(dr.ItemArray[9].ToString(), out noteTypeSize);
                DateLastUpdated = dr.ItemArray[10].ToString();
                MainTitle = dr.ItemArray[11].ToString();
                CustomColors = dr.ItemArray[12].ToString();
                if (dr.ItemArray[13].ToString() != "")
                    CaseStatus = dr.ItemArray[13].ToString();
                else CaseStatus = "Initiated#In Progress#Pending#Paused#Under Review#Closed#Archive";
            }
            MetaDataItems = metaDataItems;
            TabItems = tabItems;
            MetadataTypeSize = metadataTypeSize;
            NoteTypeSize = noteTypeSize;
            var locationsOK = true;

            if (Directory.Exists(DefaultLocation))
                SysDefaultLocation = DefaultLocation;
            else
            {
                if (Directory.Exists(SysDefaultLocation))
                {
                    MessageBox.Show("Your CaseFile Location is invalid:\r\n" + DefaultLocation + "\r\n\r\nSo using your default case location instead:\r\n" + SysDefaultLocation, "Problem with Case Defaults", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                    DefaultLocation = SysDefaultLocation;
                    locationsOK = false;
                }
            }

            if (Directory.Exists(ChecklistLocation))
                SysChecklistLocation = ChecklistLocation;
            else
            {
                if (Directory.Exists(SysChecklistLocation))
                {
                    MessageBox.Show("Your CaseFile Checklist Location is invalid:\r\n" + ChecklistLocation + "\r\n\r\nSo using your default checklist location instead:\r\n" + SysChecklistLocation, "Problem with Case Defaults", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ChecklistLocation = SysChecklistLocation;
                    locationsOK = false;
                }
            }

            if (Directory.Exists(TemplateLocation))
                SysTemplateLocation = TemplateLocation;
            else
            {
                if (Directory.Exists(SysTemplateLocation))
                {
                    MessageBox.Show("Your CaseFile Template Location is invalid:\r\n" + TemplateLocation + "\r\n\r\nSo using your default template location instead:\r\n" + SysTemplateLocation, "Problem with Case Defaults", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TemplateLocation = SysTemplateLocation;
                    locationsOK = false;
                }
            }

            if (!locationsOK) UpdateCaseFileLocations(caseNotesDB, pass);

            return (dt.Rows.Count > 0);
        }
        
        public bool ReadUserPrefs(string caseNotesDB, string pass)
        {
            var metaDescArray = new List<string>();
            var metaValueArray = new List<string>();
            var tabsValueArray = new List<string>();
            var prefs = new SQLiteDatabase();
            if (string.IsNullOrEmpty(pass))
                prefs.DBConnection = "Data Source=" + caseNotesDB;
            else
                prefs.DBConnection = "Data Source=" + caseNotesDB + ";Password=" + pass;

            if (!File.Exists(caseNotesDB))
                prefs.InitialisePrefs(caseNotesDB);

            var dt = prefs.GetDataTable("select * from UserPrefs;");

            foreach (DataRow dr in dt.Rows)
            {
                if (dr[1].ToString() == "metaDesc")
                    metaDescArray.Add(dr[2].ToString());

                if (dr[1].ToString() == "metaValue")
                    metaValueArray.Add(dr[2].ToString());

                if (dr[1].ToString() == "tabsDesc")
                    tabsValueArray.Add(dr[2].ToString());
            }

            MetaDescArray = metaDescArray;
            MetaValueArray = metaValueArray;
            TabsValueArray = tabsValueArray;

            return (dt.Rows.Count > 0);
        }

        public bool WriteMainLocation(string caseNotesDB, string pass)
        {
            var prefs = new SQLiteDatabase();
            if (string.IsNullOrEmpty(pass))
                prefs.DBConnection = "Data Source=" + caseNotesDB;
            else
                prefs.DBConnection = "Data Source=" + caseNotesDB + ";Password=" + pass;

            var data = new Dictionary<string, string>();
            data.Add("AppLocation", AppLocation.X.ToString() + "," + AppLocation.Y.ToString());
            data.Add("AppSize", AppSize.Width.ToString() + "," + AppSize.Height.ToString());

            var result = prefs.Update("Location", data, "1 = 1");
            return result;
        }

        public bool WriteNoteLocation(string caseNotesDB, string pass)
        {
            var prefs = new SQLiteDatabase();
            if (string.IsNullOrEmpty(pass))
                prefs.DBConnection = "Data Source=" + caseNotesDB;
            else
                prefs.DBConnection = "Data Source=" + caseNotesDB + ";Password=" + pass;

            var data = new Dictionary<string, string>();

            data.Add("NoteLocation", NoteLocation.X.ToString() + "," + NoteLocation.Y.ToString());
            data.Add("NoteSize", NoteSize.Width.ToString() + "," + NoteSize.Height.ToString());

            var result = prefs.Update("Location", data, "1 = 1");
            return result;
        }

        public bool WriteSystemPrefs(string caseNotesDB, string pass)
        {
            var prefs = new SQLiteDatabase();
            if (string.IsNullOrEmpty(pass))
                prefs.DBConnection = "Data Source=" + caseNotesDB;
            else
                prefs.DBConnection = "Data Source=" + caseNotesDB + ";Password=" + pass;

            prefs.ClearTable("SysPrefs");

            if (!File.Exists(caseNotesDB))
                prefs.InitialisePrefs(caseNotesDB);

            if (string.IsNullOrEmpty(CustomColors))
            {
                var newColor = new ColorDialog();
                var Colors = (int[])newColor.CustomColors.Clone();
                foreach (var color in Colors)
                {
                    CustomColors += color.ToString() + ",";
                }
                CustomColors = CustomColors.Substring(0, CustomColors.Length - 1);
            }

            var data = new Dictionary<string, string>();
            data.Add("MetaDataItems", MetaDataItems.ToString());
            data.Add("TabWindows", TabItems.ToString());
            data.Add("DefaultLocation", DefaultLocation);
            data.Add("ChecklistLocation", ChecklistLocation);
            data.Add("TemplateLocation", TemplateLocation); 
            data.Add("MetadataTypeface", MetadataTypeface);
            data.Add("MetadataTypeSize", MetadataTypeSize.ToString());
            data.Add("NoteTypeface", NoteTypeface);
            data.Add("NoteTypeSize", NoteTypeSize.ToString());
            data.Add("DateLastUpdated", DateLastUpdated);
            data.Add("MainTitle", MainTitle);
            data.Add("CustomColors", CustomColors);
            data.Add("CaseStatus", CaseStatus);

            try
            {
                var result = prefs.Insert("SysPrefs", data);
                return result;
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
                return false;
            }
        }

        public bool WriteUserPrefs(string caseNotesDB, string pass)
        {
            var prefs = new SQLiteDatabase();
            if (string.IsNullOrEmpty(pass))
                prefs.DBConnection = "Data Source=" + caseNotesDB;
            else
                prefs.DBConnection = "Data Source=" + caseNotesDB + ";Password=" + pass;

            prefs.ClearTable("UserPrefs"); 

            if (!File.Exists(caseNotesDB))
                prefs.InitialisePrefs(caseNotesDB);

            var data = new Dictionary<string, string>();
            bool res1 = false;
            bool res2 = false;
            bool res3 = false;

            foreach (var entry in MetaDescArray)
            {
                data.Add("PrefDataType", "metaDesc");
                data.Add("PrefDataValue", entry);
                res1 = prefs.Insert("UserPrefs", data);
                data.Clear();
            }

            foreach (var entry in MetaValueArray)
            {
                data.Add("PrefDataType", "metaValue");
                data.Add("PrefDataValue", entry);
                res2 = prefs.Insert("UserPrefs", data);
                data.Clear();
            }

            foreach (var entry in TabsValueArray)
            {
                data.Add("PrefDataType", "tabsDesc");
                data.Add("PrefDataValue", entry);
                res3 = prefs.Insert("UserPrefs", data);
                data.Clear();
            }

            return (res1 | res2 | res3);

        }
    }
}
