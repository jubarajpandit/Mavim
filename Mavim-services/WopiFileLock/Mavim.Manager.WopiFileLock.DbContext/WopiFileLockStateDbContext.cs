using Mavim.Manager.WopiFileLock.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Mavim.Manager.WopiFileLock.DbContext
{
    public class WopiFileLockStateDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly string _connectionString;
        private const string AzConfigConnectionString = "Mavim:WopiHostSettings:ConnectionString";

        /// <summary>
        /// Gets or sets the wopi file lock states.
        /// </summary>
        /// <value>
        /// The wopi file lock states.
        /// </value>
        public DbSet<WopiFileLockState> WopiFileLockStates { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiFileLockStateDbContext" /> class.
        /// </summary>
        public WopiFileLockStateDbContext(IConfiguration configuration)
        {
            //This is only for development environment for applying the migrations from the local machine by reading the configuration from the user secrets
            string authorizationConnection = configuration.GetSection(AzConfigConnectionString).Value;
            _connectionString = authorizationConnection;
        }

        public WopiFileLockStateDbContext()
        {
            //This is only for development environment for applying the migrations from the local machine by reading the configuration from the user secrets
            string authorizationConnection = GetConfiguration().GetSection(AzConfigConnectionString).Value;
            _connectionString = authorizationConnection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiFileLockStateDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="wopiConfigSettings">The authentication configuration settings.</param>
        /// <exception cref="ArgumentNullException">connectionString</exception>
        public WopiFileLockStateDbContext(DbContextOptions<WopiFileLockStateDbContext> options, IConfiguration configuration) : base(options)
        {
            string authorizationConnection = GetConfiguration().GetSection(AzConfigConnectionString).Value;
            _connectionString = authorizationConnection;
        }

        /// <summary>
        /// <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </para>
        /// <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null) throw new ArgumentNullException(nameof(optionsBuilder));
            if (optionsBuilder.IsConfigured) return;

            SqlConnection connection = new SqlConnection
            {
                ConnectionString = _connectionString
            };

            optionsBuilder.UseSqlServer(connection);
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));
            // composite key
            modelBuilder.Entity<WopiFileLockState>().HasKey(c => new { c.DbId, c.DcvId, c.UserId });
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        private IConfigurationRoot GetConfiguration()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<WopiFileLockStateDbContext>()
                .Build();
            return config;
        }
    }
}