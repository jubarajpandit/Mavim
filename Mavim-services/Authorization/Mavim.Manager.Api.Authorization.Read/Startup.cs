using Mavim.Libraries.Authorization.Extensions;
using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Authorization.Middleware;
using Mavim.Libraries.Authorization.Models;
using Mavim.Libraries.Middlewares.ExceptionHandler.Extensions;
using Mavim.Manager.Api.Authorization.Read.Extensions;
using Mavim.Manager.Authorization.Read;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Logging;

namespace Mavim.Manager.Api.Authorization.Read
{
    /// <summary>
    ///     Startup class
    /// </summary>
    public class Startup
    {
        private readonly bool _isDevelopment;

        /// <summary>
        ///     Startup constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _isDevelopment = env.IsDevelopment();
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true; //To show detail of error and see the problem

            services.AddHttpContextAccessor();
            services.AddFeatureManagement();
            services.AddAzureAppConfiguration();
            services.AddApplicationInsights(_isDevelopment);

            services.AddAuthorization(Configuration, _isDevelopment);
            services.AddControllers();
            services.AddNetApiVersioning();

            services.AddConnectHttpClient(Configuration);

            services.AddSwaggerGenerator();
            services.AddMediatR(typeof(Assembly).Assembly);
            services.AddDatabaseConnection(Configuration, _isDevelopment);
            services.AddScoped<IJwtSecurityToken, JwtToken>();

        }

        /// <summary>
        ///     Configures the application using the provided builder, hosting environment, and API version description provider.
        /// </summary>
        /// <param name="app">The current application builder.</param>
        /// <param name="env">The current webhost environment.</param>
        /// <param name="provider">The API version descriptor provider used to enumerate defined API versions.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (_isDevelopment)
                app.UseDeveloperExceptionPage();
            else
                app.UseAzureAppConfiguration();

            app.UseRouting();
            app.AddSecurity(env);
            app.UseSwaggerAndSwaggerUi(provider);
            app.AddExceptionHandler(_isDevelopment);
            app.UseAuthorization();

            app.UseWhen(
                httpContext => !httpContext.Request.Path.StartsWithSegments("/internal"),
                subApp =>
                {
                    subApp.UseMiddleware<JwtSecurityTokenMiddleware>();
                }
            );

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
