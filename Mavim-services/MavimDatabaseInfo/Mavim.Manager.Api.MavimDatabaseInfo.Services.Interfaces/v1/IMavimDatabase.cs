using System;

namespace Mavim.Manager.Api.Catalog.Interfaces.v1
{
    public interface IMavimDatabase
    {
        /// <summary>
        /// Mavim database Id
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// displayName
        /// </summary>
        string DisplayName { get; set; }
        
        /// <summary>
        /// Database Connection String
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Database Schema
        /// </summary>
        string Schema { get; set; }
    }
}
