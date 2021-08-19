using Mavim.Manager.ChLog.Relationship.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Extensions
{
    public static class DatabaseConnectionExtension
    {
        public static void AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.AddDbContext<RelationDbContext>(options =>
            {

                const string connectionStringConfigKey = "Mavim:ChangelogRelationSettings:ConnectionString";

                string connectionString = configuration.GetSection(connectionStringConfigKey).Value;

                if (isDevelopment)
                {
                    options.UseSqlServer(connectionString);
                }
                else
                {
                    const string resource = "https://database.windows.net/";
                    SqlConnection connection = new SqlConnection(connectionString)
                    {
                        AccessToken = new AzureServiceTokenProvider().GetAccessTokenAsync(resource).Result
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
                scope.ServiceProvider.GetService<RelationDbContext>().Database.Migrate();
            }
        }
    }
}
