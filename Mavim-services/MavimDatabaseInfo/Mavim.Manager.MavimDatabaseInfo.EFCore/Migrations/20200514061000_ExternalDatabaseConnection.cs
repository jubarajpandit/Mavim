using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mavim.Manager.Catalog.EFCore.Migrations
{
    public partial class ExternalDatabaseConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionProvider",
                table: "MavimDatabases");

            migrationBuilder.DropColumn(
                name: "ConnectionProviderType",
                table: "MavimDatabases");

            migrationBuilder.AlterColumn<string>(
                name: "Schema",
                table: "MavimDatabases",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "MavimDatabases",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationSecretKey",
                table: "MavimDatabases",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInternalDatabase",
                table: "MavimDatabases",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "MavimDatabases");

            migrationBuilder.DropColumn(
                name: "ApplicationSecretKey",
                table: "MavimDatabases");

            migrationBuilder.DropColumn(
                name: "IsInternalDatabase",
                table: "MavimDatabases");

            migrationBuilder.AlterColumn<string>(
                name: "Schema",
                table: "MavimDatabases",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ConnectionProvider",
                table: "MavimDatabases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionProviderType",
                table: "MavimDatabases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
