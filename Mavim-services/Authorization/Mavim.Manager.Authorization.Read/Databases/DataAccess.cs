using Mavim.Manager.Authorization.Read.Databases.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Mavim.Manager.Authorization.Read.Databases
{
    public class DataAccess : IConnection
    {
        private readonly SqlConnection _connection;

        public DataAccess(SqlConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _connection.Open();
        }

        public async Task<DataTable> ExecuteQueryAsync(SqlCommand command)
        {
            var data = new DataTable();

            command.Connection = _connection;

            using (command)
            {
                using var reader = await command.ExecuteReaderAsync();
                data.Load(reader);
            }

            return data;
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
                await _connection.DisposeAsync();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_connection != null)
                        _connection.Dispose();
                }

                disposedValue = true;
            }

        }
        #endregion
    }
}
