using Azure.Security.KeyVault.Secrets;
using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Threading.Tasks;

namespace Mavim.Libraries.Authorization.Middleware
{
    public class MavimDatabaseMiddleware
    {
        private const string PathDatabaseKey = "dbId";
        private const string Resource = "https://database.windows.net/";

        private readonly RequestDelegate _next;
        private readonly SecretClient _secretClient;

        private readonly bool _isDevelopment;

        /// <summary>
        /// Initializes a new instance of the <see cref="MavimDatabaseMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="secretClient">The secret client.</param>
        /// <exception cref="ArgumentNullException">
        /// next
        /// or
        /// secretClient
        /// </exception>
        public MavimDatabaseMiddleware(
            RequestDelegate next,
            SecretClient secretClient,
            bool isDevelopment
            )
        {
            _isDevelopment = isDevelopment;
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _secretClient = secretClient ?? throw new ArgumentNullException(nameof(secretClient));
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="dataAccess">The data access.</param>
        /// <param name="catalog">The catalog.</param>
        /// <param name="sqlAccessTokenClient">The SQL access token client.</param>
        /// <exception cref="ArgumentNullException">
        /// context
        /// or
        /// dataAccess
        /// </exception>
        /// <exception cref="RequestNotFoundException">Database not found</exception>
        public async Task Invoke(
            HttpContext context,
            IMavimDbDataAccess dataAccess,
            IMavimDatabaseInfoClient mavimDatabaseInfoClient,
            IAzureSqlAccessTokenClient sqlAccessTokenClient
            )
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (dataAccess is null) throw new ArgumentNullException(nameof(dataAccess));

            object databaseGuidObject = context.GetRouteValue(PathDatabaseKey);

            if (databaseGuidObject != null && Guid.TryParse(databaseGuidObject.ToString(), out Guid databaseGuid))
            {
                IMavimDatabaseInfo dbInfo = await mavimDatabaseInfoClient.GetMavimDatabaseInfo(databaseGuid);

                if (dbInfo == null)
                    throw new RequestNotFoundException("Database not found");

                if (_isDevelopment)
                    IsDevelopment(dataAccess, dbInfo);

                else if (dbInfo.IsInternalDatabase)
                    await IsInternalDatabaseConnection(dataAccess, dbInfo);

                else if (!dbInfo.IsInternalDatabase)
                    await ExternalDatabaseConnection(dataAccess, dbInfo, sqlAccessTokenClient);
            }
            await _next.Invoke(context);
        }

        private async Task ExternalDatabaseConnection(
                IMavimDbDataAccess dataAccess,
                IMavimDatabaseInfo databaseInfo,
                IAzureSqlAccessTokenClient sqlAccessTokenClient
            )
        {
            if (databaseInfo is null)
                throw new ArgumentNullException(nameof(databaseInfo));
            if (string.IsNullOrWhiteSpace(databaseInfo.ApplicationTenantId?.ToString()))
                throw new Exception("No TenantId found in MavimDatabaseInfo");
            if (string.IsNullOrWhiteSpace(databaseInfo.ApplicationId?.ToString()))
                throw new Exception("No applicationId found in MavimDatabaseInfo");
            if (string.IsNullOrWhiteSpace(databaseInfo.ApplicationSecretKey?.ToString()))
                throw new Exception("No ApplicationSecretKey found in MavimDatabaseInfo");

            // Get Azure Ad App Secret from KeyVault
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(databaseInfo.ApplicationSecretKey.ToString());
            string applicationSecret = secret?.Value ?? string.Empty;

            if (string.IsNullOrWhiteSpace(applicationSecret))
                throw new Exception($"No ApplicationSecret found in KeyVault with key {databaseInfo.ApplicationSecretKey}");

            string sqlAccessToken = await sqlAccessTokenClient.GetTokenAsync((Guid)databaseInfo.ApplicationTenantId, (Guid)databaseInfo.ApplicationId, applicationSecret);

            dataAccess.Connect(databaseInfo.ConnectionString, databaseInfo.Schema, sqlAccessToken);
        }

        private async Task IsInternalDatabaseConnection(IMavimDbDataAccess dataAccess, IMavimDatabaseInfo databaseInfo)
        {
            string accessToken = await new AzureServiceTokenProvider().GetAccessTokenAsync(Resource);
            dataAccess.Connect(databaseInfo.ConnectionString, databaseInfo.Schema, accessToken);
        }

        private void IsDevelopment(IMavimDbDataAccess dataAccess, IMavimDatabaseInfo databaseInfo)
        {
            dataAccess.Connect(databaseInfo.ConnectionString, databaseInfo.Schema);
        }
    }
}
