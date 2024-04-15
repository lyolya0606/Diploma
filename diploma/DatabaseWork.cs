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
        private string _pathToDB = @"Data Source=..\..\..\ImportantFiles\databaseDiploma.db";
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



        public List<string> GetMarks() {
            List<string> marks = new List<string>();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = $"SELECT designation FROM final_product";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read()) {
                string myreader = sqlite_datareader.GetString(0);
                marks.Add(myreader);
            }
            sqlite_conn.Close();
            return marks;
        }

        //string ReadData(SQLiteConnection conn) {
        //    string test = "";
        //    SQLiteDataReader sqlite_datareader;
        //    SQLiteCommand sqlite_cmd;
        //    sqlite_cmd = conn.CreateCommand();
        //    sqlite_cmd.CommandText = "SELECT designation FROM final_product";
        //    sqlite_datareader = sqlite_cmd.ExecuteReader();

        //    while (sqlite_datareader.Read()) {
        //        string myreader = sqlite_datareader.GetString(0);
        //        test += myreader;
        //    }
        //    conn.Close();
        //    return test;

        //}

        public string GetNameFreon(string mark) {
            string name = "";
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT name FROM final_product WHERE designation = @designation";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@designation", mark));
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read()) {
                string myreader = sqlite_datareader.GetString(0);
                name = myreader;
            }
            sqlite_conn.Close();
            return name;
        }

        public string GetFormulaFreon(string mark) {
            string formula = "";
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT formula FROM final_product JOIN chemical_formula ON " +
                "chemical_formula.id_chemical_formula = final_product.id_chemical_formula WHERE final_product.designation = @designation";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@designation", mark));
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read()) {
                string myreader = sqlite_datareader.GetString(0);
                formula = myreader;
            }
            sqlite_conn.Close();
            return formula;
        }

        public string GetArea(string mark) {
            string area = "";
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT application_area FROM final_product WHERE designation = @designation";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@designation", mark));
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read()) {
                string myreader = sqlite_datareader.GetString(0);
                area = myreader;
            }
            sqlite_conn.Close();
            return area;
        }

        public string GetSchemeFreon(string mark) {
            string scheme = "";
            int id = GetIdFreon(mark);
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT scheme FROM recipe WHERE recipe.id_final_product = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read()) {
                string myreader = sqlite_datareader.GetString(0);
                scheme = myreader;
                break;
            }
            sqlite_conn.Close();
            return scheme;
        }

        private int GetIdFreon(string mark) {
            int id = 0;
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT id_final_product FROM final_product WHERE designation = @designation";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@designation", mark));
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read()) {
                int myreader = sqlite_datareader.GetInt32(0);
                id = myreader;
            }
            sqlite_conn.Close();
            return id;
        }

        public List<Tuple<string, string>> GetEquipment(string mark) {
            List<Tuple<string, string>> equipment = [];
            int id = GetIdFreon(mark);
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT equipment.name, equipment.designation FROM stage JOIN recipe ON " +
                "recipe.id_stage = stage.id_stage JOIN equipment_for_stage ON equipment_for_stage.id_stage = stage.id_stage " +
                "JOIN equipment ON equipment.id_equipment = equipment_for_stage.id_equipment WHERE recipe.id_final_product = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read()) {
                equipment.Add(new Tuple<string, string>(sqlite_datareader.GetString(0), sqlite_datareader.GetString(1)));
            }
            sqlite_conn.Close();
            return equipment;
        }


        public List<int> GetSequenceStages(string mark) {
            List<int> sequence = new();
            int id = GetIdFreon(mark);
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT sequence FROM recipe WHERE id_final_product = @id_final_product";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id_final_product", id));
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read()) {
                string myreader = sqlite_datareader.GetString(0);
                sequence = myreader.Split('-').Select(Int32.Parse).ToList();
                break;
            }
            sqlite_conn.Close();
            return sequence;
        }

        public string GetStage(int id) {
            string stage = "";
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT name FROM stage WHERE id_stage = @id_stage";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id_stage", id));
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read()) {
                string myreader = sqlite_datareader.GetString(0);
                stage = myreader; ;
            }
            sqlite_conn.Close();
            return stage;
        }

        public string GetStageEquipment(int idStage) {
            string equipment = "";
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT equipment.designation FROM equipment JOIN " +
                "equipment_for_stage ON equipment.id_equipment = equipment_for_stage.id_equipment " +
                "WHERE equipment_for_stage.id_stage = @idStage";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@idStage", idStage));
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read()) {
                equipment += sqlite_datareader.GetString(0) + ", ";
            }
            sqlite_conn.Close();
            return equipment;
        }

        public Dictionary<string, string> GetEquipment(int idStage) {
            Dictionary<string, string> equipment = new();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT equipment.name, equipment.designation FROM equipment " +
                "JOIN equipment_for_stage ON equipment.id_equipment = equipment_for_stage.id_equipment " +
                "WHERE equipment_for_stage.id_stage = @idStage";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@idStage", idStage));
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read()) {
                equipment[sqlite_datareader.GetString(1)] = sqlite_datareader.GetString(0);
            }
            sqlite_conn.Close();
            return equipment;
        }

    }
}
