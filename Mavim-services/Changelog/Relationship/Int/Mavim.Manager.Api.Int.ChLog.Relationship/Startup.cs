using Mavim.Libraries.Authorization.Extensions;
using Mavim.Libraries.Middlewares.ExceptionHandler.Extensions;
using Mavim.Libraries.Middlewares.Language.Extensions;
using Mavim.Manager.Api.Int.ChLog.Relationship.Extensions;
using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.Interfaces.v1;
using Mavim.Manager.Api.Int.ChLog.Relationship.Repository.v1;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.Interfaces.v1;
using Mavim.Manager.Api.Int.ChLog.Relationship.Services.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Logging;

namespace Mavim.Manager.Api.Int.ChLog.Relationship
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

            services.AddHttpContextAccessor();
            services.ConfigureNewtonsoftJsonSerializerSettings();
            services.AddFeatureManagement();
            services.AddAzureAppConfiguration();

            services.AddAzureAppConfiguration();

            services.AddApplicationInsights(_isDevelopment);
            services.AddScoped<IRelationService, RelationService>();
            services.AddScoped<IRelationRepository, RelationRepository>();

            services.AddRouting();
            services.AddSwaggerGenerator();

            services.AddAuthorization(Configuration, _isDevelopment);

            services.AddDatabaseConnection(Configuration, _isDevelopment);
            services.AddDataLanguage();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!_isDevelopment)
                app.UseAzureAppConfiguration();

            app.AddExceptionHandler(_isDevelopment);
            app.UseRouting();
            app.AddSecurity(env);

            app.UseAuth()
               .UseAuthorizationChangelog();

            app.UseDataLanguage();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwaggerAndSwaggerUi();

            app.MigrateDatabase(_isDevelopment);
        }
    }
}
