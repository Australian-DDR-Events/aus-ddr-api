using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddEventToScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 8, 9, 9, 5, 17, 763, DateTimeKind.Utc).AddTicks(7320),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 7, 24, 14, 32, 38, 455, DateTimeKind.Utc).AddTicks(380));

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Scores",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scores_EventId",
                table: "Scores",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Events_EventId",
                table: "Scores",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Events_EventId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_EventId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Scores");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 24, 14, 32, 38, 455, DateTimeKind.Utc).AddTicks(380),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 8, 9, 9, 5, 17, 763, DateTimeKind.Utc).AddTicks(7320));
        }
    }
}
