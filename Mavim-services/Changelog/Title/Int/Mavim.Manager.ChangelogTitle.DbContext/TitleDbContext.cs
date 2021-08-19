using Mavim.Manager.ChangelogTitle.DbModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Mavim.Manager.ChangelogTitle.DbContext
{
    public class TitleDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly string _connectionString;
        private const string AzConfigConnectionString = "Mavim:ChangelogTitleSettings";

        /// <summary>
        /// Gets or sets the changelogs.
        /// </summary>
        /// <value>
        /// The changelogs.
        /// </value>
        public DbSet<Title> Titles { get; set; }

        // only used to use the in memory database for testing purposes
        public TitleDbContext(DbContextOptions<TitleDbContext> options)
        : base(options)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleDbContext"/> class.
        /// </summary>
        public TitleDbContext()
        {
            IConfigurationSection configSection = GetConfiguration().GetSection(AzConfigConnectionString);
            TitleConnectionSettings changelogTitleConnection = configSection.Get<TitleConnectionSettings>();
            _connectionString = changelogTitleConnection.ConnectionString;
        }

        public TitleDbContext(DbContextOptions<TitleDbContext> options,
            IConfiguration configuration) : base(options)
        {
            TitleConnectionSettings changelogTitleConnection = configuration.GetSection(AzConfigConnectionString).Get<TitleConnectionSettings>();
            _connectionString = changelogTitleConnection.ConnectionString;
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

            modelBuilder.Entity<Title>()
                .HasKey(c => c.ChangelogId);

            modelBuilder.Entity<Title>()
                .Property(c => c.Status)
                .HasConversion<int>();

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        private IConfigurationRoot GetConfiguration()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<TitleDbContext>()
                .Build();
            return config;
        }
    }
}