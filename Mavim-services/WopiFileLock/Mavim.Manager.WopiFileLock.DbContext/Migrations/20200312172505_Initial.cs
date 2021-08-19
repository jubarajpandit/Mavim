using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mavim.Manager.WopiFileLock.DbContext.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WopiFileLockStates",
                columns: table => new
                {
                    DbId = table.Column<Guid>(nullable: false),
                    DcvId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    Expires = table.Column<DateTime>(nullable: false),
                    Version = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WopiFileLockStates", x => new { x.DbId, x.DcvId, x.UserId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WopiFileLockStates");
        }
    }
}
