using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BioskopCSharp.SetupRDBMS
{
    class Command : IDisposable
    {
        public string Query { private get; set; }

        public List<T> ExecuteQuery<T>(Func<dynamic, T> Entity) where T : class
        {
            var datalist = new List<T>();
            try
            {
                using (var state = new Connect())
                {
                    var con = state.GetConnect();
                    var command = new MySqlCommand(Query, con);
                    var result = command.ExecuteReader();
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            datalist.Add(Entity(result));
                        }
                    }
                    state.CloseConnection();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: RDBMS.RetrievingData at " + e.StackTrace);
            }
            return datalist;
        }

        public bool ExecuteUpdate()
        {
            using (var state = new Connect())
            {
                var con = state.GetConnect();
                Console.WriteLine(Query);
                var command = new MySqlCommand(Query, con);
                command.ExecuteNonQuery();
                state.CloseConnection();
            }
            return true;
        }

        public void FillCombo(dynamic cbo)
        {
            try
            {
                using (var state = new Connect())
                {
                    var con = state.GetConnect();
                    var list = new Dictionary<string, string>();
                    var command = new MySqlCommand(Query, con);
                    var result = command.ExecuteReader();
                    if (result.HasRows)
                    {
                        list["0"] = "<Pilih Satu>";
                        while (result.Read())
                        {
                            list[result.GetValue(0).ToString()] = result.GetString(1);
                        }
                    }
                    state.CloseConnection();
                    cbo.Items.Clear();
                    cbo.ItemsSource = list;
                    cbo.DisplayMemberPath = "Value";
                    cbo.SelectedValuePath = "Key";
                    cbo.SelectedIndex = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: CBO.FillComboRecord at " + e.StackTrace);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dilakukan: dispose managed state (managed objects).
                }

                // dilakukan: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // dilakukan: set large fields to null.

                disposedValue = true;
            }
        }

        // dilakukan: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Command() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // dilakukan: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
