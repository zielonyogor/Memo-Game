using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NR155910155992.MemoGame.Dao.Migrations
{
    /// <inheritdoc />
    public partial class ImproveSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "ImagePath", "Name" },
                values: new object[] { 4, "images/card4.png", "Card 4" });

            migrationBuilder.InsertData(
                table: "GameSessions",
                columns: new[] { "Id", "Duration", "GameDate", "GameMode", "GameType" },
                values: new object[,]
                {
                    { 2, new TimeSpan(0, 0, 2, 30, 0), new DateTime(2025, 1, 2, 15, 30, 0, 0, DateTimeKind.Unspecified), 0, 1 },
                    { 3, new TimeSpan(0, 0, 3, 20, 0), new DateTime(2025, 1, 3, 18, 45, 0, 0, DateTimeKind.Unspecified), 0, 2 }
                });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "Id", "UserName" },
                values: new object[] { 2, "Player2" });

            migrationBuilder.InsertData(
                table: "PlayerGameResults",
                columns: new[] { "Id", "CardsUncovered", "GameSessionId", "IsWinner", "UserProfileId" },
                values: new object[,]
                {
                    { 2, 8, 2, false, 2 },
                    { 3, 10, 3, true, 1 },
                    { 4, 7, 3, false, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PlayerGameResults",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PlayerGameResults",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PlayerGameResults",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GameSessions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GameSessions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UserProfiles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
