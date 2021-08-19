using Mavim.Manager.ChLog.Relationship.DbModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Mavim.Manager.ChLog.Relationship.DbContext
{
    public class RelationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly string _connectionString;
        private const string AzConfigConnectionString = "Mavim:ChangelogRelationSettings:ConnectionString";
        private const string AzConfigConnectionStringUP = "Mavim:ChangelogRelationSettingsUP:ConnectionString";

        /// <summary>
        /// Gets or sets the changelogs.
        /// </summary>
        /// <value>
        /// The changelogs.
        /// </value>
        public DbSet<Relation> Relations { get; set; }

        public RelationDbContext(IConfiguration configuration)
        {
            //This is only for development environment for applying the migrations from the local machine by reading the configuration from the user secrets
            string connectionString = configuration.GetSection(AzConfigConnectionStringUP).Value;
            _connectionString = connectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationDbContext"/> class.
        /// </summary>
        public RelationDbContext()
        {
            string connectionString = GetConfiguration().GetSection(AzConfigConnectionString).Value;
            _connectionString = connectionString;
        }

        public RelationDbContext(DbContextOptions<RelationDbContext> options) : base(options)
        { }

        public RelationDbContext(DbContextOptions<RelationDbContext> options,
            IConfiguration configuration) : base(options)
        {
            string connectionString = configuration.GetSection(AzConfigConnectionString).Value;
            _connectionString = connectionString;
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

            optionsBuilder.UseSqlServer(_connectionString);
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

            modelBuilder.Entity<Relation>()
                .HasKey(c => c.ChangelogId);

            modelBuilder.Entity<Relation>()
                .Property(c => c.Status)
                .HasConversion<int>();

            modelBuilder.Entity<Relation>()
                .Property(c => c.InitiatorUserEmail)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        private IConfigurationRoot GetConfiguration()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<RelationDbContext>()
                .Build();
            return config;
        }
    }
}