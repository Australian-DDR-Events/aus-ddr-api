using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class CreateRewards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 24, 4, 1, 21, 650, DateTimeKind.Utc).AddTicks(210),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 7, 10, 13, 48, 57, 842, DateTimeKind.Utc).AddTicks(7570));

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TriggerData = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RewardTriggers",
                columns: table => new
                {
                    Trigger = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardTriggers", x => x.Trigger);
                });

            migrationBuilder.CreateTable(
                name: "RewardRewardTrigger",
                columns: table => new
                {
                    RewardsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TriggersTrigger = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardRewardTrigger", x => new { x.RewardsId, x.TriggersTrigger });
                    table.ForeignKey(
                        name: "FK_RewardRewardTrigger_Rewards_RewardsId",
                        column: x => x.RewardsId,
                        principalTable: "Rewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RewardRewardTrigger_RewardTriggers_TriggersTrigger",
                        column: x => x.TriggersTrigger,
                        principalTable: "RewardTriggers",
                        principalColumn: "Trigger",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RewardRewardTrigger_TriggersTrigger",
                table: "RewardRewardTrigger",
                column: "TriggersTrigger");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RewardRewardTrigger");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropTable(
                name: "RewardTriggers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmissionTime",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 10, 13, 48, 57, 842, DateTimeKind.Utc).AddTicks(7570),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 7, 24, 4, 1, 21, 650, DateTimeKind.Utc).AddTicks(210));
        }
    }
}
