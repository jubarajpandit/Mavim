using Mavim.Manager.Authorization.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Admin.Api.Import.Authorization.Extensions
{
    public static class DatabaseConnectionExtension
    {
        public static void AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.AddDbContext<AuthorizationDbContext>(options => {
                const string connectionStringConfigKey = "Mavim:AdminImportAuthSettings:ConnectionString";
                
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
            if (isDevelopment) return;
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<AuthorizationDbContext>().Database.Migrate();
        }
    }
}
