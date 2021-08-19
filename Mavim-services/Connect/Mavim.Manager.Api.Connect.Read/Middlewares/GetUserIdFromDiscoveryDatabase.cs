using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Connect.Read.Featureflags;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases.Interfaces;
using Mavim.Manager.Connect.Read.Extensions;
using Mavim.Manager.Connect.Read.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Connect.Read.Middlewares
{
    /// <summary>
    /// Middleware to fetch the userId
    /// </summary>
    public class GetUserIdFromDiscoveryDatabase
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;
        private readonly ILogger<GetUserIdFromDiscoveryDatabase> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserIdFromDiscoveryDatabase"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="cache">The cache server.</param>
        /// <param name="logger">The cache server.</param>
        /// <exception cref="ArgumentNullException">
        /// next
        /// </exception>
        public GetUserIdFromDiscoveryDatabase(
            RequestDelegate next,
            IDistributedCache cache,
            ILogger<GetUserIdFromDiscoveryDatabase> logger
            )
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repository">The Connect Repository.</param>
        /// <param name="token">The IJwtSecurityToken.</param>
        /// <param name="userIdentity">The userId injectable.</param>
        /// 
        /// <param name="featureManager">The feature manager.</param>
        /// <exception cref="ArgumentNullException">
        /// context
        /// or
        /// dbContext
        /// or
        /// token
        /// or
        /// userIdentity
        /// or
        /// cache
        /// or
        /// logger
        /// or
        /// featureManager
        /// </exception>
        /// <exception cref="RequestNotFoundException">Database not found</exception>
        public async Task Invoke(
            HttpContext context,
            IJwtSecurityToken token,
            IUserIdentity userIdentity,
            IConnectRepository repository,
            IFeatureManager featureManager
            )
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (token is null) throw new ArgumentNullException(nameof(token));
            if (userIdentity is null) throw new ArgumentNullException(nameof(userIdentity));
            if (repository is null) throw new ArgumentNullException(nameof(repository));
            if (featureManager is null) throw new ArgumentNullException(nameof(featureManager));

            if (IsAuthorizedEndpoint(context) && await featureManager.IsEnabledAsync(nameof(Features.ConnectRead)))
            {
                string key = $"{token.Email}:{token.TenantId}";
                string base64Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(key));

                IDiscoveryUser user = await GetUserFromCache(base64Key);
                if (user is null)
                {
                    user = await GetUserFromDatabase(token, repository);
                    if (user is null || user.Disabled)
                        throw new ForbiddenRequestException(Logging.NOT_ALLOWED);

                    SetUserToCache(base64Key, user);
                }

                userIdentity.Id = user.Id;
            }

            await _next.Invoke(context);
        }

        private async Task<IDiscoveryUser> GetUserFromCache(string base64Key)
        {
            try
            {
                return await _cache.GetDiscoverUserAsync<string>(base64Key);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Could not connect to the redis database, aborting redis operation. Message: {e.Message}");
            }

            return null;
        }

        private async Task<IDiscoveryUser> GetUserFromDatabase(IJwtSecurityToken token, IConnectRepository repository)
        {
            return await repository.GetDiscoveryUser(token.Email, token.TenantId);
        }

        private void SetUserToCache(string base64Key, IDiscoveryUser user)
        {
            try
            {
                // fire and forget
                _ = _cache.SetRecordAsync(base64Key, user);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Could not connect to the redis database, aborting redis operation. Message: {e.Message}");
            }
        }

        private bool IsAuthorizedEndpoint(HttpContext context)
        {
            return !context.Request.Path.StartsWithSegments("/swagger")
                && !context.Request.Path.StartsWithSegments("/version")
                && !context.Request.Path.StartsWithSegments("/");
        }
    }
}
