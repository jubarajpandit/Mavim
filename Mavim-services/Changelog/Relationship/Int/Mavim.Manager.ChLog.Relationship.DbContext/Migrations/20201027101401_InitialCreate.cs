using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mavim.Manager.ChLog.Relationship.DbContext.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Relations",
                columns: table => new
                {
                    ChangelogId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: false),
                    DatabaseId = table.Column<Guid>(nullable: false),
                    DataLanguage = table.Column<int>(nullable: false),
                    InitiatorUserEmail = table.Column<string>(nullable: false),
                    ReviewerUserEmail = table.Column<string>(nullable: true),
                    TimestampChanged = table.Column<DateTime>(nullable: false),
                    TimestampApproved = table.Column<DateTime>(nullable: true),
                    TopicId = table.Column<string>(nullable: true),
                    Action = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    RelationId = table.Column<string>(nullable: true),
                    OldCategory = table.Column<string>(nullable: true),
                    OldTopicId = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    ToTopicId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relations", x => x.ChangelogId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relations");
        }
    }
}
