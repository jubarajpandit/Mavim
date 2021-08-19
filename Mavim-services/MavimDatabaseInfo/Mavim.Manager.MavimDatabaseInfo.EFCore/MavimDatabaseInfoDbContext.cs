using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Mavim.Manager.MavimDatabaseInfo.EFCore
{
    public class MavimDatabaseInfoDbContext : DbContext
    {
        private readonly string _mavimDatabaseInfoSettings = "Mavim:DatabaseInfoSettings:ConnectionString";
        private readonly string _connectionString;

        private readonly ILogger<MavimDatabaseInfoDbContext> _logger;

        public DbSet<Models.DbConnectionInfo> MavimDatabases { get; set; }

        public MavimDatabaseInfoDbContext(ILogger<MavimDatabaseInfoDbContext> logger, DbContextOptions<MavimDatabaseInfoDbContext> options) : base(options) { }

        /// <summary>Initializes a new instance of the <see cref="MavimDatabaseInfoDbContext"/> class.</summary>
        public MavimDatabaseInfoDbContext(ILogger<MavimDatabaseInfoDbContext> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            string connectionString = GetConfiguration().GetSection(_mavimDatabaseInfoSettings).Value;
            _connectionString = connectionString;
        }

        public MavimDatabaseInfoDbContext(ILogger<MavimDatabaseInfoDbContext> logger, IOptionsSnapshot<MavimDatabaseInfoConnectionSettings> authConfigSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //This is only for development environment for applying the migrations from the local machine by reading the configuration from the user secrets
            _connectionString = authConfigSettings.Value.ConnectionString;
            _logger.LogInformation($"Connectionstring: {_connectionString}");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="authConfigSettings">The authentication configuration settings.</param>
        /// <exception cref="ArgumentNullException">connectionString</exception>
        public MavimDatabaseInfoDbContext(
            ILogger<MavimDatabaseInfoDbContext> logger,
            DbContextOptions<MavimDatabaseInfoDbContext> options,
            IOptionsSnapshot<MavimDatabaseInfoConnectionSettings> authConfigSettings) : base(options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionString = authConfigSettings.Value.ConnectionString;
            _logger.LogInformation($"Connectionstring: {_connectionString}");
        }

        /// <summary>
        ///   <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </para>
        ///   <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions"/> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured"/> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)"/>.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            optionsBuilder.UseSqlServer(_connectionString);
        }

        /// <summary>Gets the configuration.</summary>
        /// <returns></returns>
        private IConfigurationRoot GetConfiguration()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<MavimDatabaseInfoDbContext>()
                .Build();
            return config;
        }
    }
}
