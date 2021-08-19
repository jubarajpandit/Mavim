using Mavim.Libraries.Authorization.Extensions;
using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Extensions;
using Mavim.Libraries.Middlewares.Language.Extensions;
using Mavim.Libraries.Middlewares.WopiValidator.Extensions;
using Mavim.Manager.Api.WopiFileLock.Repository.Interfaces.v1;
using Mavim.Manager.Api.WopiFileLock.Repository.Model.v1;
using Mavim.Manager.Api.WopiFileLock.Services.Interfaces;
using Mavim.Manager.Api.WopiFileLock.Services.v1;
using Mavim.Manager.Api.WopiHost.Extensions;
using Mavim.Manager.Api.WopiHost.Repository;
using Mavim.Manager.Api.WopiHost.Repository.Interfaces.v1;
using Mavim.Manager.Api.WopiHost.Repository.v1;
using Mavim.Manager.Api.WopiHost.Services;
using Mavim.Manager.Api.WopiHost.Services.Interfaces.v1;
using Mavim.Manager.Api.WopiHost.Services.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

namespace Mavim.Manager.Api.WopiHost
{
    public class Startup
    {
        private const string VersionPath = "/v1/version";
        private const string WopiActionUrl = "/v1/wopiactionurl";
        private const string SwaggerPath = "/swagger/v1/swagger.json";
        private readonly bool IsDevelopment;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            IsDevelopment = env.IsDevelopment();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.ConfigureNewtonsoftJsonSerializerSettings();
            services.AddFeatureManagement();
            services.AddApplicationInsights(IsDevelopment);
            services.AddAzureAppConfiguration();

            services.AddRouting();

            services.AddScoped<IMavimDbDataAccess, DataAccess>();
            services.AddFileLockDatabaseConnection(Configuration, IsDevelopment);
            services.AddScoped<IWopiFileLockService, WopiFileLockService>();
            services.AddScoped<IWopiFileLockRepository, WopiFileLockRepository>();
            services.AddScoped<IDescriptionService, DescriptionService>();
            services.AddScoped<IWopiActionUrlMetadataService, WopiActionUrlMetadataService>();
            services.AddScoped<IChartService, ChartService>();
            services.AddScoped<IChartRepository, ChartRepository>();
            services.AddScoped<IDescriptionRepository, DescriptionRepository>();
            services.AddSwaggerGenerator();

            services.AddKeyVaultSecrets(Configuration, IsDevelopment);
            services.AddAuthorization(Configuration, IsDevelopment);
            services.AddDataLanguage();

            services.AddWopiValidationMiddleware(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
                app.UseAzureAppConfiguration();

            app.AddExceptionHandler(IsDevelopment);
            app.UseRouting();
            app.AddSecurity(env);

            app.UseAuth()
               .UseMavimDatabaseMiddleware(env.IsDevelopment())
               .UseAuthorizationTopic();
            app.UseDataLanguage();

            app.UseWhen(context => !context.Request.Path.Equals("/") &&
            !context.Request.Path.Equals(VersionPath, System.StringComparison.InvariantCultureIgnoreCase) &&
            !context.Request.Path.Equals(WopiActionUrl, System.StringComparison.InvariantCultureIgnoreCase) &&
            !context.Request.Path.Equals(SwaggerPath, System.StringComparison.InvariantCultureIgnoreCase), appBuilder =>
            {
                appBuilder.UseWopiValidationMiddleware();
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwaggerAndSwaggerUi();

            app.MigrateDatabase(IsDevelopment);
        }
    }
}
