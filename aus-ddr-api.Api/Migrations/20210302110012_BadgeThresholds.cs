using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AusDdrApi.Migrations
{
    public partial class BadgeThresholds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 2, 11, 0, 12, 97, DateTimeKind.Utc).AddTicks(5900),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 3, 1, 12, 53, 32, 420, DateTimeKind.Utc).AddTicks(1270));

            migrationBuilder.CreateTable(
                name: "BadgeThresholds",
                columns: table => new
                {
                    BadgeId = table.Column<Guid>(type: "uuid", nullable: false),
                    BadgeId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    RequiredPoints = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeThresholds", x => x.BadgeId);
                    table.ForeignKey(
                        name: "FK_BadgeThresholds_Badges_BadgeId1",
                        column: x => x.BadgeId1,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BadgeThresholds_BadgeId1",
                table: "BadgeThresholds",
                column: "BadgeId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BadgeThresholds");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 1, 12, 53, 32, 420, DateTimeKind.Utc).AddTicks(1270),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 3, 2, 11, 0, 12, 97, DateTimeKind.Utc).AddTicks(5900));
        }
    }
}
