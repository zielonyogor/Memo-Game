using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NR155910155992.MemoGame.Dao.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "Assets/Cards/card1.png");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagePath",
                value: "Assets/Cards/card2.png");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: "Assets/Cards/card3.png");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagePath",
                value: "Assets/Cards/card4.png");

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "ImagePath", "Name" },
                values: new object[] { 5, "Assets/Cards/card5.png", "Card 5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "images/card1.png");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagePath",
                value: "images/card2.png");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: "images/card3.png");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagePath",
                value: "images/card4.png");
        }
    }
}
