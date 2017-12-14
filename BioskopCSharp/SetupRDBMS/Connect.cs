﻿using System;
using MySql.Data.MySqlClient;

namespace BioskopCSharp.SetupRDBMS
{
    class Connect : IDisposable
    {
        private static string cs = @"server=localhost;userid=root;password=;database=bioskop";

        private MySqlConnection connection;


        public Connect()
        {
            connection = new MySqlConnection(cs);
            connection.Open();
        }

        public MySqlConnection GetConnect()
        {
            return connection;
        }

        public void CloseConnection()
        {
            connection.Close();
            connection = null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Connect() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
