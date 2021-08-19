using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mavim.Manager.Catalog.EFCore.Migrations
{
    public partial class ApplicationTenantID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationTenantId",
                table: "MavimDatabases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationTenantId",
                table: "MavimDatabases");
        }
    }
}
