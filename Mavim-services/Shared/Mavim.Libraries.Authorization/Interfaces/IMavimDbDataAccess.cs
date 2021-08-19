using Mavim.Manager.Model;

namespace Mavim.Libraries.Authorization.Interfaces
{
    public interface IMavimDbDataAccess
    {
        IMavimDatabaseModel DatabaseModel { get; set; }
        void Connect(string connectionString, string schema, string accessToken = null);
    }
}
