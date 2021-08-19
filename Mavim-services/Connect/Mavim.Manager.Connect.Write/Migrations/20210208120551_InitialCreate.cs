using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mavim.Manager.Connect.Write.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "EventSourcings",
                table => new
                {
                    AggregateId = table.Column<int>("int", nullable: false),
                    EntityId = table.Column<Guid>("uniqueidentifier", nullable: false),
                    EventType = table.Column<int>("int", nullable: false),
                    EntityType = table.Column<int>("int", nullable: false),
                    EntityModelVersion = table.Column<int>("int", nullable: false),
                    Payload = table.Column<string>("nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>("datetime2", nullable: false),
                    CompanyId = table.Column<Guid>("uniqueidentifier", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_EventSourcings", x => new { x.EntityId, x.AggregateId }); });

            migrationBuilder.CreateIndex(
                "IX_EventSourcings_CompanyId",
                "EventSourcings",
                "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "EventSourcings");
        }
    }
}