using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class CreateEventSongRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 28, 4, 47, 29, 615, DateTimeKind.Utc).AddTicks(7480),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 2, 22, 23, 58, 42, 457, DateTimeKind.Utc).AddTicks(5010));

            migrationBuilder.CreateTable(
                name: "CourseEvent",
                columns: table => new
                {
                    CoursesId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEvent", x => new { x.CoursesId, x.EventsId });
                    table.ForeignKey(
                        name: "FK_CourseEvent_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEvent_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventSongDifficulty",
                columns: table => new
                {
                    EventsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongDifficultiesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSongDifficulty", x => new { x.EventsId, x.SongDifficultiesId });
                    table.ForeignKey(
                        name: "FK_EventSongDifficulty_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSongDifficulty_SongDifficulties_SongDifficultiesId",
                        column: x => x.SongDifficultiesId,
                        principalTable: "SongDifficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseEvent_EventsId",
                table: "CourseEvent",
                column: "EventsId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSongDifficulty_SongDifficultiesId",
                table: "EventSongDifficulty",
                column: "SongDifficultiesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseEvent");

            migrationBuilder.DropTable(
                name: "EventSongDifficulty");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 22, 23, 58, 42, 457, DateTimeKind.Utc).AddTicks(5010),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 2, 28, 4, 47, 29, 615, DateTimeKind.Utc).AddTicks(7480));
        }
    }
}
