using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class score_validation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 29, 6, 3, 1, 460, DateTimeKind.Utc).AddTicks(9560),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 5, 4, 14, 22, 58, 120, DateTimeKind.Utc).AddTicks(1720));

            migrationBuilder.AddColumn<bool>(
                name: "Validated",
                table: "Scores",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Validated",
                table: "Scores");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 4, 14, 22, 58, 120, DateTimeKind.Utc).AddTicks(1720),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 5, 29, 6, 3, 1, 460, DateTimeKind.Utc).AddTicks(9560));
        }
    }
}
