using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class songdifficulty_to_chart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishSong_SongDifficulties_SongDifficultyId",
                table: "DishSong");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_SongDifficulties_SongDifficultyId",
                table: "Ingredient");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_SongDifficulties_SongDifficultyId",
                table: "Scores");

            migrationBuilder.DropTable(
                name: "CourseSongDifficulty");

            migrationBuilder.DropTable(
                name: "EventSongDifficulty");

            migrationBuilder.DropTable(
                name: "SongDifficulties");

            migrationBuilder.RenameColumn(
                name: "SongDifficultyId",
                table: "Scores",
                newName: "ChartId");

            migrationBuilder.RenameIndex(
                name: "IX_Scores_SongDifficultyId",
                table: "Scores",
                newName: "IX_Scores_ChartId");

            migrationBuilder.RenameColumn(
                name: "SongDifficultyId",
                table: "Ingredient",
                newName: "ChartId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredient_SongDifficultyId",
                table: "Ingredient",
                newName: "IX_Ingredient_ChartId");

            migrationBuilder.RenameColumn(
                name: "SongDifficultyId",
                table: "DishSong",
                newName: "ChartId");

            migrationBuilder.RenameIndex(
                name: "IX_DishSong_SongDifficultyId",
                table: "DishSong",
                newName: "IX_DishSong_ChartId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 4, 14, 22, 58, 120, DateTimeKind.Utc).AddTicks(1720),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 9, 5, 14, 25, 46, 772, DateTimeKind.Utc).AddTicks(4120));

            migrationBuilder.CreateTable(
                name: "Charts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayMode = table.Column<string>(type: "text", nullable: false),
                    Difficulty = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    MaxScore = table.Column<int>(type: "integer", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Charts_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChartCourse",
                columns: table => new
                {
                    ChartsId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoursesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartCourse", x => new { x.ChartsId, x.CoursesId });
                    table.ForeignKey(
                        name: "FK_ChartCourse_Charts_ChartsId",
                        column: x => x.ChartsId,
                        principalTable: "Charts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChartCourse_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChartEvent",
                columns: table => new
                {
                    ChartsId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartEvent", x => new { x.ChartsId, x.EventsId });
                    table.ForeignKey(
                        name: "FK_ChartEvent_Charts_ChartsId",
                        column: x => x.ChartsId,
                        principalTable: "Charts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChartEvent_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChartCourse_CoursesId",
                table: "ChartCourse",
                column: "CoursesId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartEvent_EventsId",
                table: "ChartEvent",
                column: "EventsId");

            migrationBuilder.CreateIndex(
                name: "IX_Charts_SongId",
                table: "Charts",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishSong_Charts_ChartId",
                table: "DishSong",
                column: "ChartId",
                principalTable: "Charts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_Charts_ChartId",
                table: "Ingredient",
                column: "ChartId",
                principalTable: "Charts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Charts_ChartId",
                table: "Scores",
                column: "ChartId",
                principalTable: "Charts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishSong_Charts_ChartId",
                table: "DishSong");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_Charts_ChartId",
                table: "Ingredient");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Charts_ChartId",
                table: "Scores");

            migrationBuilder.DropTable(
                name: "ChartCourse");

            migrationBuilder.DropTable(
                name: "ChartEvent");

            migrationBuilder.DropTable(
                name: "Charts");

            migrationBuilder.RenameColumn(
                name: "ChartId",
                table: "Scores",
                newName: "SongDifficultyId");

            migrationBuilder.RenameIndex(
                name: "IX_Scores_ChartId",
                table: "Scores",
                newName: "IX_Scores_SongDifficultyId");

            migrationBuilder.RenameColumn(
                name: "ChartId",
                table: "Ingredient",
                newName: "SongDifficultyId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredient_ChartId",
                table: "Ingredient",
                newName: "IX_Ingredient_SongDifficultyId");

            migrationBuilder.RenameColumn(
                name: "ChartId",
                table: "DishSong",
                newName: "SongDifficultyId");

            migrationBuilder.RenameIndex(
                name: "IX_DishSong_ChartId",
                table: "DishSong",
                newName: "IX_DishSong_SongDifficultyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 5, 14, 25, 46, 772, DateTimeKind.Utc).AddTicks(4120),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 5, 4, 14, 22, 58, 120, DateTimeKind.Utc).AddTicks(1720));

            migrationBuilder.CreateTable(
                name: "SongDifficulties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false),
                    Difficulty = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    MaxScore = table.Column<int>(type: "integer", nullable: false),
                    PlayMode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongDifficulties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SongDifficulties_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseSongDifficulty",
                columns: table => new
                {
                    CoursesId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongDifficultiesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSongDifficulty", x => new { x.CoursesId, x.SongDifficultiesId });
                    table.ForeignKey(
                        name: "FK_CourseSongDifficulty_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSongDifficulty_SongDifficulties_SongDifficultiesId",
                        column: x => x.SongDifficultiesId,
                        principalTable: "SongDifficulties",
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
                name: "IX_CourseSongDifficulty_SongDifficultiesId",
                table: "CourseSongDifficulty",
                column: "SongDifficultiesId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSongDifficulty_SongDifficultiesId",
                table: "EventSongDifficulty",
                column: "SongDifficultiesId");

            migrationBuilder.CreateIndex(
                name: "IX_SongDifficulties_SongId",
                table: "SongDifficulties",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishSong_SongDifficulties_SongDifficultyId",
                table: "DishSong",
                column: "SongDifficultyId",
                principalTable: "SongDifficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_SongDifficulties_SongDifficultyId",
                table: "Ingredient",
                column: "SongDifficultyId",
                principalTable: "SongDifficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_SongDifficulties_SongDifficultyId",
                table: "Scores",
                column: "SongDifficultyId",
                principalTable: "SongDifficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
