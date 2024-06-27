using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes
{
    class Note
    {
        public string Entered { get; set; }
        public string Occurred { get; set; }
        public byte[] CaseNote { get; set; }
        public string Hash { get; set; }
        public List<Note> NoteArray { get; set; }

        public bool ReadNotes(string CaseFileDB, string FilePass)
        {
            var note = new SQLiteDatabase();
            if (string.IsNullOrEmpty(FilePass))
                note.DBConnection = "Data Source=" + CaseFileDB;
            else
                note.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

            var dt = note.GetDataTable("select * from Notes;");
            var noteArray = new List<Note>();
            var notesOK = true;
            var hashOK = true;
            var noteCount = true;
            var hashes = "";
            var count = 0;

            foreach (DataRow dr in dt.Rows)
            {
                var local = new Note();
                local.Entered = dr[1].ToString();
                local.Occurred = dr[2].ToString();
                local.CaseNote = Functions.Decompress((byte[]) dr[3]);
                local.Hash = (dr[4].ToString());
                hashes += local.Hash;
                count++;
                var hash = Functions.GetMd5(local.CaseNote);
                if (hash != local.Hash)
                    notesOK = false;

                noteArray.Add(local);
            }
            NoteArray = noteArray;

            if (count > 0)
            {
                dt = note.GetDataTable("select * from Hash;");
                foreach (DataRow dr in dt.Rows)
                {
                    var dataBlob = Functions.Decompress((byte[]) dr[1]);
                    var hash = Functions.GetMd5(hashes);
                    hashOK = (hash == Functions.Byte2String(dataBlob));
                    byte[] Notes;
                    if (dr[4] != null)
                    {
                        Notes = Functions.Decompress((byte[])dr[4]);
                        var numNotes = Int32.Parse(Functions.Byte2String(Notes));
                        noteCount = numNotes == count;
                    }
                    else
                        noteCount = true;
                }
            }

            return (notesOK && hashOK && noteCount);
        }

        public bool WriteNote(string CaseFileDB, string FilePass)
        {
            Hash = Functions.GetMd5(CaseNote);
            var result = 0;

            CaseNote = Functions.Compress(CaseNote);

            try
            {
                string dbConnection;
                if (string.IsNullOrEmpty(FilePass))
                    dbConnection = "Data Source=" + CaseFileDB;
                else
                    dbConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

                using (var connection = new SQLiteConnection(dbConnection))
                using (var command = new SQLiteCommand(connection))
                {
                    connection.Open();

                    command.CommandText = "INSERT INTO Notes (Entered, Occurred, CaseNote, Hash) VALUES (@Entered, @Occurred, @CaseNote, @Hash)";
                    command.Parameters.Add("@Entered", DbType.String).Value = Entered;
                    command.Parameters.Add("@Occurred", DbType.String).Value = Occurred; 
                    command.Parameters.Add("@CaseNote", DbType.Binary).Value = CaseNote;
                    command.Parameters.Add("@Hash", DbType.String).Value = Hash; 

                    result = command.ExecuteNonQuery();
                    connection.Close();
                }

                var note = new SQLiteDatabase();
                if (string.IsNullOrEmpty(FilePass))
                    note.DBConnection = "Data Source=" + CaseFileDB;
                else
                    note.DBConnection = "Data Source=" + CaseFileDB + ";Password=" + FilePass;

                var dt = note.GetDataTable("select * from Notes;");
                var hashes = "";
                var Notes = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    hashes += dr[4];
                    Notes++;
                }

                var dataHash = Functions.String2Byte(Functions.GetMd5(hashes));
                dataHash = Functions.Compress(dataHash);
                var numNotes = Functions.String2Byte(Notes.ToString(CultureInfo.InvariantCulture));
                numNotes = Functions.Compress(numNotes);

                try
                {
                    var commandValue = "";
                    dt = note.GetDataTable("select * from Hash;");
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
                }
                catch (Exception fail)
                {
                    MessageBox.Show(fail.Message);
                    return false;
                }

                return (result > 0);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
