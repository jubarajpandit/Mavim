using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Mavim.Manager.Authorization.Read.Databases.Interfaces
{
    public interface IConnection : IDisposable, IAsyncDisposable
    {
        Task<DataTable> ExecuteQueryAsync(SqlCommand command);
    }
}
