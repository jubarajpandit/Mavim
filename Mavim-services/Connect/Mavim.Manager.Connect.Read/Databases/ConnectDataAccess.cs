using Mavim.Manager.Connect.Read.Databases.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Databases
{
    public class ConnectDataAccess : IConnectConnection, IAsyncDisposable
    {
        private readonly SqlConnection _connection;

        public ConnectDataAccess(SqlConnection connection)
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
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}
