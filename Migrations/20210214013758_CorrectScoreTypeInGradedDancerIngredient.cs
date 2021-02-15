using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AusDdrApi.Migrations
{
    public partial class CorrectScoreTypeInGradedDancerIngredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedDancerIngredients_Songs_ScoreId",
                table: "GradedDancerIngredients");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 14, 1, 37, 58, 654, DateTimeKind.Utc).AddTicks(2170),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 14, 1, 12, 35, 149, DateTimeKind.Utc).AddTicks(440));

            migrationBuilder.AddForeignKey(
                name: "FK_GradedDancerIngredients_Scores_ScoreId",
                table: "GradedDancerIngredients",
                column: "ScoreId",
                principalTable: "Scores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedDancerIngredients_Scores_ScoreId",
                table: "GradedDancerIngredients");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 14, 1, 12, 35, 149, DateTimeKind.Utc).AddTicks(440),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 14, 1, 37, 58, 654, DateTimeKind.Utc).AddTicks(2170));

            migrationBuilder.AddForeignKey(
                name: "FK_GradedDancerIngredients_Songs_ScoreId",
                table: "GradedDancerIngredients",
                column: "ScoreId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
