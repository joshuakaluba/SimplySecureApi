using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplySecureApi.Data.Migrations
{
    public partial class ModuleName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Name",
                table: "Modules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ArmedModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    State = table.Column<bool>(nullable: false),
                    ModuleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmedModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArmedModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TriggeredModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    State = table.Column<bool>(nullable: false),
                    ModuleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggeredModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TriggeredModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArmedModules_ModuleId",
                table: "ArmedModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TriggeredModules_ModuleId",
                table: "TriggeredModules",
                column: "ModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArmedModules");

            migrationBuilder.DropTable(
                name: "TriggeredModules");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Modules");
        }
    }
}
