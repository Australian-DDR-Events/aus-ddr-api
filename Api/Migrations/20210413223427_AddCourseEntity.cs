using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AusDdrApi.Migrations
{
    public partial class AddCourseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 4, 13, 22, 34, 27, 208, DateTimeKind.Utc).AddTicks(8590),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 3, 11, 12, 7, 32, 339, DateTimeKind.Utc).AddTicks(9750));

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseSong",
                columns: table => new
                {
                    CoursesId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSong", x => new { x.CoursesId, x.SongsId });
                    table.ForeignKey(
                        name: "FK_CourseSong_Course_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSong_Songs_SongsId",
                        column: x => x.SongsId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSong_SongsId",
                table: "CourseSong",
                column: "SongsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseSong");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 11, 12, 7, 32, 339, DateTimeKind.Utc).AddTicks(9750),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 4, 13, 22, 34, 27, 208, DateTimeKind.Utc).AddTicks(8590));
        }
    }
}
