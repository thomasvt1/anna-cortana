using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace BotAnna.Dialogs
{
    public class DatabaseConnector
    {

        private String server = "localhost";
        private String database = "";
        private String uid = "root";
        private String password = "";

        private void InsertSQL(String query)
        {
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        private String SelectSQL(String query)
        {
            String list = null;

            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                list = dataReader.GetString("data");
            }
            connection.Close();

            return list;
        }

        private int SelectSQLInt(String query)
        {
            int list = 0;

            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                list = dataReader.GetInt32("IdPatient");
            }
            connection.Close();

            return list;
        }

        public int CheckPatient(String patient)
        {
            String query = $"SELECT IdPatient FROM patient WHERE Firstname = '{patient}'";  // make query to check if patient is know in database

            int PatientId = SelectSQLInt(query);

            return PatientId;
        }

        public void AddNote(String note, int IdPatient)
        {
            String query = $"INSERT INTO `note` (`IdNote`, `IdCaretaker`, `IdPatient`, `data`, `timestamp`) VALUES(NULL, '1', '{IdPatient}', '{note}', CURRENT_TIMESTAMP);";

            InsertSQL(query);
        }

        public String GetNote(String patient, int PatientId)
        {
            String message = null;
            String query = "SELECT data FROM note WHERE IdNote = (SELECT COUNT(IdNote) FROM note);";


            message = SelectSQL(query);

            return message;
        }


    }
}