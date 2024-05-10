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


        public List<List<string>> GetTableFinalProduct() {
            //DataTable dt = new DataTable();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM final_product";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<List<string>> table = new List<List<string>>();
            while (sqlite_datareader.Read()) {
                List<string> row = new List<string> { sqlite_datareader.GetInt32(0).ToString(), sqlite_datareader.GetInt32(1).ToString(), sqlite_datareader.GetString(2), sqlite_datareader.GetString(3), sqlite_datareader.GetString(4) };

                table.Add(row);

                //equipment.Add(new Tuple<string, string>(sqlite_datareader.GetString(0), sqlite_datareader.GetString(1)));
            }
            sqlite_conn.Close();

            return table;

        }

        public List<List<string>> GetTableChemic() {
            //DataTable dt = new DataTable();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM chemical_formula";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<List<string>> table = new List<List<string>>();
            while (sqlite_datareader.Read()) {
                List<string> row = new List<string> { sqlite_datareader.GetInt32(0).ToString(), sqlite_datareader.GetString(1) };

                table.Add(row);

                //equipment.Add(new Tuple<string, string>(sqlite_datareader.GetString(0), sqlite_datareader.GetString(1)));
            }
            sqlite_conn.Close();

            return table;

        }

        public List<List<string>> GetTableRecipe() {
            //DataTable dt = new DataTable();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM recipe";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<List<string>> table = new List<List<string>>();
            while (sqlite_datareader.Read()) {
                List<string> row = new List<string> { sqlite_datareader.GetInt32(0).ToString(), sqlite_datareader.GetInt32(1).ToString(), sqlite_datareader.GetString(2), sqlite_datareader.GetString(3) };

                table.Add(row);

                //equipment.Add(new Tuple<string, string>(sqlite_datareader.GetString(0), sqlite_datareader.GetString(1)));
            }
            sqlite_conn.Close();

            return table;

        }

        public List<List<string>> GetTableEquipment() {
            //DataTable dt = new DataTable();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM equipment";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<List<string>> table = new List<List<string>>();
            while (sqlite_datareader.Read()) {
                List<string> row = new List<string> { sqlite_datareader.GetInt32(0).ToString(), sqlite_datareader.GetString(1), sqlite_datareader.GetString(2) };

                table.Add(row);

                //equipment.Add(new Tuple<string, string>(sqlite_datareader.GetString(0), sqlite_datareader.GetString(1)));
            }
            sqlite_conn.Close();
            return table;

        }

        public List<List<string>> GetTableStage() {
            //DataTable dt = new DataTable();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM stage";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<List<string>> table = new List<List<string>>();
            while (sqlite_datareader.Read()) {
                List<string> row = new List<string> { sqlite_datareader.GetInt32(0).ToString(), sqlite_datareader.GetString(2) };

                table.Add(row);

                //equipment.Add(new Tuple<string, string>(sqlite_datareader.GetString(0), sqlite_datareader.GetString(1)));
            }
            sqlite_conn.Close();
            return table;

        }

        public List<List<string>> GetTableKinetic() {
            //DataTable dt = new DataTable();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM kinetic_parameter";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<List<string>> table = new List<List<string>>();
            while (sqlite_datareader.Read()) {
                List<string> row = new List<string> { sqlite_datareader.GetInt32(0).ToString(), sqlite_datareader.GetInt32(1).ToString(), sqlite_datareader.GetString(2), sqlite_datareader.GetString(3) };

                table.Add(row);

                //equipment.Add(new Tuple<string, string>(sqlite_datareader.GetString(0), sqlite_datareader.GetString(1)));
            }
            sqlite_conn.Close();

            return table;

        }

        public List<List<string>> GetTableKineticValue() {
            //DataTable dt = new DataTable();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM kinetic_parameter_value";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<List<string>> table = new List<List<string>>();
            while (sqlite_datareader.Read()) {
                List<string> row = new List<string> { sqlite_datareader.GetInt32(0).ToString(), sqlite_datareader.GetInt32(1).ToString(), sqlite_datareader.GetFloat(2).ToString()};

                table.Add(row);

                //equipment.Add(new Tuple<string, string>(sqlite_datareader.GetString(0), sqlite_datareader.GetString(1)));
            }
            sqlite_conn.Close();

            return table;

        }

        public List<List<string>> GetTableUnit() {
            //DataTable dt = new DataTable();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM unit_measurement";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<List<string>> table = new List<List<string>>();
            while (sqlite_datareader.Read()) {
                List<string> row = new List<string> { sqlite_datareader.GetInt32(0).ToString(), sqlite_datareader.GetString(1) };

                table.Add(row);

                //equipment.Add(new Tuple<string, string>(sqlite_datareader.GetString(0), sqlite_datareader.GetString(1)));
            }
            sqlite_conn.Close();

            return table;

        }

        public void UpdateFinalProduct(string id, string name, string designation, string area) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE final_product SET name = @name, designation = @designation, " +
                "application_area = @area WHERE id_final_product = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@name", name));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@designation", designation));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@area", area));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void UpdateEquipment(string id, string name, string designation) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE equipment SET name = @name, designation = @designation WHERE id_equipment = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@name", name));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@designation", designation));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void UpdateStage(string id, string name) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE stage SET name = @name WHERE id_stage = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@name", name));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void UpdateChemic(string id, string name) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE chemical_formula SET formula = @formula WHERE id_chemical_formula = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@formula", name));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void UpdateRecipe(string id, string idStage, string seq, string scheme) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE recipe SET sequence = @seq, scheme = @scheme WHERE id_final_product = @id AND id_stage = @idStage";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@seq", seq));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@scheme", scheme));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@idStage", idStage));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void UpdateKinetic(string id, string idUnit, string name, string designation) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE kinetic_parameter SET id_unit_measurement = @idUnit, name = @name, designation = @designation " +
                "WHERE id_kinetic_parameter = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@idUnit", idUnit));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@name", name));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@designation", designation));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void UpdateKineticValue(string id, string idProd, string value) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE kinetic_parameter_value SET value = @value WHERE id_kinetic_parameter = @id AND id_final_product = @idProd";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@value", value));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@idProd", idProd));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void UpdateUnit(string id, string designation) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE unit_measurement SET designation = @designation WHERE id_unit_measurement = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@designation", designation));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void InsertFinalProduct(string idChem, string name, string designation, string area) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO final_product (id_chemical_formula, name, designation, application_area) " +
                                $"VALUES (@id_chemical_formula, @name, @designation, @area)";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id_chemical_formula", idChem));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@name", name));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@designation", designation));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@area", area));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void InsertRecipe(string id, string idStage, string seq, string scheme) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO recipe (id_final_product, id_stage, sequence, scheme) " +
                                $"VALUES (@id, @idStage, @seq, @scheme)";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@idStage", idStage));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@seq", seq));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@scheme", scheme));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void InsertEquipment(string name, string designation) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO equipment (name, designation) " +
                                $"VALUES (@name, @designation)";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@name", name));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@designation", designation));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void InsertStage(string name) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO stage (name) VALUES (@name)";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@name", name));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void InsertChemic(string name) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO chemical_formula (formula) VALUES (@formula)";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@formula", name));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void InsertKinetic(string idUnit, string name, string designation) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO kinetic_parameter (id_unit_measurement, name, designation) VALUES " +
                "(@idUnit, @name, @designation)";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@idUnit", idUnit));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@name", name));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@designation", designation));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void InsertKineticValue(string id, string idProd, string value) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO kinetic_parameter_value (id_kinetic_parameter, id_final_product, value) " +
                                $"VALUES (@id, @idProd, @value)";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@idProd", idProd));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@value", value));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void InsertUnit(string designation) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO unit_measurement (designation) VALUES (@designation)";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@designation", designation));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }
        public void DeleteFinalProduct(string id) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM final_product WHERE id_final_product = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void DeleteChemic(string id) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM chemical_formula WHERE id_chemical_formula = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void DeleteEquipment(string id) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM equipment WHERE id_equipment = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void DeleteStage(string id) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM stage WHERE id_stage = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void DeleteRecipe(string id, string idStage) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM recipe WHERE id_final_product = @id AND id_stage = @idStage";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@idStage", idStage));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void DeleteKinetic(string id) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM kinetic_parameter WHERE id_kinetic_parameter = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }


        public void DeleteKineticValue(string id, string idProd) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM kinetic_parameter_value WHERE id_final_product = @idProd AND id_kinetic_parameter = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@idProd", idProd));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void DeleteUnit(string id) {
            //SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            SQLiteConnection sqlite_conn = new SQLiteConnection(_pathToDB);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM unit_measurement WHERE id_unit_measurement = @id";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@id", id));
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }
    }
}
