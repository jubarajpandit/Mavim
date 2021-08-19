﻿// <auto-generated />
using System;
using Mavim.Manager.MavimDatabaseInfo.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mavim.Manager.Catalog.EFCore.Migrations
{
    [DbContext(typeof(MavimDatabaseInfoDbContext))]
    [Migration("20200526121639_ApplicationTenantID")]
    partial class ApplicationTenantID
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mavim.Manager.Catalog.EFCore.Models.DbConnectionInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApplicationSecretKey")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApplicationTenantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConnectionString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsInternalDatabase")
                        .HasColumnType("bit");

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("MavimDatabases");
                });
#pragma warning restore 612, 618
        }
    }
}
