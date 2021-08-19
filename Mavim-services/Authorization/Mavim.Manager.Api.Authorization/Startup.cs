using Mavim.Libraries.Authorization.Extensions;
using Mavim.Libraries.Middlewares.ExceptionHandler.Extensions;
using Mavim.Manager.Api.Authorization.Extensions;
using Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Interface;
using Mavim.Manager.Api.Authorization.Repository.v1;
using Mavim.Manager.Api.Authorization.Services.Interfaces.v1;
using Mavim.Manager.Api.Authorization.Services.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Logging;

namespace Mavim.Manager.Api.Authorization
{
    public class Startup
    {
        private readonly bool _isDevelopment;
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _isDevelopment = env.IsDevelopment();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true; //To show detail of error and see the problem

            services.AddHttpContextAccessor();
            services.GetAzureAppSettings(Configuration);
            services.AddFeatureManagement();
            services.AddApplicationInsights(_isDevelopment);
            services.AddRouting();
            services.ConfigureNewtonsoftJsonSerializerSettings();
            services.AddSwaggerGenerator();
            services.AddControllers();
            services.AddAzureAppConfiguration();

            services.AddAuthorization(Configuration, _isDevelopment);

            services.AddDatabaseConnection(Configuration, _isDevelopment);

            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
                app.UseAzureAppConfiguration();
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
