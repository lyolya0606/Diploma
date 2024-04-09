using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Diploma {
    public class DatabaseWork {

        private SQLiteConnection _sqlite_conn;
        private string _pathToDB = @"Data Source=..\..\..\ImportantFiles\databaseCourseWork.db";
        public DatabaseWork() {
   
        }

        public List<List<string>> GetAFromDatabase() {
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT kinetic_parameter.name, kinetic_parameter_value.value FROM kinetic_parameter " +
                "JOIN kinetic_parameter_value ON kinetic_parameter.id_kinetic_parameter = kinetic_parameter_value.id_kinetic_parameter WHERE kinetic_parameter.id_kinetic_parameter < 22";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<List<string>> table = new List<List<string>>();
            while (sqlite_datareader.Read()) {
                List<string> row = new List<string> { sqlite_datareader.GetString(0), sqlite_datareader.GetFloat(1).ToString() };

                table.Add(row);

            }
            sqlite_conn.Close();

            return table;
        }

        public List<List<string>> GetEFromDatabase() {
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT kinetic_parameter.name, kinetic_parameter_value.value FROM kinetic_parameter JOIN " +
                "kinetic_parameter_value ON kinetic_parameter.id_kinetic_parameter = kinetic_parameter_value.id_kinetic_parameter WHERE kinetic_parameter.id_kinetic_parameter > 21";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<List<string>> table = new List<List<string>>();
            while (sqlite_datareader.Read()) {
                List<string> row = new List<string> { sqlite_datareader.GetString(0), sqlite_datareader.GetFloat(1).ToString() };

                table.Add(row);

            }
            sqlite_conn.Close();

            return table;
        }

        public void UpdateAE(string id, string name) {
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE kinetic_parameter SET name = @name WHERE id_kinetic_parameter = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@name", name));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void UpdateAEValue(string id, double value) {
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE kinetic_parameter_value SET value = @value WHERE id_kinetic_parameter = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@value", value));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public List<double> GetAValues() {
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT kinetic_parameter_value.value FROM kinetic_parameter_value WHERE kinetic_parameter_value.id_kinetic_parameter < 22";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<double> values = new();
            while (sqlite_datareader.Read()) {
                values.Add(double.Parse(sqlite_datareader.GetFloat(0).ToString()));

            }
            sqlite_conn.Close();

            return values;
        }

        public List<double> GetEValues() {
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT kinetic_parameter_value.value FROM kinetic_parameter_value WHERE kinetic_parameter_value.id_kinetic_parameter > 21";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<double> values = new();
            while (sqlite_datareader.Read()) {
                values.Add(double.Parse(sqlite_datareader.GetFloat(0).ToString()));

            }
            sqlite_conn.Close();

            return values;
        }
    }
}
