using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AusDdrApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dancers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthenticationId = table.Column<string>(type: "text", nullable: false),
                    DdrName = table.Column<string>(type: "text", nullable: false),
                    DdrCode = table.Column<string>(type: "text", nullable: false),
                    PrimaryMachineLocation = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    ProfilePictureTimestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dancers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MaxScore = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Artist = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradedDishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Grade = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DishId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradedDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradedDishes_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "SongDifficulties",
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
                    table.PrimaryKey("PK_SongDifficulties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SongDifficulties_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradedDancerDishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GradedDishId = table.Column<Guid>(type: "uuid", nullable: false),
                    DancerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradedDancerDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradedDancerDishes_Dancers_DancerId",
                        column: x => x.DancerId,
                        principalTable: "Dancers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradedDancerDishes_GradedDishes_GradedDishId",
                        column: x => x.GradedDishId,
                        principalTable: "GradedDishes",
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
                name: "DishSongs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CookingOrder = table.Column<int>(type: "integer", nullable: false),
                    CookingMethod = table.Column<string>(type: "text", nullable: false),
                    DishId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongDifficultyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishSongs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishSongs_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishSongs_SongDifficulties_SongDifficultyId",
                        column: x => x.SongDifficultyId,
                        principalTable: "SongDifficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SongDifficultyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_SongDifficulties_SongDifficultyId",
                        column: x => x.SongDifficultyId,
                        principalTable: "SongDifficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    SubmissionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 4, 18, 2, 49, 37, 946, DateTimeKind.Utc).AddTicks(8460)),
                    DancerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongDifficultyId = table.Column<Guid>(type: "uuid", nullable: false),
                    GradedDancerDishId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_Dancers_DancerId",
                        column: x => x.DancerId,
                        principalTable: "Dancers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scores_GradedDancerDishes_GradedDancerDishId",
                        column: x => x.GradedDancerDishId,
                        principalTable: "GradedDancerDishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scores_SongDifficulties_SongDifficultyId",
                        column: x => x.SongDifficultyId,
                        principalTable: "SongDifficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishIngredient",
                columns: table => new
                {
                    DishesId = table.Column<Guid>(type: "uuid", nullable: false),
                    IngredientsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishIngredient", x => new { x.DishesId, x.IngredientsId });
                    table.ForeignKey(
                        name: "FK_DishIngredient_Dishes_DishesId",
                        column: x => x.DishesId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishIngredient_Ingredients_IngredientsId",
                        column: x => x.IngredientsId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradedIngredients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Grade = table.Column<string>(type: "text", nullable: false),
                    RequiredScore = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IngredientId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradedIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradedIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradedDancerIngredients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GradedIngredientId = table.Column<Guid>(type: "uuid", nullable: false),
                    DancerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScoreId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradedDancerIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradedDancerIngredients_Dancers_DancerId",
                        column: x => x.DancerId,
                        principalTable: "Dancers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradedDancerIngredients_GradedIngredients_GradedIngredientId",
                        column: x => x.GradedIngredientId,
                        principalTable: "GradedIngredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradedDancerIngredients_Scores_ScoreId",
                        column: x => x.ScoreId,
                        principalTable: "Scores",
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

            migrationBuilder.CreateIndex(
                name: "IX_BadgeThresholds_BadgeId1",
                table: "BadgeThresholds",
                column: "BadgeId1");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSongDifficulty_SongDifficultiesId",
                table: "CourseSongDifficulty",
                column: "SongDifficultiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Dancers_AuthenticationId",
                table: "Dancers",
                column: "AuthenticationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DishIngredient_IngredientsId",
                table: "DishIngredient",
                column: "IngredientsId");

            migrationBuilder.CreateIndex(
                name: "IX_DishSongs_DishId",
                table: "DishSongs",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_DishSongs_SongDifficultyId",
                table: "DishSongs",
                column: "SongDifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_GradedDancerDishes_DancerId",
                table: "GradedDancerDishes",
                column: "DancerId");

            migrationBuilder.CreateIndex(
                name: "IX_GradedDancerDishes_GradedDishId",
                table: "GradedDancerDishes",
                column: "GradedDishId");

            migrationBuilder.CreateIndex(
                name: "IX_GradedDancerIngredients_DancerId",
                table: "GradedDancerIngredients",
                column: "DancerId");

            migrationBuilder.CreateIndex(
                name: "IX_GradedDancerIngredients_GradedIngredientId",
                table: "GradedDancerIngredients",
                column: "GradedIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_GradedDancerIngredients_ScoreId",
                table: "GradedDancerIngredients",
                column: "ScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_GradedDishes_DishId",
                table: "GradedDishes",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_GradedIngredients_IngredientId",
                table: "GradedIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_SongDifficultyId",
                table: "Ingredients",
                column: "SongDifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_DancerId",
                table: "Scores",
                column: "DancerId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_GradedDancerDishId",
                table: "Scores",
                column: "GradedDancerDishId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_SongDifficultyId",
                table: "Scores",
                column: "SongDifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_SongDifficulties_SongId",
                table: "SongDifficulties",
                column: "SongId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BadgeDancer");

            migrationBuilder.DropTable(
                name: "BadgeThresholds");

            migrationBuilder.DropTable(
                name: "CourseSongDifficulty");

            migrationBuilder.DropTable(
                name: "DishIngredient");

            migrationBuilder.DropTable(
                name: "DishSongs");

            migrationBuilder.DropTable(
                name: "GradedDancerIngredients");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "GradedIngredients");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "GradedDancerDishes");

            migrationBuilder.DropTable(
                name: "SongDifficulties");

            migrationBuilder.DropTable(
                name: "Dancers");

            migrationBuilder.DropTable(
                name: "GradedDishes");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Dishes");
        }
    }
}
