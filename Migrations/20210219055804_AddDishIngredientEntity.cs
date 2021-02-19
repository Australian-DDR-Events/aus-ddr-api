using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AusDdrApi.Migrations
{
    public partial class AddDishIngredientEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishSongs_Dishes_DishId",
                table: "DishSongs");

            migrationBuilder.DropTable(
                name: "DishIngredient");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 19, 5, 58, 4, 556, DateTimeKind.Utc).AddTicks(2110),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 19, 4, 50, 47, 985, DateTimeKind.Utc).AddTicks(9620));

            migrationBuilder.AlterColumn<Guid>(
                name: "DishId",
                table: "DishSongs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DishIngredients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DishId = table.Column<Guid>(type: "uuid", nullable: false),
                    IngredientId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishIngredients_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishIngredients_DishId",
                table: "DishIngredients",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_DishIngredients_IngredientId",
                table: "DishIngredients",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishSongs_Dishes_DishId",
                table: "DishSongs",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishSongs_Dishes_DishId",
                table: "DishSongs");

            migrationBuilder.DropTable(
                name: "DishIngredients");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 19, 4, 50, 47, 985, DateTimeKind.Utc).AddTicks(9620),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 19, 5, 58, 4, 556, DateTimeKind.Utc).AddTicks(2110));

            migrationBuilder.AlterColumn<Guid>(
                name: "DishId",
                table: "DishSongs",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

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
                name: "IX_DishIngredient_IngredientsId",
                table: "DishIngredient",
                column: "IngredientsId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishSongs_Dishes_DishId",
                table: "DishSongs",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
