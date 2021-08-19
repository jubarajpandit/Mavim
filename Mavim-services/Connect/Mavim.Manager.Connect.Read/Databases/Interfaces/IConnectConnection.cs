using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Databases.Interfaces
{
    public interface IConnectConnection
    {
        Task<DataTable> ExecuteQueryAsync(SqlCommand command);
        ValueTask DisposeAsync();
    }
}
