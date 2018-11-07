using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplySecureApi.Data.Migrations
{
    public partial class AddedTriggeredAndArmed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Armed",
                table: "Modules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Triggered",
                table: "Modules",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Armed",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Triggered",
                table: "Modules");
        }
    }
}
