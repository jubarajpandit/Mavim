﻿// <auto-generated />
using System;
using Mavim.Manager.Authorization.Read.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mavim.Manager.Authorization.Read.Migrations
{
    [DbContext(typeof(AuthorizationDatabaseContext))]
    [Migration("20210621125317_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mavim.Manager.Authorization.Read.Databases.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AggregateId")
                        .HasColumnType("int");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("Groups")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TopicPermissions")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Mavim.Manager.Authorization.Read.Databases.Models.TopicPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AggregateId")
                        .HasColumnType("int");

                    b.Property<Guid>("Database")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Permissions")
                        .HasColumnType("int");

                    b.Property<string>("TopicID")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TopicPermissions");
                });
#pragma warning restore 612, 618
        }
    }
}