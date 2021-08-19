using Mavim.Manager.Connect.Read.Databases.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Mavim.Manager.Connect.Read.Databases
{
    public class ConnectDatabaseContext : DbContext
    {
        public ConnectDatabaseContext() : base() { }
        public ConnectDatabaseContext(DbContextOptions<ConnectDatabaseContext> options) : base(options) { }

        public DbSet<DiscoveryUser> DiscoveryUsers { get; set; }
        public DbSet<UserTable> Users { get; set; }
        public DbSet<GroupTable> Groups { get; set; }
        public DbSet<CompanyTable> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Entity<DiscoveryUser>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<UserTable>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<GroupTable>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<CompanyTable>()
                .HasKey(e => e.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}