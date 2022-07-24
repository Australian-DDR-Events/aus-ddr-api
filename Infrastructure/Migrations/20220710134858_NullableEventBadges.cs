using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class NullableEventBadges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badges_Events_EventId",
                table: "Badges");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 10, 13, 48, 57, 842, DateTimeKind.Utc).AddTicks(7570),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 2, 28, 4, 47, 29, 615, DateTimeKind.Utc).AddTicks(7480));

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "Badges",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_Events_EventId",
                table: "Badges",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badges_Events_EventId",
                table: "Badges");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 28, 4, 47, 29, 615, DateTimeKind.Utc).AddTicks(7480),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 7, 10, 13, 48, 57, 842, DateTimeKind.Utc).AddTicks(7570));

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "Badges",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_Events_EventId",
                table: "Badges",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
