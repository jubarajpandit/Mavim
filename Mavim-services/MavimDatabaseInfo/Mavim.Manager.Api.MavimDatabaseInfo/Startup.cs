using Mavim.Libraries.Authorization.Extensions;
using Mavim.Libraries.Middlewares.ExceptionHandler.Extensions;
using Mavim.Manager.Api.MavimDatabaseInfo.Extensions;
using Mavim.Manager.Api.MavimDatabaseInfo.Services.Interfaces.v1;
using Mavim.Manager.Api.MavimDatabaseInfo.Services.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mavim.Manager.Api.MavimDatabaseInfo
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
            services.AddHttpContextAccessor();
            services.GetAzureAppSettings(Configuration);
            services.AddApplicationInsights(_isDevelopment);
            services.AddRouting();
            services.AddAuthorization(Configuration, _isDevelopment);
            services.AddDatabaseConnection(Configuration, _isDevelopment);
            services.AddScoped<IMavimDatabaseInfoService, MavimDatabaseInfoService>();
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
            app.AddExceptionHandler(_isDevelopment);

            app.UseRouting();

            app.AddSecurity(env);

            app.UseAuth();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwaggerAndSwaggerUi();

            app.MigrateDatabase(_isDevelopment);
        }
    }
}
