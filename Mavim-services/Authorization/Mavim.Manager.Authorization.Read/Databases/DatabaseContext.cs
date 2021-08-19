using Mavim.Manager.Authorization.Read.Databases.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Mavim.Manager.Authorization.Read.Databases
{

    public class AuthorizationDatabaseContext : DbContext
    {
        public AuthorizationDatabaseContext() : base() { }
        public AuthorizationDatabaseContext(DbContextOptions<AuthorizationDatabaseContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<TopicPermission> TopicPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Entity<Role>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Role>()
            .Property(e => e.Groups)
            .HasConversion(
                v => string.Join(',', v.Select(s => s.ToString())),
                v => v.Split(',', StringSplitOptions.None).Select(s => Guid.Parse(s)).ToArray());

            modelBuilder.Entity<Role>()
            .Property(e => e.TopicPermissions)
            .HasConversion(
                v => string.Join(',', v.Select(s => s.ToString())),
                v => v.Split(',', StringSplitOptions.None).Select(s => Guid.Parse(s)).ToArray());

            modelBuilder.Entity<TopicPermission>()
                .HasKey(e => e.Id);

            base.OnModelCreating(modelBuilder);
        }

    }

}