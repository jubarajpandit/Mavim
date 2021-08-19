using Mavim.Manager.Connect.Read.Databases;
using Mavim.Manager.Connect.Read.Databases.Interfaces;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.Connect.Read.Extensions
{
    /// <summary>
    /// The extensionclass to add database connections
    /// </summary>
    public static class DatabaseConnectionExtension
    {
        const string ConnectionStringConfigKey = "Mavim:ConnectReadSettings:ConnectConnectionString";

        /// <summary>
        /// Adds a connect database connection injectable in the service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="isDevelopment"></param>
        public static void AddConnectDatabaseConnection(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            string connectionString = configuration.GetSection(ConnectionStringConfigKey).Value;

            MigrateDatabase(connectionString, isDevelopment);

            services.AddDbContext<ConnectDatabaseContext>(options =>
            {
                var connection = GetConnection(connectionString, isDevelopment);
                options.UseSqlServer(connection);
            });

            services.AddScoped<IConnectRepository, ConnectRepository>();
            services.AddScoped<IConnectConnection, ConnectDataAccess>(_ =>
            {
                var connection = GetConnection(connectionString, isDevelopment);
                return new ConnectDataAccess(connection);
            });
        }

        /// <summary>
        /// Migrates the database
        /// TODO: WI-21465 Entity framework migrations (database up/downgrade)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="isDevelopment"></param>
        private static void MigrateDatabase(string connectionString, bool isDevelopment)
        {
            var options = new DbContextOptionsBuilder<ConnectDatabaseContext>();
            var connection = GetConnection(connectionString, isDevelopment);
            options.UseSqlServer(connection);

            using var dbcontext = new ConnectDatabaseContext(options.Options);
            dbcontext.Database.Migrate();
        }

        private static SqlConnection GetConnection(string connectionString, bool isDevelopment)
        {
            SqlConnection Connection;

            if (isDevelopment)
            {
                Connection = new SqlConnection(connectionString);
            }
            else
            {
                const string Resource = "https://database.windows.net/";
                Connection = new SqlConnection(connectionString)
                {
                    AccessToken = new AzureServiceTokenProvider().GetAccessTokenAsync(Resource).Result,
                };
            }

            return Connection;
        }
    }
}
