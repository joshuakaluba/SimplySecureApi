using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplySecureApi.Data.Migrations
{
    public partial class ModuleStateChangesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModuleStateChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    State = table.Column<bool>(nullable: false),
                    ModuleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleStateChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleStateChanges_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleStateChanges_ModuleId",
                table: "ModuleStateChanges",
                column: "ModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleStateChanges");
        }
    }
}
