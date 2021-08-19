using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mavim.Manager.ChangelogTitle.DbContext.Migrations
{
    public partial class Initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    ChangelogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(nullable: false),
                    DatabaseId = table.Column<Guid>(nullable: false),
                    InitiatorUserEmail = table.Column<string>(nullable: true),
                    ReviewerUserEmail = table.Column<string>(nullable: true),
                    TimestampChanged = table.Column<DateTime>(nullable: false),
                    TimestampApproved = table.Column<DateTime>(nullable: true),
                    TopicDcv = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    FromTitleValue = table.Column<string>(nullable: true),
                    ToTitleValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.ChangelogId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Titles");
        }
    }
}
