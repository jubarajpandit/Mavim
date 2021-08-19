using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Authorization.Middleware;
using Mavim.Libraries.Authorization.Models;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mavim.Libraries.Authorization.Extensions
{
    public static class AuthorizationHandlerExtension
    {

        private const string Authority = "https://sts.windows.net/{0}/";

        public static IServiceCollection AddAuth(this IServiceCollection serviceCollection, string applicationId, string tenant, bool isDevelopment, bool useQueryString = false, TokenValidationParameters validationParms = null)
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));

            if (string.IsNullOrEmpty(applicationId)) throw new ArgumentException("message", nameof(applicationId));

            if (string.IsNullOrEmpty(tenant)) throw new ArgumentException("message", nameof(tenant));

            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtSchema(applicationId, tenant, isDevelopment, useQueryString, validationParms));

            serviceCollection.AddScoped<IJwtSecurityToken, JwtToken>();

            return serviceCollection;
        }

        public static IServiceCollection AddAuth(this IServiceCollection serviceCollection, List<AuthSchema> schemas, bool isDevelopment)
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));
            if (schemas is null) throw new ArgumentNullException(nameof(schemas));

            if (schemas.Count(s => s.IsDefault) > 1) throw new Exception("Only allow to have 1 default schema");

            var defaultSchema = schemas.FirstOrDefault(s => s.IsDefault);

            if (defaultSchema is null) throw new Exception("No default schema has been set");

            var authBuilder = serviceCollection
                .AddAuthorization(options =>
                {
                    options.AddAzureAdPolicy(schemas.Select(schema => schema.Name).ToArray());
                })
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = defaultSchema.Name;
                    options.DefaultChallengeScheme = defaultSchema.Name;
                });

            schemas.ForEach(schema =>
            {
                authBuilder.AddJwtBearer(schema.Name, JwtSchema(schema.Audience, schema.TenantId, isDevelopment, schema.UseQueryString, schema.ValidationParms));
            });

            return serviceCollection;
        }

        private static Action<JwtBearerOptions> JwtSchema(string applicationId, string tenant, bool isDevelopment, bool useQueryString, TokenValidationParameters validationParms)
        {
            return options =>
            {
                options.RequireHttpsMetadata = !isDevelopment;
                options.SaveToken = true;
                options.Audience = applicationId;
                options.Authority = string.Format(Authority, tenant);
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                            throw new SecurityTokenExpiredException("JWT Token expired", context.Exception);

                        throw new JwtAuthenticationFailed(context.Exception.Message, context.Exception.InnerException);
                    },
                    OnMessageReceived = context =>
                    {
                        if (useQueryString)
                        {
                            // Read the token out of the query string
                            var accessToken = context.Request.Query[Manager.Api.Utils.Constants.Configuration.TOKEN_NAME];

                            if (!string.IsNullOrEmpty(accessToken))
                                context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = validationParms ?? new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = false,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                };
            };
        }

        public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication()
               .UseAuthorization();


            app.UseMiddleware<JwtSecurityTokenMiddleware>();

            return app;
        }

        private static AuthorizationOptions AddAzureAdPolicy(this AuthorizationOptions options, params string[] schemas)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(schemas)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy("MavimSchemaPolicy", policy);

            return options;
        }
    }

}