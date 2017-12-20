using System;
using System.Data.SQLite;

namespace BioskopCSharp.SetupDBS {
    public class Connection_SQLite : IDisposable {
        private SQLiteConnection _connex;
        private string _dbname;
        private bool _state;

        public Connection_SQLite() {
            _connex = new SQLiteConnection();
            _dbname = "bioskop.db";
        }

        public object GetConnex {
            get {
                return _connex;
            }
        }

        public bool OpenConnection {
            get {
                _connex = new SQLiteConnection();
                return _state;
            }
            set {
                if (!value) return;
                _connex.ConnectionString = "Data Source=" + _dbname + ";Version=3;";
                _connex.Open();
                _state = (_connex.State == System.Data.ConnectionState.Open) ? true : false;
            }
        }

        public object CloseConnection {
            set {
                var result = (SQLiteConnection)value;
                result.Close();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // dilakukan: dispose managed state (managed objects).
                }
                disposedValue = true;
            }
        }

        ~Connection_SQLite() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
