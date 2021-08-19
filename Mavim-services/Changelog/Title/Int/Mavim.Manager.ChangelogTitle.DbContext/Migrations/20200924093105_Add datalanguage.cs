using Microsoft.EntityFrameworkCore.Migrations;

namespace Mavim.Manager.ChangelogTitle.DbContext.Migrations
{
    public partial class Adddatalanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataLanguage",
                table: "Titles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataLanguage",
                table: "Titles");
        }
    }
}
