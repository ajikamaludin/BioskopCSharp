using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace BioskopCSharp.SetupDBS {
    public class Command_SQLite {
        public string Query { private get; set; }

        public List<T> ExecuteQuery<T>(Func<dynamic, T> Entity) where T: class {
            var datalist = new List<T>();
            try {
                using (var state = new Connection_SQLite()) {
                    state.OpenConnection = true;
                    var command = new SQLiteCommand(Query, (SQLiteConnection)state.GetConnex);
                    var result = command.ExecuteReader();
                    if (result.HasRows) {
                        while (result.Read()) datalist.Add(Entity(result));
                    }
                    state.CloseConnection = (SQLiteConnection)state.GetConnex;
                }
            } catch (Exception e) {
                Console.WriteLine("ERROR: RDBMS.RetrievingData at " + e.StackTrace);
            }
            return datalist;
        }

        public bool ExecuteUpdate() {
            using (var state = new Connection_SQLite()) {
                state.OpenConnection = true;
                Console.WriteLine(Query);
                var command = new SQLiteCommand(Query, (SQLiteConnection)state.GetConnex);
                command.ExecuteNonQuery();
                state.CloseConnection = (SQLiteConnection)state.GetConnex;
            }
            return true;
        }

        public void FillCombo(dynamic cbo) {
            try {
                using (var state = new Connection_SQLite()) {
                    state.OpenConnection = true;
                    var list = new Dictionary<string, string>();
                    var command = new SQLiteCommand(Query, (SQLiteConnection)state.GetConnex);
                    var result = command.ExecuteReader();
                    if (result.HasRows) {
                        list["0"] = "<Pilih Satu>";
                        while (result.Read()) {
                            list[result.GetValue(0).ToString()] = result.GetString(1);
                        }
                    }
                    state.CloseConnection = (SQLiteConnection)state.GetConnex;
                    cbo.Items.Clear();
                    cbo.ItemsSource = list;
                    cbo.DisplayMemberPath = "Value";
                    cbo.SelectedValuePath = "Key";
                    cbo.SelectedIndex = 0;
                }
            } catch (Exception e) {
                Console.WriteLine("ERROR: CBO.FillComboRecord at " + e.StackTrace);
            }
        }

        public void Report(string srctable, out DataSet dataset) {
            using (var state = new Connection_SQLite()) {
                state.OpenConnection = true;
                var data = new DataSet();
                var command = new SQLiteCommand(Query, (SQLiteConnection)state.GetConnex);
                var adapter = new SQLiteDataAdapter() {
                    SelectCommand = command
                };
                adapter.Fill(data, srctable);
                dataset = data;
                state.CloseConnection = (SQLiteConnection)state.GetConnex;
            }
        }

        public string Generate() {
            var list = string.Empty;
            using (var state = new Connection_SQLite()) {
                state.OpenConnection = true;
                var command = new SQLiteCommand(Query, (SQLiteConnection)state.GetConnex);
                var result = command.ExecuteReader();
                if (result.HasRows) {
                    while (result.Read()) {
                        var n = result.GetInt32(0) + 1;
                        list = n.ToString();
                    }
                }
                state.CloseConnection = (SQLiteConnection)state.GetConnex;
            }
            return list;
        }

        public object Report(string srctable) {
            var data = new DataSet();
            using (var state = new Connection_SQLite()) {
                state.OpenConnection = true;
                var command = new SQLiteCommand(Query, (SQLiteConnection)state.GetConnex);
                var adapter = new SQLiteDataAdapter() {
                    SelectCommand = command
                };
                adapter.Fill(data, srctable);
                state.CloseConnection = (SQLiteConnection)state.GetConnex;
            }
            return data;
        }
    }
}
