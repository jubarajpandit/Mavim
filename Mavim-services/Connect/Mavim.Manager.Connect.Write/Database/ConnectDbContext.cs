using Mavim.Manager.Connect.Write.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Mavim.Manager.Connect.Write.Database
{
    public class ConnectDbContext : DbContext
    {
        public DbSet<EventSourcingModel> EventSourcings { get; set; }

        public ConnectDbContext() { }

        public ConnectDbContext(DbContextOptions<ConnectDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Entity<EventSourcingModel>()
                .HasKey(es => new { es.EntityId, es.AggregateId });

            modelBuilder.Entity<EventSourcingModel>()
                .HasIndex(es => es.CompanyId);

            base.OnModelCreating(modelBuilder);
        }
    }
}