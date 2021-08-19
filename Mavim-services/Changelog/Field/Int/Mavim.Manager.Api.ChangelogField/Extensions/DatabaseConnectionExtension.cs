﻿using Mavim.Manager.ChangelogField.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.ChangelogField.Extensions
{
    public static class DatabaseConnectionExtension
    {
        public static void AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.AddDbContext<FieldDbContext>(options =>
            {

                const string ConnectionStringConfigKey = "Mavim:ChangelogFieldSettings:ConnectionString";

                string ConnectionString = configuration.GetSection(ConnectionStringConfigKey).Value;

                if (isDevelopment)
                {
                    //options.UseSqlServer(ConnectionString);
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
                scope.ServiceProvider.GetService<FieldDbContext>().Database.Migrate();
            }
        }
    }
}