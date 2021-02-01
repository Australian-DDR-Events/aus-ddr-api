using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AusDdrApi.Migrations
{
    public partial class Addimageurltoscoreentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 1, 22, 30, 16, 592, DateTimeKind.Utc).AddTicks(760),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 1, 31, 12, 5, 47, 459, DateTimeKind.Utc).AddTicks(8480));

            migrationBuilder.AddColumn<string>(
                name: "ScoreUrl",
                table: "Scores",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoreUrl",
                table: "Scores");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 1, 31, 12, 5, 47, 459, DateTimeKind.Utc).AddTicks(8480),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 1, 22, 30, 16, 592, DateTimeKind.Utc).AddTicks(760));
        }
    }
}
