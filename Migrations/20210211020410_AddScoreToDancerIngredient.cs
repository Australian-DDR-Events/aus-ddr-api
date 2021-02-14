using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AusDdrApi.Migrations
{
    public partial class AddScoreToDancerIngredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedDancerIngredients_GradedDancerDishes_GradedDancerDish~",
                table: "GradedDancerIngredients");

            migrationBuilder.DropTable(
                name: "DishIngredient");

            migrationBuilder.DropIndex(
                name: "IX_GradedDancerIngredients_GradedDancerDishId",
                table: "GradedDancerIngredients");

            migrationBuilder.DropColumn(
                name: "GradedDancerDishId",
                table: "GradedDancerIngredients");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 11, 2, 4, 9, 978, DateTimeKind.Utc).AddTicks(1580),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 7, 7, 42, 17, 216, DateTimeKind.Utc).AddTicks(8180));

            migrationBuilder.AddColumn<Guid>(
                name: "DishId",
                table: "Ingredients",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SongId",
                table: "Ingredients",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ScoreId",
                table: "GradedDancerIngredients",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_DishId",
                table: "Ingredients",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_SongId",
                table: "Ingredients",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_GradedDancerIngredients_ScoreId",
                table: "GradedDancerIngredients",
                column: "ScoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_GradedDancerIngredients_Songs_ScoreId",
                table: "GradedDancerIngredients",
                column: "ScoreId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Dishes_DishId",
                table: "Ingredients",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Songs_SongId",
                table: "Ingredients",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedDancerIngredients_Songs_ScoreId",
                table: "GradedDancerIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Dishes_DishId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Songs_SongId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_DishId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_SongId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_GradedDancerIngredients_ScoreId",
                table: "GradedDancerIngredients");

            migrationBuilder.DropColumn(
                name: "DishId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "SongId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "ScoreId",
                table: "GradedDancerIngredients");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 7, 7, 42, 17, 216, DateTimeKind.Utc).AddTicks(8180),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 11, 2, 4, 9, 978, DateTimeKind.Utc).AddTicks(1580));

            migrationBuilder.AddColumn<Guid>(
                name: "GradedDancerDishId",
                table: "GradedDancerIngredients",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_GradedDancerIngredients_GradedDancerDishId",
                table: "GradedDancerIngredients",
                column: "GradedDancerDishId");

            migrationBuilder.CreateIndex(
                name: "IX_DishIngredient_IngredientsId",
                table: "DishIngredient",
                column: "IngredientsId");

            migrationBuilder.AddForeignKey(
                name: "FK_GradedDancerIngredients_GradedDancerDishes_GradedDancerDish~",
                table: "GradedDancerIngredients",
                column: "GradedDancerDishId",
                principalTable: "GradedDancerDishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
