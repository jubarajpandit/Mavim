﻿// <auto-generated />
using System;
using Mavim.Manager.WopiFileLock.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mavim.Manager.WopiFileLock.DbContext.Migrations
{
    [DbContext(typeof(WopiFileLockStateDbContext))]
    [Migration("20200312172505_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mavim.Manager.WopiFileLock.Model.WopiFileLockState", b =>
                {
                    b.Property<Guid>("DbId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DcvId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DbId", "DcvId", "UserId");

                    b.ToTable("WopiFileLockStates");
                });
#pragma warning restore 612, 618
        }
    }
}
