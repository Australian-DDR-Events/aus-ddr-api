using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddSessions_RemoveOldEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishIngredient_Dishes_DishesId",
                table: "DishIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_DishIngredient_Ingredients_IngredientsId",
                table: "DishIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_DishSongs_Dishes_DishId",
                table: "DishSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_DishSongs_SongDifficulties_SongDifficultyId",
                table: "DishSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_GradedDishes_Dishes_DishId",
                table: "GradedDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_GradedIngredients_Ingredients_IngredientId",
                table: "GradedIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_SongDifficulties_SongDifficultyId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_GradedDancerDishes_GradedDancerDishId",
                table: "Scores");

            migrationBuilder.DropTable(
                name: "GradedDancerDishes");

            migrationBuilder.DropTable(
                name: "GradedDancerIngredients");

            migrationBuilder.DropIndex(
                name: "IX_Scores_GradedDancerDishId",
                table: "Scores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradedIngredients",
                table: "GradedIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradedDishes",
                table: "GradedDishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishSongs",
                table: "DishSongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dishes",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "GradedDancerDishId",
                table: "Scores");

            migrationBuilder.RenameTable(
                name: "Ingredients",
                newName: "Ingredient");

            migrationBuilder.RenameTable(
                name: "GradedIngredients",
                newName: "GradedIngredient");

            migrationBuilder.RenameTable(
                name: "GradedDishes",
                newName: "GradedDish");

            migrationBuilder.RenameTable(
                name: "DishSongs",
                newName: "DishSong");

            migrationBuilder.RenameTable(
                name: "Dishes",
                newName: "Dish");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_SongDifficultyId",
                table: "Ingredient",
                newName: "IX_Ingredient_SongDifficultyId");

            migrationBuilder.RenameIndex(
                name: "IX_GradedIngredients_IngredientId",
                table: "GradedIngredient",
                newName: "IX_GradedIngredient_IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_GradedDishes_DishId",
                table: "GradedDish",
                newName: "IX_GradedDish_DishId");

            migrationBuilder.RenameIndex(
                name: "IX_DishSongs_SongDifficultyId",
                table: "DishSong",
                newName: "IX_DishSong_SongDifficultyId");

            migrationBuilder.RenameIndex(
                name: "IX_DishSongs_DishId",
                table: "DishSong",
                newName: "IX_DishSong_DishId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 5, 14, 25, 46, 772, DateTimeKind.Utc).AddTicks(4120),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 9, 1, 11, 38, 6, 103, DateTimeKind.Utc).AddTicks(4680));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradedIngredient",
                table: "GradedIngredient",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradedDish",
                table: "GradedDish",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishSong",
                table: "DishSong",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dish",
                table: "Dish",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Cookie = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    Expiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DancerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Cookie);
                    table.ForeignKey(
                        name: "FK_Sessions_Dancers_DancerId",
                        column: x => x.DancerId,
                        principalTable: "Dancers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_DancerId",
                table: "Sessions",
                column: "DancerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishIngredient_Dish_DishesId",
                table: "DishIngredient",
                column: "DishesId",
                principalTable: "Dish",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishIngredient_Ingredient_IngredientsId",
                table: "DishIngredient",
                column: "IngredientsId",
                principalTable: "Ingredient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishSong_Dish_DishId",
                table: "DishSong",
                column: "DishId",
                principalTable: "Dish",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishSong_SongDifficulties_SongDifficultyId",
                table: "DishSong",
                column: "SongDifficultyId",
                principalTable: "SongDifficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GradedDish_Dish_DishId",
                table: "GradedDish",
                column: "DishId",
                principalTable: "Dish",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GradedIngredient_Ingredient_IngredientId",
                table: "GradedIngredient",
                column: "IngredientId",
                principalTable: "Ingredient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_SongDifficulties_SongDifficultyId",
                table: "Ingredient",
                column: "SongDifficultyId",
                principalTable: "SongDifficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishIngredient_Dish_DishesId",
                table: "DishIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_DishIngredient_Ingredient_IngredientsId",
                table: "DishIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_DishSong_Dish_DishId",
                table: "DishSong");

            migrationBuilder.DropForeignKey(
                name: "FK_DishSong_SongDifficulties_SongDifficultyId",
                table: "DishSong");

            migrationBuilder.DropForeignKey(
                name: "FK_GradedDish_Dish_DishId",
                table: "GradedDish");

            migrationBuilder.DropForeignKey(
                name: "FK_GradedIngredient_Ingredient_IngredientId",
                table: "GradedIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_SongDifficulties_SongDifficultyId",
                table: "Ingredient");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradedIngredient",
                table: "GradedIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradedDish",
                table: "GradedDish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishSong",
                table: "DishSong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dish",
                table: "Dish");

            migrationBuilder.RenameTable(
                name: "Ingredient",
                newName: "Ingredients");

            migrationBuilder.RenameTable(
                name: "GradedIngredient",
                newName: "GradedIngredients");

            migrationBuilder.RenameTable(
                name: "GradedDish",
                newName: "GradedDishes");

            migrationBuilder.RenameTable(
                name: "DishSong",
                newName: "DishSongs");

            migrationBuilder.RenameTable(
                name: "Dish",
                newName: "Dishes");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredient_SongDifficultyId",
                table: "Ingredients",
                newName: "IX_Ingredients_SongDifficultyId");

            migrationBuilder.RenameIndex(
                name: "IX_GradedIngredient_IngredientId",
                table: "GradedIngredients",
                newName: "IX_GradedIngredients_IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_GradedDish_DishId",
                table: "GradedDishes",
                newName: "IX_GradedDishes_DishId");

            migrationBuilder.RenameIndex(
                name: "IX_DishSong_SongDifficultyId",
                table: "DishSongs",
                newName: "IX_DishSongs_SongDifficultyId");

            migrationBuilder.RenameIndex(
                name: "IX_DishSong_DishId",
                table: "DishSongs",
                newName: "IX_DishSongs_DishId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 1, 11, 38, 6, 103, DateTimeKind.Utc).AddTicks(4680),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 9, 5, 14, 25, 46, 772, DateTimeKind.Utc).AddTicks(4120));

            migrationBuilder.AddColumn<Guid>(
                name: "GradedDancerDishId",
                table: "Scores",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradedIngredients",
                table: "GradedIngredients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradedDishes",
                table: "GradedDishes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishSongs",
                table: "DishSongs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dishes",
                table: "Dishes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GradedDancerDishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DancerId = table.Column<Guid>(type: "uuid", nullable: false),
                    GradedDishId = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "GradedDancerIngredients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DancerId = table.Column<Guid>(type: "uuid", nullable: false),
                    GradedIngredientId = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "IX_Scores_GradedDancerDishId",
                table: "Scores",
                column: "GradedDancerDishId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DishIngredient_Dishes_DishesId",
                table: "DishIngredient",
                column: "DishesId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishIngredient_Ingredients_IngredientsId",
                table: "DishIngredient",
                column: "IngredientsId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishSongs_Dishes_DishId",
                table: "DishSongs",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishSongs_SongDifficulties_SongDifficultyId",
                table: "DishSongs",
                column: "SongDifficultyId",
                principalTable: "SongDifficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GradedDishes_Dishes_DishId",
                table: "GradedDishes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GradedIngredients_Ingredients_IngredientId",
                table: "GradedIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_SongDifficulties_SongDifficultyId",
                table: "Ingredients",
                column: "SongDifficultyId",
                principalTable: "SongDifficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_GradedDancerDishes_GradedDancerDishId",
                table: "Scores",
                column: "GradedDancerDishId",
                principalTable: "GradedDancerDishes",
                principalColumn: "Id");
        }
    }
}
