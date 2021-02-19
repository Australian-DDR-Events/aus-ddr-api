using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AusDdrApi.Migrations
{
    public partial class ReworkDishes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedDancerDishes_GradedDish_GradedDishId",
                table: "GradedDancerDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_GradedDish_Dishes_DishId",
                table: "GradedDish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradedDish",
                table: "GradedDish");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "GradedDish");

            migrationBuilder.RenameTable(
                name: "GradedDish",
                newName: "GradedDishes");

            migrationBuilder.RenameIndex(
                name: "IX_GradedDish_DishId",
                table: "GradedDishes",
                newName: "IX_GradedDishes_DishId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 19, 4, 21, 15, 744, DateTimeKind.Utc).AddTicks(950),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 14, 1, 37, 58, 654, DateTimeKind.Utc).AddTicks(2170));

            migrationBuilder.AddColumn<Guid>(
                name: "GradedDancerDishId",
                table: "Scores",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradedDishes",
                table: "GradedDishes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DishSongs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CookingOrder = table.Column<int>(type: "integer", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false),
                    DishId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishSongs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishSongs_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DishSongs_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scores_GradedDancerDishId",
                table: "Scores",
                column: "GradedDancerDishId");

            migrationBuilder.CreateIndex(
                name: "IX_DishSongs_DishId",
                table: "DishSongs",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_DishSongs_SongId",
                table: "DishSongs",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_GradedDancerDishes_GradedDishes_GradedDishId",
                table: "GradedDancerDishes",
                column: "GradedDishId",
                principalTable: "GradedDishes",
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
                name: "FK_Scores_GradedDancerDishes_GradedDancerDishId",
                table: "Scores",
                column: "GradedDancerDishId",
                principalTable: "GradedDancerDishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedDancerDishes_GradedDishes_GradedDishId",
                table: "GradedDancerDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_GradedDishes_Dishes_DishId",
                table: "GradedDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_GradedDancerDishes_GradedDancerDishId",
                table: "Scores");

            migrationBuilder.DropTable(
                name: "DishSongs");

            migrationBuilder.DropIndex(
                name: "IX_Scores_GradedDancerDishId",
                table: "Scores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradedDishes",
                table: "GradedDishes");

            migrationBuilder.DropColumn(
                name: "GradedDancerDishId",
                table: "Scores");

            migrationBuilder.RenameTable(
                name: "GradedDishes",
                newName: "GradedDish");

            migrationBuilder.RenameIndex(
                name: "IX_GradedDishes_DishId",
                table: "GradedDish",
                newName: "IX_GradedDish_DishId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 14, 1, 37, 58, 654, DateTimeKind.Utc).AddTicks(2170),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 19, 4, 21, 15, 744, DateTimeKind.Utc).AddTicks(950));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "GradedDish",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradedDish",
                table: "GradedDish",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GradedDancerDishes_GradedDish_GradedDishId",
                table: "GradedDancerDishes",
                column: "GradedDishId",
                principalTable: "GradedDish",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GradedDish_Dishes_DishId",
                table: "GradedDish",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
