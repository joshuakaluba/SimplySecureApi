using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplySecureApi.Data.Migrations
{
    public partial class LocationsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Armed",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "IsSilentAlarm",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Triggered",
                table: "Modules");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Modules",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Armed = table.Column<bool>(nullable: false),
                    IsSilentAlarm = table.Column<bool>(nullable: false),
                    Triggered = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_LocationId",
                table: "Modules",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Locations_LocationId",
                table: "Modules",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Locations_LocationId",
                table: "Modules");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Modules_LocationId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Modules");

            migrationBuilder.AddColumn<bool>(
                name: "Armed",
                table: "Modules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSilentAlarm",
                table: "Modules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Triggered",
                table: "Modules",
                nullable: false,
                defaultValue: false);
        }
    }
}
