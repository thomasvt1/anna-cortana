using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private String server = "logger.thomasvt.xyz";
        private String database = "webhook";
        private String uid = "bryan";
        private String password = "55my3vuc";

        private void insertSQL(String query)
        {
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public void addNote()
        {

            

            String query = "INSERT INTO `note` (`IdNote`, `IdCaretaker`, `IdPatient`, `data`, `timestamp`) VALUES(NULL, '1', '1', 'Test', CURRENT_TIMESTAMP);";

            insertSQL(query);
        }
    }
}