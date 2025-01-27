﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Mavim.Manager.ChLog.Relationship.DbContext.Migrations
{
    [DbContext(typeof(RelationDbContext))]
    partial class RelationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mavim.Manager.ChangelogRelation.DbModel.Relation", b =>
                {
                    b.Property<Guid>("ChangelogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Action")
                        .HasColumnType("int");

                    b.Property<string>("ReviewerUserEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DataLanguage")
                        .HasColumnType("int");

                    b.Property<Guid>("DatabaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OldCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldTopicId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InitiatorUserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("TimestampApproved")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimestampChanged")
                        .HasColumnType("datetime2");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToTopicId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TopicId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ChangelogId");

                    b.ToTable("Relations");
                });
#pragma warning restore 612, 618
        }
    }
}
