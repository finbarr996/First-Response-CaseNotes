using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes
{
    class Functions
    {
        public static Point String2Point(string inputString)
        {
            int first;
            int second;
            var pos = inputString.IndexOf(',');
            int.TryParse(inputString.Substring(0, pos), out first);
            int.TryParse(inputString.Substring(pos + 1), out second);

            var outputPoint = new Point(first, second);

            return outputPoint;
        }

        public static string ValidateCaseFile(string CaseFile, string FilePass)
        {
            if (!File.Exists(CaseFile))
            {
                return "CaseFile doesn't exist or cannot be found (has it moved or been renamed?)";
            }

            var fi = new FileInfo(CaseFile);
            if (fi.Length < 100 || fi.IsReadOnly)
                return "CaseFile is blank or read only";

            try
            {
                var metaData = new SQLiteDatabase();
                if (string.IsNullOrEmpty(FilePass))
                    metaData.DBConnection = "Data Source=" + CaseFile;
                else
                    metaData.DBConnection = "Data Source=" + CaseFile + ";Password=" + FilePass;

                var dt = metaData.GetDataTable("select * from SysPrefs;");

                return "success";
            }
            catch (Exception fail)
            {
                return fail.Message;
            }
        }

        public static string ConfigureDatabase(string CaseFile, string FilePass)
        {
            var dbversion = "";
            var db = new SQLiteDatabase();

            if (string.IsNullOrEmpty(FilePass))
                db.DBConnection = "Data Source=" + CaseFile;
            else
                db.DBConnection = "Data Source=" + CaseFile + ";Password=" + FilePass;

            var dt = db.GetDataTable("SELECT * FROM sqlite_master WHERE name='dbVer' AND type='table';");

            if (dt.Rows.Count == 0)
            {
                db.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS dbVer (admin_id INTEGER PRIMARY KEY AUTOINCREMENT, database_version VARCHAR(10), when_updated VARCHAR(19));");
                db.ExecuteNonQuery("insert into dbVer (database_version, when_updated) values ('2.17.2.12','" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "');");
                dbversion = "2.17.2.12";
            }

            dt = db.GetDataTable("SELECT MAX(admin_id), database_version, when_updated FROM dbVer;");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dbversion = dr[1].ToString();
                }
                if (dbversion == "")
                {
                    db.ExecuteNonQuery("insert into dbVer (database_version, when_updated) values ('2.17.2.12','" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "');");
                    dbversion = "2.17.2.12";
                }
            }

            try
            {
                if (dbversion == "2.17.2.12")
                {
                    MessageBox.Show("Your CaseFile is now being upgraded to the latest version\r\nYou should make a backup copy of your CaseFile before proceeding\r\n\r\nClick OK to Continue...","Database Upgrade", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    db.ExecuteNonQuery("DROP TABLE HASH;");
                    db.ExecuteNonQuery("CREATE TABLE Hash (MetaHash BLOB, DataHash BLOB, AuditHash BLOB, FileHash BLOB, Notes BLOB);");
                    db.ExecuteNonQuery("insert into dbVer (database_version, when_updated) values ('2.17.8.20','" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "');");
                    dbversion = "2.17.8.20";
                    var status = ConvertDB(CaseFile, FilePass);
                    if (status == 7)
                        MessageBox.Show("Success! Your CaseFile has been upgraded to the latest version!", "Database Upgrade Complete",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (status == 6)
                        MessageBox.Show("Failed - your Metadata has not converted correctly.", "Database Upgrade Failed",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (status == 5)
                        MessageBox.Show("Failed - your CaseNote data has not converted correctly.", "Database Upgrade Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (status == 4)
                        MessageBox.Show("Failed - your Metadata & CaseNote data has not converted correctly.", "Database Upgrade Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (status == 3)
                        MessageBox.Show("Failed - your Audit data has not converted correctly.", "Database Upgrade Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (status == 2)
                        MessageBox.Show("Failed - your Metadata & Audit data has not converted correctly.", "Database Upgrade Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (status == 1)
                        MessageBox.Show("Failed - your CaseNotes & Audit data has not converted correctly.", "Database Upgrade Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (dbversion == "2.17.8.20")
                {
                    db.ExecuteNonQuery("ALTER TABLE SysPrefs ADD COLUMN CaseStatus VARCHAR(256);");
                    db.ExecuteNonQuery("insert into dbVer (database_version, when_updated) values ('2.18.4.20','" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "');");
                    dbversion = "2.18.4.20";
                }
            }
            catch (Exception fail)
            {
                var problem = fail.Message;
            }

            return dbversion;
        }

        public static int ConvertDB(string CaseFileDB, string FilePass)
        {
            #region Metadata

            var status = 0;
            var result = 0;
            var data = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                data.DBConnection = "Data Source=" + CaseFileDB;
            else
                data.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            string dbConnection;
            if (string.IsNullOrEmpty(FilePass))
                dbConnection = "Data Source=" + CaseFileDB;
            else
                dbConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;
            
            var dt = data.GetDataTable("select * from MetaData;");
            var metaDataArray = new List<string>();
            var MetaOK = true;
            var hashes = "";

            foreach (DataRow dr in dt.Rows)
                hashes += dr[3].ToString();

            var MetaHash = String2Byte(GetMd5(hashes));
            MetaHash = Compress(MetaHash);

            try
            {
                var commandValue = "";
                dt = data.GetDataTable("select * from Hash;");
                if (dt.Rows.Count == 0)
                    commandValue = "INSERT INTO Hash (MetaHash) VALUES (@MetaHash)";
                else
                    commandValue = "UPDATE Hash SET MetaHash = @MetaHash";

                using (var connection = new SQLiteConnection(dbConnection))
                using (var command = new SQLiteCommand(connection))
                {
                    connection.Open();
                    command.CommandText = commandValue;
                    command.Parameters.Add("@MetaHash", DbType.Binary).Value = MetaHash;

                    result = command.ExecuteNonQuery();
                    connection.Close();
                }
                if (result > 0) status = 1;
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
            }

            #endregion
        
            #region Notes data

            dt = data.GetDataTable("select * from Notes;");
            hashes = "";
            var Notes = 0;

            foreach (DataRow dr in dt.Rows)
            {
                hashes += dr[4];
                Notes++;
            }

            var dataHash = String2Byte(GetMd5(hashes));
            dataHash = Compress(dataHash);
            var numNotes = String2Byte(Notes.ToString(CultureInfo.InvariantCulture));
            numNotes = Compress(numNotes);

            try
            {
                var commandValue = "";
                dt = data.GetDataTable("select * from Hash;");
                if (dt.Rows.Count == 0)
                    commandValue = "INSERT INTO Hash (DataHash, Notes) VALUES (@dataHash, @numNotes)";
                else
                    commandValue = "UPDATE Hash SET DataHash = @dataHash, Notes = @numNotes";
                
                using (var connection = new SQLiteConnection(dbConnection))
                using (var command = new SQLiteCommand(connection))
                {
                    connection.Open();

                    command.CommandText = commandValue;
                    command.Parameters.Add("@dataHash", DbType.Binary).Value = dataHash;
                    command.Parameters.Add("@numNotes", DbType.Binary).Value = numNotes;

                    result = command.ExecuteNonQuery();
                    connection.Close();
                }
                if (result > 0) status += 2;
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
            }

            #endregion

            #region Audit Data

            dt = data.GetDataTable("select * from Audit;");
            hashes = "";

            foreach (DataRow dr in dt.Rows)
                hashes += dr[4].ToString();

            var auditHash = String2Byte(GetMd5(hashes));
            auditHash = Compress(auditHash);

            try
            {
                var commandValue = "";
                dt = data.GetDataTable("select * from Hash;");
                if (dt.Rows.Count == 0)
                    commandValue = "INSERT INTO Hash (AuditHash) VALUES (@auditHash)";
                else
                    commandValue = "UPDATE Hash SET AuditHash = @auditHash";

                using (var connection = new SQLiteConnection(dbConnection))
                using (var command = new SQLiteCommand(connection))
                {
                    connection.Open();
                    command.CommandText = commandValue;
                    command.Parameters.Add("@AuditHash", DbType.Binary).Value = auditHash;

                    result = command.ExecuteNonQuery();
                    connection.Close();
                }
                if (result > 0) status += 4;
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
            }
            #endregion

            return status;
        }

        public static byte[] Compress(byte[] raw)
        {
            using (var memory = new MemoryStream())
            {
                using (var gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        public static byte[] Decompress(byte[] gzip)
        {
            var memory = new MemoryStream();
            try
            {

                using (var stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
                {
                    const int size = 4096;
                    var buffer = new byte[size];
                    using (memory)
                    {
                        int count;
                        do
                        {
                            count = stream.Read(buffer, 0, size);
                            if (count > 0)
                                memory.Write(buffer, 0, count);

                        } while (count > 0);

                        return memory.ToArray();
                    }
                }
            }
            catch (Exception fail)
            {
                MessageBox.Show("Decompress failed: " + fail.Message);
            }
            return memory.ToArray();
        }
        
        public static byte[] Encrypt(byte[] clearData, byte[] key, byte[] iv)
        {
            var ms = new MemoryStream();
            var alg = Rijndael.Create();

            alg.Key = key;
            alg.IV = iv;

            var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(clearData, 0, clearData.Length);
            cs.Close();

            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }

        public static byte[] Decrypt(byte[] cipherData, byte[] key, byte[] iv)
        {
            var ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();

            alg.Key = key;
            alg.IV = iv;

            var cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }

        public static string Encrypt(string clearText, string password)
        {
            var clearBytes = Encoding.Unicode.GetBytes(clearText);
            var pdb = new Rfc2898DeriveBytes(password, new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
            var encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            return Convert.ToBase64String(encryptedData);
        }

        public static string Decrypt(string cipherText, string password)
        {
            var cipherBytes = Convert.FromBase64String(cipherText);
            var pdb = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            var decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Encoding.Unicode.GetString(decryptedData);
        }

        public static byte[] Encrypt(byte[] clearData, string password)
        {
            var pdb = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            return Encrypt(clearData, pdb. GetBytes(32), pdb.GetBytes(16));
        }

        public static byte[] Decrypt(byte[] cipherData, string password)
        {
            var pdb = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        public static string String2Hex(string asciiString)
        {
            var hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        public static string Hex2String(string hexValue)
        {
            var StrValue = "";
            while (hexValue.Length > 0)
            {
                StrValue += Convert.ToChar(Convert.ToUInt32(hexValue.Substring(0, 2), 16)).ToString();
                hexValue = hexValue.Substring(2, hexValue.Length - 2);
            }
            return StrValue;
        }

        public static byte[] String2Byte(string convert)
        {
            return Encoding.UTF8.GetBytes(convert);
        }

        public static string Byte2String(byte[] convert)
        {
            var str = Encoding.UTF8.GetString(convert);
            return str;
        }

        public static string Char2String(char[] convert)
        {
            var str = new string(convert);
            return str;
        }

        public static string GetMd5(string str)
        {
            Encoder enc = Encoding.Unicode.GetEncoder();

            var unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);
            byte[] result = (new MD5CryptoServiceProvider()).ComputeHash(unicodeText);

            var sb = new StringBuilder();
            for (var i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static string GetMd5(byte[] inputArray)
        {
            byte[] result = (new MD5CryptoServiceProvider()).ComputeHash(inputArray);

            var sb = new StringBuilder();
            for (var i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static int[] GetCustomColors(string CaseNotesDB)
        {
            var prefs = new Preferences();
            prefs.ReadSystemPrefs(CaseNotesDB, null, "open");
            
            var colours = (prefs.CustomColors.Split(','));
            var intColors = new int[16];
            var count = 0;

            try
            {
                foreach (var color in colours)
                {
                    intColors[count++] = Convert.ToInt32(color);
                }

                return intColors;
            }
            catch (Exception)
            {
                return intColors;
            }
        }

        public static string SetCustomColors(int[] colors, string CaseNotesDB)
        {
            var CustomColors = "";

            foreach (var color in colors)
            {
                CustomColors += color.ToString() + ",";
            }

            CustomColors = CustomColors.Substring(0, CustomColors.Length - 1);

            var prefs = new Preferences();

            prefs.ReadSystemPrefs(CaseNotesDB, null, "open");
            prefs.CustomColors = CustomColors;
            prefs.WriteSystemPrefs(CaseNotesDB, null);
            
            return CustomColors;
        } 
    }
}
