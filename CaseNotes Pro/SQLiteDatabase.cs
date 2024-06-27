using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes
{
    internal class SQLiteDatabase
    {
        public String DBConnection;

        /// <summary>
        ///     Default Constructor for SQLiteDatabase Class.
        /// </summary>
        public SQLiteDatabase()
        {
            DBConnection = "";
        }

        /// <summary>
        ///     Single Param Constructor for specifying the DB file.
        /// </summary>
        /// <param name="inputFile">The File containing the DB</param>
        public SQLiteDatabase(string inputFile)
        {
            DBConnection = "Data Source=" + inputFile;
        }

        /// <summary>
        ///     Double Param Constructor for specifying the DB file and a password.
        /// </summary>
        /// <param name="inputFile">The File containing the DB</param>
        /// <param name="password">The password required to open this DB</param>
        public SQLiteDatabase(string inputFile, string password)
        {
            DBConnection = "Data Source=" + inputFile + ";Password=" + password;
        }

        /// <summary>
        ///     Double Param Constructor for specifying the DB file and a password.
        /// </summary>
        /// <param name="inputFile">The File containing the DB</param>
        /// <param name="password">The password required to open this DB</param>
        public void SQLiteSetPassword(string password)
        {
            var cnn = new SQLiteConnection(DBConnection);

            cnn.SetPassword(password);
            cnn.Open();

            cnn.Close();
        }

        /// <summary>
        ///     Single Param Constructor for specifying advanced connection options.
        /// </summary>
        /// <param name="connectionOpts">A dictionary containing all desired options and their values</param>
        public SQLiteDatabase(Dictionary<string, string> connectionOpts)
        {
            var str = "";
            foreach (KeyValuePair<string, string> row in connectionOpts)
            {
                str += row.Key + "=" + row.Value;
            }
            str = str.Trim().Substring(0, str.Length - 1);
            DBConnection = str;
        }

        /// <summary>
        ///     Creates a brand new Prefs db and defines all of the tables.
        /// </summary>
        /// <param name="path">The path, including the database filename to create.</param>
        /// <returns>true for success, false for a failure</returns>
        public void InitialisePrefs(string path)
        {
            if (!File.Exists(path))
                SQLiteConnection.CreateFile(path);

            DBConnection = "Data Source=" + path;

            const string createdbVer = "CREATE TABLE IF NOT EXISTS dbVer (admin_id INTEGER PRIMARY KEY AUTOINCREMENT, database_version VARCHAR(10), when_updated VARCHAR(19));";

            string dbVer = "insert into dbVer (database_version, when_updated) values ('2.17.8.20','" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "');";

            const string createSysPrefs = "CREATE TABLE IF NOT EXISTS SysPrefs (RowID INTEGER PRIMARY KEY AUTOINCREMENT, MetadataItems INTEGER NOT NULL, TabWindows INTEGER NOT NULL, " +
                                          "DefaultLocation VARCHAR(256), ChecklistLocation VARCHAR(256), TemplateLocation VARCHAR(256), MetadataTypeface VARCHAR(64) NOT NULL, MetadataTypeSize INTEGER NOT NULL, " +
                                          "NoteTypeface VARCHAR(64) NOT NULL, NoteTypeSize INTEGER NOT NULL, DateLastUpdated VARCHAR(24), MainTitle VARCHAR(256), CustomColors VARCHAR(256), CaseStatus VARCHAR(256));";

            const string createUserPrefs = "CREATE TABLE IF NOT EXISTS UserPrefs (PrefIndex INTEGER PRIMARY KEY AUTOINCREMENT, PrefDataType VARCHAR(45) NOT NULL, PrefDataValue VARCHAR(256));";

            const string createLocation = "CREATE TABLE IF NOT EXISTS Location (LocIndex INTEGER PRIMARY KEY AUTOINCREMENT, AppLocation VARCHAR(10) NOT NULL, AppSize VARCHAR(10) NOT NULL, " +
                                          "NoteLocation VARCHAR(10) NOT NULL, NoteSize VARCHAR(10) NOT NULL);";

            const string dbinit = createdbVer + createSysPrefs + createUserPrefs + createLocation;

            ExecuteNonQuery(dbinit);
            ExecuteNonQuery(dbVer);

            #region Set Database default values

            var defaults = new Dictionary<string, string>();
            defaults.Add("PrefDataType", "metaDesc");
            defaults.Add("PrefDataValue", "Case Reference:");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaValue");
            defaults.Add("PrefDataValue", "");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaDesc");
            defaults.Add("PrefDataValue", "Case Type:");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaValue");
            defaults.Add("PrefDataValue", "");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaDesc");
            defaults.Add("PrefDataValue", "Analyst Name:");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaValue");
            defaults.Add("PrefDataValue", "");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaDesc");
            defaults.Add("PrefDataValue", "Analyst Agency:");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaValue");
            defaults.Add("PrefDataValue", "");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaDesc");
            defaults.Add("PrefDataValue", "Client Name:");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaValue");
            defaults.Add("PrefDataValue", "");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaDesc");
            defaults.Add("PrefDataValue", "Client Agency:");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaValue");
            defaults.Add("PrefDataValue", "");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaDesc");
            defaults.Add("PrefDataValue", "Client Contact:");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaValue");
            defaults.Add("PrefDataValue", "");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaDesc");
            defaults.Add("PrefDataValue", "Item 8:");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaValue");
            defaults.Add("PrefDataValue", "");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaDesc");
            defaults.Add("PrefDataValue", "Item 9:");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaValue");
            defaults.Add("PrefDataValue", "");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaDesc");
            defaults.Add("PrefDataValue", "Item 10:");
            Insert("UserPrefs", defaults);
            defaults.Clear();

            defaults.Add("PrefDataType", "metaValue");
            defaults.Add("PrefDataValue", "");
            Insert("UserPrefs", defaults);

            var tabs = new Dictionary<string, string>();
            tabs.Add("PrefDataType", "tabsDesc");
            tabs.Add("PrefDataValue", "To Do List");
            Insert("UserPrefs", tabs);
            tabs.Clear();

            tabs.Add("PrefDataType", "tabsDesc");
            tabs.Add("PrefDataValue", "Exhibits");
            Insert("UserPrefs", tabs);
            tabs.Clear();

            tabs.Add("PrefDataType", "tabsDesc");
            tabs.Add("PrefDataValue", "Search Hits");
            Insert("UserPrefs", tabs);
            tabs.Clear();

            tabs.Add("PrefDataType", "tabsDesc");
            tabs.Add("PrefDataValue", "Items of Interest");
            Insert("UserPrefs", tabs);
            tabs.Clear();

            tabs.Add("PrefDataType", "tabsDesc");
            tabs.Add("PrefDataValue", "Tab 5");
            Insert("UserPrefs", tabs);
            tabs.Clear();

            tabs.Add("PrefDataType", "tabsDesc");
            tabs.Add("PrefDataValue", "Tab 6");
            Insert("UserPrefs", tabs);
            tabs.Clear();

            tabs.Add("PrefDataType", "tabsDesc");
            tabs.Add("PrefDataValue", "Tab 7");
            Insert("UserPrefs", tabs);
            tabs.Clear();

            tabs.Add("PrefDataType", "tabsDesc");
            tabs.Add("PrefDataValue", "Tab 8");
            Insert("UserPrefs", tabs);
            tabs.Clear();

            tabs.Add("PrefDataType", "tabsDesc");
            tabs.Add("PrefDataValue", "Tab 9");
            Insert("UserPrefs", tabs);
            tabs.Clear();

            tabs.Add("PrefDataType", "tabsDesc");
            tabs.Add("PrefDataValue", "Tab 10");
            Insert("UserPrefs", tabs);
            tabs.Clear();

            var sys = new Dictionary<string, string>();
            string temp = Application.UserAppDataPath;
            temp = temp.Substring(0, temp.LastIndexOf("\\"));
            sys.Add("MetadataItems", "7");
            sys.Add("TabWindows", "4");
            sys.Add("DefaultLocation", temp);
            sys.Add("ChecklistLocation", temp + "\\Templates");
            sys.Add("TemplateLocation", temp + "\\Templates");
            sys.Add("MetadataTypeface", "Arial");
            sys.Add("MetadataTypeSize", "9");
            sys.Add("NoteTypeface", "Arial");
            sys.Add("NoteTypeSize", "10");
            sys.Add("DateLastUpdated", DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString());
            sys.Add("MainTitle", "Forensic Analysis Contemporaneous Notes");
            sys.Add("CaseStatus", "Initiated#In Progress#Pending#Paused#Under Review#Closed#Archive");

            Insert("SysPrefs", sys);

            var loc = new Dictionary<string, string>();
            loc.Add("AppLocation", "100,120");
            loc.Add("AppSize", "1024,768");
            loc.Add("NoteLocation", "150,150");
            loc.Add("NoteSize", "900,600");

            Insert("Location", loc);

            #endregion

        }

        /// <summary>
        ///     Creates a brand new CaseFile db and define all of the tables.
        /// </summary>
        /// <param name="path">The path, including the database filename to create.</param>
        /// <returns>true for success, false for a failure</returns>
        public void InitialiseCase(string path, string pass)
        {
            if (!File.Exists(path))
                SQLiteConnection.CreateFile(path);

            if (string.IsNullOrEmpty(pass))
                DBConnection = "Data Source=" + path;
            else
                DBConnection = "Data Source=" + path + ";Password=" + pass;

            const string createSysPrefs = "CREATE TABLE IF NOT EXISTS SysPrefs (RowID INTEGER PRIMARY KEY AUTOINCREMENT, MetadataItems INTEGER NOT NULL, TabWindows INTEGER NOT NULL, " +
                                          "DefaultLocation VARCHAR(256), ChecklistLocation VARCHAR(256), TemplateLocation VARCHAR(256), MetadataTypeface VARCHAR(64) NOT NULL, MetadataTypeSize INTEGER NOT NULL, " +
                                          "NoteTypeface VARCHAR(64) NOT NULL, NoteTypeSize INTEGER NOT NULL, DateLastUpdated VARCHAR(24), MainTitle VARCHAR(256), CustomColors VARCHAR(256), CaseStatus VARCHAR(256));";

            const string createdbVer = "CREATE TABLE IF NOT EXISTS dbVer (admin_id INTEGER PRIMARY KEY AUTOINCREMENT, database_version VARCHAR(10), when_updated VARCHAR(19));";

            string dbVer = "insert into dbVer (database_version, when_updated) values ('2.17.8.20','" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "');";

            const string createUserPrefs = "CREATE TABLE IF NOT EXISTS UserPrefs (PrefIndex INTEGER PRIMARY KEY AUTOINCREMENT, PrefDataType VARCHAR(45) NOT NULL, PrefDataValue VARCHAR(256));";

            const string createMetadata = "CREATE TABLE IF NOT EXISTS MetaData (RowID INTEGER PRIMARY KEY AUTOINCREMENT, MetaKey VARCHAR(256) NOT NULL, MetaValue VARCHAR(256), Hash VARCHAR(32));";

            const string createNotes = "CREATE TABLE IF NOT EXISTS Notes (RowID INTEGER PRIMARY KEY AUTOINCREMENT, Entered VARCHAR(20), Occurred VARCHAR(20), CaseNote BLOB, Hash VARCHAR(32));";

            const string createTabMetadata = "CREATE TABLE IF NOT EXISTS TabMetaData (RowID INTEGER PRIMARY KEY AUTOINCREMENT, TabNum INTEGER NOT NULL, TabKey VARCHAR(256) NOT NULL);";

            const string createTabdata = "CREATE TABLE IF NOT EXISTS TabData (RowID INTEGER PRIMARY KEY AUTOINCREMENT, TabNum INTEGER NOT NULL, TabKey BLOB NOT NULL, TabValue BLOB);";

            const string createChecklist = "CREATE TABLE IF NOT EXISTS Checklist (RowID INTEGER PRIMARY KEY AUTOINCREMENT, ChecklistName VARCHAR(256), ChecklistPath VARCHAR(256), ChecklistXML BLOB, Hash VARCHAR(32));";

            const string createAudit = "CREATE TABLE IF NOT EXISTS Audit (RowID INTEGER PRIMARY KEY AUTOINCREMENT, Date VARCHAR(20) NOT NULL, EventType BLOB NOT NULL, EventDescription BLOB NOT NULL, Hash VARCHAR(32) NOT NULL);";

            const string createHash = "CREATE TABLE IF NOT EXISTS Hash (MetaHash BLOB, DataHash BLOB, AuditHash BLOB, FileHash BLOB, Notes BLOB);";

            const string dbinit = createSysPrefs + createdbVer + createUserPrefs + createMetadata + createNotes + createTabMetadata + createTabdata + createChecklist + createAudit + createHash;

            ExecuteNonQuery(dbinit);
            ExecuteNonQuery(dbVer);
        }

        /// <summary>
        ///     Runs a query against the Database.
        /// </summary>
        /// <param name="sql">The SQL to run</param>
        /// <returns>A DataTable containing the result set.</returns>
        public DataTable GetDataTable(string sql)
        {
            var dt = new DataTable();
            try
            {
                var con = new SQLiteConnection(DBConnection);
                con.Open();
                var mycommand = new SQLiteCommand(con);
                mycommand.CommandText = sql;
                SQLiteDataReader reader = mycommand.ExecuteReader();
                dt.Load(reader);
                reader.Close();
                con.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return dt;
        }

        /// <summary>
        ///     Interacts with the database for purposes other than a data returning query.
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>An Integer containing the number of rows updated.</returns>
        public int ExecuteNonQuery(string sql)
        {
            var cnn = new SQLiteConnection(DBConnection);
            cnn.Open();
            var mycommand = new SQLiteCommand(cnn);
            mycommand.CommandText = sql;
            int rowsUpdated = mycommand.ExecuteNonQuery();
            cnn.Close();
            return rowsUpdated;
        }

        /// <summary>
        ///     Retrieves a single item from the DB.
        /// </summary>
        /// <param name="sql">The query to run.</param>
        /// <returns>A string.</returns>
        public string ExecuteScalar(string sql)
        {
            var cnn = new SQLiteConnection(DBConnection);
            cnn.Open();
            var mycommand = new SQLiteCommand(cnn);
            mycommand.CommandText = sql;
            object value = mycommand.ExecuteScalar();
            cnn.Close();
            if (value != null)
            {
                return value.ToString();
            }
            return "";
        }

        /// <summary>
        ///     Updates individual rows in the DB.
        /// </summary>
        /// <param name="tableName">The table to update.</param>
        /// <param name="data">A dictionary containing Column names and their new values.</param>
        /// <param name="where">The where clause for the update statement.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Update(String tableName, Dictionary<String, String> data, String where)
        {
            var vals = "";
            var vals1 = "";
            var returnCode = true;
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<String, String> val in data)
                {
                    vals1 = val.Value.Replace("'", "");
                    vals += " " + val.Key + " = '" + vals1 + "',";
                }
                vals = vals.Substring(0, vals.Length - 1);
            }
            try
            {
                ExecuteNonQuery("update " + tableName + " set " + vals + " where " + where + ";");
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        ///     Deletes rows from the DB.
        /// </summary>
        /// <param name="tableName">The table from which to delete.</param>
        /// <param name="where">The where clause for the delete.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Delete(String tableName, String where)
        {
            var returnCode = true;
            try
            {
                ExecuteNonQuery("delete from " + tableName + "where " + where + ";");
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        ///     Insert a row into the DB.
        /// </summary>
        /// <param name="tableName">The table into which we insert the data.</param>
        /// <param name="data">A dictionary containing the column names and data for the insert.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Insert(String tableName, Dictionary<String, String> data)
        {
            var columns = "";
            var values = "";
            var vals1 = "";
            var returnCode = true;
            foreach (KeyValuePair<String, String> val in data)
            {
                columns += " " + val.Key + ",";
                vals1 = val.Value.Replace("'", "");
                values += " '" + vals1 + "',";
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                ExecuteNonQuery("insert into " + tableName + "(" + columns + ") values(" + values + ");");
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        ///     Deletes all data from the DB.
        /// </summary>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool ClearDB()
        {
            try
            {
                var tables = GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;");
                foreach (DataRow table in tables.Rows)
                {
                    ClearTable(table["NAME"].ToString());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Clears all data from a specific table.
        /// </summary>
        /// <param name="table">The name of the table to clear.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool ClearTable(String table)
        {
            try
            {
                ExecuteNonQuery("delete from " + table + ";");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}