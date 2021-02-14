using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AusDdrApi.Migrations
{
    public partial class AddRequiredScoreToGradedIngredientAndSchemaCheanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "Dancers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 14, 1, 12, 35, 149, DateTimeKind.Utc).AddTicks(440),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 11, 2, 4, 9, 978, DateTimeKind.Utc).AddTicks(1580));

            migrationBuilder.AddColumn<int>(
                name: "RequiredScore",
                table: "GradedIngredients",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredScore",
                table: "GradedIngredients");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 11, 2, 4, 9, 978, DateTimeKind.Utc).AddTicks(1580),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2021, 2, 14, 1, 12, 35, 149, DateTimeKind.Utc).AddTicks(440));

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "Dancers",
                type: "text",
                nullable: true);
        }
    }
}
