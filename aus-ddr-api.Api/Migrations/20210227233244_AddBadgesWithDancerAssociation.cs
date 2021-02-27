using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AusDdrApi.Migrations
{
    public partial class AddBadgesWithDancerAssociation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 27, 23, 32, 43, 679, DateTimeKind.Utc).AddTicks(5510),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 22, 7, 45, 28, 385, DateTimeKind.Utc).AddTicks(8860));

            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Badges_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BadgeDancer",
                columns: table => new
                {
                    BadgesId = table.Column<Guid>(type: "uuid", nullable: false),
                    DancersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeDancer", x => new { x.BadgesId, x.DancersId });
                    table.ForeignKey(
                        name: "FK_BadgeDancer_Badges_BadgesId",
                        column: x => x.BadgesId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BadgeDancer_Dancers_DancersId",
                        column: x => x.DancersId,
                        principalTable: "Dancers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BadgeDancer_DancersId",
                table: "BadgeDancer",
                column: "DancersId");

            migrationBuilder.CreateIndex(
                name: "IX_Badges_EventId",
                table: "Badges",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BadgeDancer");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 22, 7, 45, 28, 385, DateTimeKind.Utc).AddTicks(8860),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 27, 23, 32, 43, 679, DateTimeKind.Utc).AddTicks(5510));
        }
    }
}
