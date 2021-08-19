using Mavim.Libraries.Middlewares.ExceptionHandler.Extensions;
using Mavim.Manager.Api.Connect.Write.Extensions;
using Mavim.Manager.Connect.Write;
using Mavim.Manager.Connect.Write.DomainModel;
using Mavim.Manager.Connect.Write.EventSourcing;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;
using Mavim.Manager.Connect.Write.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Logging;
using System;

namespace Mavim.Manager.Api.Connect.Write
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

            services.AddSwaggerGenerator();
            services.AddMediatR(typeof(Assembly).Assembly);
            services.AddDatabaseConnection(Configuration, _isDevelopment);

            services.AddServiceBusAsync(Configuration, _isDevelopment);

            var companyId = new Guid("f0fd2957-ddca-40c2-bc87-7dc9029ad2d3");
            services.AddScoped<IIdentityService>(_ => new IdentityService(Guid.Empty, Guid.Empty, companyId));

            services.AddTransient<ICommonEventSourcing, CommonEventSourcing>();
            services.AddTransient<IEventSourcingGeneric<UserV1>, UserV1EventSourcing>();
            services.AddTransient<IEventSourcingGeneric<GroupV1>, GroupV1EventSourcing>();
            services.AddTransient<IEventSourcingGeneric<CompanyV1>, CompanyV1EventSourcing>();
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

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}