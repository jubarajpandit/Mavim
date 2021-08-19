using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Manager.Model;
using Mavim.Manager.Server.Tarantula;
using System;

namespace Mavim.Manager.Api.Topic.Repository.v1.Models
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
                throw new ArgumentException("message", nameof(connectionString));

            if (string.IsNullOrWhiteSpace(schema))
                throw new ArgumentException("message", nameof(schema));

            if (DatabaseModel != null)
                return;

            Session = SessionFactory.CreateSession(connectionString, accessToken, schema);

            if (Session.MavimHandle == -1)
                throw new Exception($"Error while logging in to the Mavim database: {Session.LoginError.ToString()}");

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

