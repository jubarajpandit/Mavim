using Mavim.Manager.Authorization.Read.Databases;
using Mavim.Manager.Authorization.Read.Databases.Interfaces;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.Authorization.Read.Extensions
{
    /// <summary>
    ///     Database Connection Extension
    /// </summary>
    public static class DatabaseConnectionExtension
    {
        const string ConnectionStringConfigKey = "Mavim:AuthReadSettings:ConnectionString";

        /// <summary>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="isDevelopment"></param>
        public static void AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration,
            bool isDevelopment)
        {
            var connectionString = configuration.GetSection(ConnectionStringConfigKey).Value;

            MigrateDatabase(connectionString, isDevelopment);

            services.AddDbContext<AuthorizationDatabaseContext>(options =>
            {
                var connection = GetConnection(connectionString, isDevelopment);
                options.UseSqlServer(connection);
            });


            services.AddTransient<IRepository, Repository>();
            services.AddScoped<IConnection, DataAccess>(_ =>
            {
                var connection = GetConnection(connectionString, isDevelopment);
                return new DataAccess(connection);
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
            var options = new DbContextOptionsBuilder<AuthorizationDatabaseContext>();
            var connection = GetConnection(connectionString, isDevelopment);
            options.UseSqlServer(connection);

            using var dbcontext = new AuthorizationDatabaseContext(options.Options);
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