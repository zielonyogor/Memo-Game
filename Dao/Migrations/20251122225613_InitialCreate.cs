using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NR155910155992.MemoGame.Dao.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    GameType = table.Column<int>(type: "INTEGER", nullable: false),
                    GameMode = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerGameResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardsUncovered = table.Column<int>(type: "INTEGER", nullable: false),
                    IsWinner = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameSessionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGameResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerGameResults_GameSessions_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerGameResults_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "ImagePath", "Name" },
                values: new object[,]
                {
                    { 1, "images/card1.png", "Card 1" },
                    { 2, "images/card2.png", "Card 2" },
                    { 3, "images/card3.png", "Card 3" }
                });

            migrationBuilder.InsertData(
                table: "GameSessions",
                columns: new[] { "Id", "Duration", "GameDate", "GameMode", "GameType" },
                values: new object[] { 1, new TimeSpan(0, 0, 1, 30, 0), new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), 0, 1 });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "Id", "UserName" },
                values: new object[] { 1, "Player1" });

            migrationBuilder.InsertData(
                table: "PlayerGameResults",
                columns: new[] { "Id", "CardsUncovered", "GameSessionId", "IsWinner", "UserProfileId" },
                values: new object[] { 1, 12, 1, true, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameResults_GameSessionId",
                table: "PlayerGameResults",
                column: "GameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameResults_UserProfileId",
                table: "PlayerGameResults",
                column: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "PlayerGameResults");

            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}
