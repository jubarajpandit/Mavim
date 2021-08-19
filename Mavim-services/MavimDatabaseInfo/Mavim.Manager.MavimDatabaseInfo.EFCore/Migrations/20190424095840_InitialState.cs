using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mavim.Manager.Catalog.EFCore.Migrations
{
    public partial class InitialState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MavimDatabases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    DisplayName = table.Column<string>(nullable: false),
                    ConnectionString = table.Column<string>(nullable: false),
                    Schema = table.Column<string>(nullable: true),
                    ConnectionProviderType = table.Column<string>(nullable: false),
                    ConnectionProvider = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MavimDatabases", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MavimDatabases");
        }
    }
}
