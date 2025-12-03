using System.Data.SQLite;
using System.Text;

namespace Cewe2pdf {

    class Mcfx {
        /* Quick Note:
         * .mcfx is 'just' a sqlite3 database file, containing 3 columns:
         *  | Data | Filename | LastModified |
         *  this class wraps SQL commands to access specific data blobs from the database
        */

        public Mcfx(string filePath) {
            _filePath = filePath;
        }

        public byte[] getDataForFilename(string filename) {
            // Provide the path to your existing database
            string connectionString = "Data Source=" + _filePath + ";Version=3;";

            // Open the connection
            using (SQLiteConnection conn = new SQLiteConnection(connectionString)) {
                conn.Open();

                // Query the database (example: SELECT all rows from the Users table)
                string selectQuery = "SELECT Data FROM Files WHERE Filename = '" + filename + "';"; // Replace "Users" with your table name
                using (SQLiteCommand cmd = new SQLiteCommand(selectQuery, conn)) {
                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {

                        if (reader.Read()) {
                            // Read the BLOB data from the database
                            byte[] blobData = reader["Data"] as byte[];

                            if (blobData != null) {
                                return blobData;
                            }
                        }
                    }
                }
            }

            Log.Error("No BLOB data found for filename " + filename);
            return null;
        }

        public System.Drawing.Image getSystemImageForFilename(string filename) {
            byte[] blobData = getDataForFilename(filename);
            if (blobData == null) return null;

            // for debugging, write extracted binary data to jpg file - should be valid to open in any image viewer
            //using (var fileStream = new System.IO.FileStream("test_img.jpg", System.IO.FileMode.Create)) {
            //    fileStream.Write(blobData, 0, blobData.Length);
            //}

            // construct an in-memory system image from binary data
            using (var ms = new System.IO.MemoryStream(blobData)) {
                return System.Drawing.Image.FromStream(ms);
            }
        }

        public System.IO.MemoryStream getMcfFile() {
            byte[] blobData = getDataForFilename("data.mcf");
            if (blobData == null) return null;

            // TODO: first convert to string and cut off anything after </fotobook>
            // then convert to memory stream for returning
            // mcf has some binary data after </fotobook>, so we need to trim that off - not really sure what the data is for now
            string mcf = Encoding.UTF8.GetString(blobData).Split("</fotobook>")[0] + "</fotobook>";

            // mcf parser expects a memory stream, so convert mcf text back to binary
            byte[] trimmed = Encoding.UTF8.GetBytes(mcf);
            return new System.IO.MemoryStream(trimmed);
        }

        private string _filePath; // the database file path
    }
}
