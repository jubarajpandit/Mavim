using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mavim.Manager.ChangelogField.DbContext.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: false),
                    DatabaseId = table.Column<Guid>(nullable: false),
                    DataLanguage = table.Column<int>(nullable: false),
                    InitiatorEmail = table.Column<string>(nullable: true),
                    ReviewerEmail = table.Column<string>(nullable: true),
                    TimestampChanged = table.Column<DateTime>(nullable: false),
                    TimestampReviewed = table.Column<DateTime>(nullable: true),
                    TopicId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    FieldSetId = table.Column<string>(nullable: true),
                    FieldId = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    OldFieldValue = table.Column<string>(nullable: true),
                    NewFieldValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fields");
        }
    }
}
