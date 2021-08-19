using Mavim.Libraries.Authorization.Extensions;
using Mavim.Libraries.Changelog.Extensions;
using Mavim.Libraries.Middlewares.ExceptionHandler.Extensions;
using Mavim.Libraries.Middlewares.Language.Extensions;
using Mavim.Manager.Api.Ext.ChLog.Relationship.Extensions;
using Mavim.Manager.Api.Ext.ChLog.Services.Interfaces.v1;
using Mavim.Manager.Api.Ext.ChLog.Services.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Logging;

namespace Mavim.Manager.Api.Ext.ChLog.Relationship
{
    public class Startup
    {
        private readonly bool _isDevelopment;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _isDevelopment = env.IsDevelopment();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true; //To show detail of error and see the problem

            services.AddAzureAppConfiguration();
            services.AddHttpContextAccessor();
            services.AddApplicationInsights(_isDevelopment);
            services.AddFeatureManagement();
            services.AddRouting();
            services.AddChangelogRelationshipClient(Configuration);
            services.AddAuthorization(Configuration, _isDevelopment);
            services.AddScoped<IChangelogRelationshipPublicService, ChangelogRelationshipPublicService>();
            services.AddDataLanguage();
            services.ConfigureNewtonsoftJsonSerializerSettings();
            services.AddSwaggerGenerator();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAzureAppConfiguration();
            app.AddExceptionHandler(_isDevelopment);
            app.UseRouting();
            app.AddSecurity(env);
            app.UseDataLanguage();

            app.UseAuth().UseAuthorizationChangelog();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwaggerAndSwaggerUi();
        }
    }
}
