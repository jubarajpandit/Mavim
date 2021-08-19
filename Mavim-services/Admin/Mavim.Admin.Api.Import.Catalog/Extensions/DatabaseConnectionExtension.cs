using Mavim.Manager.MavimDatabaseInfo.EFCore;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Admin.Api.Import.Catalog.Extensions
{
    public static class DatabaseConnectionExtension
    {
        public static void AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.AddDbContext<MavimDatabaseInfoDbContext>(options =>
            {
                const string ConnectionStringConfigKey = "Mavim:AdminImportCatalogSettings:ConnectionString";

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
    }
}
