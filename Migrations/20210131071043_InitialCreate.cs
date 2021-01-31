using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AusDdrApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dancers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthenticationId = table.Column<string>(type: "text", nullable: true),
                    DdrName = table.Column<string>(type: "text", nullable: true),
                    DdrCode = table.Column<string>(type: "text", nullable: true),
                    PrimaryMachineLocation = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dancers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dancers_AuthenticationId",
                table: "Dancers",
                column: "AuthenticationId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dancers");
        }
    }
}
