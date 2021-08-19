using Mavim.Libraries.Authorization.Extensions;
using Mavim.Libraries.Changelog.Extensions;
using Mavim.Libraries.Middlewares.ExceptionHandler.Extensions;
using Mavim.Libraries.Middlewares.Language.Extensions;
using Mavim.Manager.Api.ChangelogTitle.Public.Extensions;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.Interfaces.v1;
using Mavim.Manager.Api.ChangelogTitle.Public.Services.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Logging;

namespace Mavim.Manager.Api.ChangelogTitle.Public
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
            services.AddRouting();
            services.AddChangelogTitleClient(Configuration);
            services.AddFeatureManagement();
            services.AddAuthorization(Configuration, _isDevelopment);
            services.AddScoped<IChangelogTitlePublicService, ChangelogTitlePublicService>();
            services.AddDataLanguage();
            services.ConfigureNewtonsoftJsonSerializerSettings();
            services.AddSwaggerGenerator();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.AddExceptionHandler(_isDevelopment);
            app.UseRouting();
            app.UseAzureAppConfiguration();
            app.AddSecurity(env);

            app.UseAuth().UseAuthorizationChangelog();
            app.UseDataLanguage();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwaggerAndSwaggerUi();
        }
    }
}
