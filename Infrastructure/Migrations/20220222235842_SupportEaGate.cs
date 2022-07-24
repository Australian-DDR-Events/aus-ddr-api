using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class SupportEaGate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KonamiId",
                table: "Songs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 22, 23, 58, 42, 457, DateTimeKind.Utc).AddTicks(5010),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2021, 6, 9, 8, 29, 6, 807, DateTimeKind.Utc).AddTicks(9110));

            migrationBuilder.AddColumn<int>(
                name: "ExScore",
                table: "Scores",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KonamiId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "ExScore",
                table: "Scores");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 6, 9, 8, 29, 6, 807, DateTimeKind.Utc).AddTicks(9110),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 2, 22, 23, 58, 42, 457, DateTimeKind.Utc).AddTicks(5010));
        }
    }
}
