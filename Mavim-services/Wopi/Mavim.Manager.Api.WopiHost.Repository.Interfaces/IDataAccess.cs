using System;
using Mavim.Manager.Model;

namespace Mavim.Manager.Api.WopiHost.Repository.Interfaces
{
    public interface IDataAccess : IDisposable
    {
        IMavimDatabaseModel DatabaseModel { get; set; }
        void Connect(string connectionString, string schema, string accessToken = null);
    }
}

