using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Mavim.Manager.ChangelogField.DbContext
{
    public class FieldDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly string _connectionString;
        private const string AzConfigConnectionString = "Mavim:ChangelogFieldSettings";

        /// <summary>
        /// Gets or sets the Fields.
        /// </summary>
        /// <value>
        /// The changelogs.
        /// </value>
        public DbSet<DbModel.ChangelogField> Fields { get; set; }

        /// <summary>Initializes a new instance of the <see cref="FieldDbContext"/> class.</summary>
        public FieldDbContext()
        {
            FieldConnectionSettings changelogFieldConnection = GetConfiguration().GetSection(AzConfigConnectionString).Get<FieldConnectionSettings>();
            _connectionString = changelogFieldConnection.ConnectionString;
        }

        public FieldDbContext(DbContextOptions<FieldDbContext> options) : base(options)
        { }

        public FieldDbContext(DbContextOptions<FieldDbContext> options,
            IConfiguration configuration) : base(options)
        {
            FieldConnectionSettings changelogFieldConnection = configuration.GetSection(AzConfigConnectionString).Get<FieldConnectionSettings>();
            _connectionString = changelogFieldConnection.ConnectionString;
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

            modelBuilder.Entity<DbModel.ChangelogField>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<DbModel.ChangelogField>()
                .Property(c => c.DataLanguage)
                .HasConversion<int>();


            modelBuilder.Entity<DbModel.ChangelogField>()
                .Property(c => c.Status)
                .HasConversion<int>();

            modelBuilder.Entity<DbModel.ChangelogField>()
                .Property(c => c.Type)
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
                .AddUserSecrets<FieldDbContext>()
                .Build();
            return config;
        }
    }
}