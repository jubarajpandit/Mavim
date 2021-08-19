using Mavim.Manager.WopiFileLock.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.WopiHost.Extensions
{
    public static class DatabaseConnectionExtension
    {
        public static void AddFileLockDatabaseConnection(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.AddDbContext<WopiFileLockStateDbContext>(options =>
            {

                const string ConnectionStringConfigKey = "Mavim:WopiSettings:FileLockDbConnectionString";

                string ConnectionString = configuration.GetSection(ConnectionStringConfigKey).Value;

                if (isDevelopment)
                {
                    options.UseSqlServer(ConnectionString);
                }
                else
                {
                    const string Resource = "https://database.windows.net/";
                    SqlConnection connection = new SqlConnection(ConnectionString)
                    {
                        AccessToken = new AzureServiceTokenProvider().GetAccessTokenAsync(Resource).Result
                    };
                    options.UseSqlServer(connection);
                }
            });
        }

        // TODO: WI-21465 Entity framework migrations (database up/downgrade)
        public static void MigrateDatabase(this IApplicationBuilder app, bool isDevelopment)
        {
            if (!isDevelopment)
            {
                using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                scope.ServiceProvider.GetService<WopiFileLockStateDbContext>().Database.Migrate();
            }
        }
    }
}
