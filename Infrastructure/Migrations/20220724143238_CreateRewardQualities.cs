using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class CreateRewardQualities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 24, 14, 32, 38, 455, DateTimeKind.Utc).AddTicks(380),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 7, 24, 4, 1, 21, 650, DateTimeKind.Utc).AddTicks(210));

            migrationBuilder.CreateTable(
                name: "RewardQualities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    RewardId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardQualities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardQualities_Rewards_RewardId",
                        column: x => x.RewardId,
                        principalTable: "Rewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DancerRewardQuality",
                columns: table => new
                {
                    DancersId = table.Column<Guid>(type: "uuid", nullable: false),
                    RewardQualitiesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DancerRewardQuality", x => new { x.DancersId, x.RewardQualitiesId });
                    table.ForeignKey(
                        name: "FK_DancerRewardQuality_Dancers_DancersId",
                        column: x => x.DancersId,
                        principalTable: "Dancers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DancerRewardQuality_RewardQualities_RewardQualitiesId",
                        column: x => x.RewardQualitiesId,
                        principalTable: "RewardQualities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DancerRewardQuality_RewardQualitiesId",
                table: "DancerRewardQuality",
                column: "RewardQualitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardQualities_RewardId",
                table: "RewardQualities",
                column: "RewardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DancerRewardQuality");

            migrationBuilder.DropTable(
                name: "RewardQualities");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 24, 4, 1, 21, 650, DateTimeKind.Utc).AddTicks(210),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 7, 24, 14, 32, 38, 455, DateTimeKind.Utc).AddTicks(380));
        }
    }
}
