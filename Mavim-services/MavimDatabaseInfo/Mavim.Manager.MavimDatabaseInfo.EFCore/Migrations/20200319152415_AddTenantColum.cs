using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mavim.Manager.Catalog.EFCore.Migrations
{
    public partial class AddTenantColum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "MavimDatabases",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "MavimDatabases");
        }
    }
}
