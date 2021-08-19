using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Manager.Model;
using Mavim.Manager.Server.Tarantula;
using System;

namespace Mavim.Manager.Api.WopiHost.Repository
{
    public class DataAccess : IDisposable, IMavimDbDataAccess
    {
        /// <summary>
        /// Session object
        /// </summary>
        private ISession Session { get; set; }

        public IMavimDatabaseModel DatabaseModel { get; set; }

        public void Connect(string connectionString, string schema, string accessToken = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Missing value for: ", nameof(connectionString));

            if (string.IsNullOrWhiteSpace(schema))
                throw new ArgumentException("Missing value for: ", nameof(schema));

            if (DatabaseModel != null)
                return;

            Session = SessionFactory.CreateSession(connectionString, accessToken, schema);

            DatabaseModel = MavimDatabaseModelManager.Instance.GetMavimDatabaseModel(Session.MavimHandle);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Session != null)
                        Session.Dispose();
                }

                disposedValue = true;
            }

        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}

