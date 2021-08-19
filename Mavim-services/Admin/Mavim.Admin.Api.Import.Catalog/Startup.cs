using Mavim.Admin.Api.Import.Catalog.Extensions;
using Mavim.Libraries.Middlewares.ExceptionHandler.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace Mavim.Admin.Api.Import.Catalog
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

            services.AddControllers();
            services.AddAzureAppConfiguration();

            services.AddApplicationInsights(_isDevelopment);
            services.AddSwaggerGenerator();
            services.AddKeyVaultSecrets(Configuration, _isDevelopment);
            services.AddAuthorization(Configuration, _isDevelopment);
            services.AddDatabaseConnection(Configuration, _isDevelopment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
                app.UseAzureAppConfiguration();
            app.AddExceptionHandler(_isDevelopment);
            app.UseRouting();
            app.AddSecurity(env);

            app.UseAuthentication()
                .UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwaggerAndSwaggerUi();
        }
    }
}
