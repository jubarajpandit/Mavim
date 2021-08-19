using Mavim.Libraries.Authorization.Extensions;
using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Extensions;
using Mavim.Libraries.Middlewares.Language.Extensions;
using Mavim.Libraries.Wopi.Extensions;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Extensions;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Topics;
using Mavim.Manager.Api.Topic.Repository.v1;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Factory;
using Mavim.Manager.Api.Topic.Repository.v1.Models;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Services.v1;
using Mavim.Manager.Api.Topic.v1.Mappers;
using Mavim.Manager.Api.Topic.v1.Mappers.Interfaces;
using Mavim.Manager.Topic;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Logging;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;

namespace Mavim.Manager.Api.Topic
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private readonly bool _isDevelopment;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _isDevelopment = env.IsDevelopment();
        }

        /// <summary>
        /// Startup configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
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
            services.AddMavimApiVersioning();

            services.ConfigureNewtonsoftJsonSerializerSettings();
            services.AddSwaggerGenerator();

            services.AddKeyVaultSecrets(Configuration, _isDevelopment);
            services.AddDataLanguage();
            services.AddWopiHostClient(Configuration);

            services.AddScoped<IFieldService, FieldsService>();
            services.AddScoped<IChartService, ChartService>();
            services.AddScoped<IRelationshipService, RelationshipService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IFieldBusiness, Business.v1.FieldsBusiness>();
            services.AddScoped<IBusiness.IChartBusiness, Business.v1.ChartBusiness>();
            services.AddScoped<IBusiness.IRelationshipBusiness, Business.v1.RelationshipBusiness>();
            services.AddScoped<IBusiness.ITopicBusiness, Business.v1.TopicBusiness>();
            services.AddScoped<IFieldRepository, FieldsRepository>();
            services.AddScoped<IRelationshipsRepository, RelationshipRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IMavimDbDataAccess, DataAccess>();
            services.AddScoped<IFieldMapper, FieldMapper>();
            services.AddScoped<FieldMapperFactory>();

            services.AddMediatR(typeof(Assembly).Assembly);

            services.AddCommands();

            //Remove the ProblemDetails
            services.AddMvc()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = true;
                });
        }

        /// <summary>
        /// Configure API
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (_isDevelopment)
                app.UseDeveloperExceptionPage();
            else
                app.UseAzureAppConfiguration();

            app.UseRouting();
            app.AddSecurity(_isDevelopment);
            app.UseSwaggerAndSwaggerUi(provider);
            app.AddExceptionHandler(_isDevelopment);

            app.UseAuth()
               .UseMavimDatabaseMiddleware(env.IsDevelopment())
               .UseAuthorizationTopic();

            app.UseDataLanguage();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
