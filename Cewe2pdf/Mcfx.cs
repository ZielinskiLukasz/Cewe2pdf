using System.Data.SQLite;
using System.Text;

namespace Cewe2pdf {

    class Mcfx {
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

            //using (var fileStream = new System.IO.FileStream("C:\\Users\\Stefan\\Desktop\\Cewe2pdf\\test_img.jpg", System.IO.FileMode.Create))
            //{
            //    fileStream.Write(blobData, 0, blobData.Length);
            //}

            using (var ms = new System.IO.MemoryStream(blobData)) {
                return System.Drawing.Image.FromStream(ms);
            }
        }

        public System.IO.MemoryStream getMcfFile() {
            byte[] blobData = getDataForFilename("data.mcf");
            if (blobData == null) return null;

            // TODO: first convert to string and cut off anything after </fotobook>
            // then convert to memory stream for returning
            // mcf has some binary data after </fotobook>, so we need to trim that off
            string mcf = Encoding.UTF8.GetString(blobData).Split("</fotobook>")[0] + "</fotobook>";

            byte[] trimmed = Encoding.UTF8.GetBytes(mcf);

            return new System.IO.MemoryStream(trimmed);
        }

        private string _filePath; // the database file path
    }
}
