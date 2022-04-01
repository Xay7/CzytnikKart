using Microsoft.EntityFrameworkCore.Migrations;

namespace Czytnik123.Migrations
{
    public partial class card : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AddColumn<int>(
                name: "Card",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 1,
                column: "CardSerialNumber",
                value: "4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22");

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "Card",
                value: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Card",
                table: "Rooms");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: 1,
                column: "CardSerialNumber",
                value: "13123");

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "CardSerialNumber" },
                values: new object[,]
                {
                    { 2, "45745754" },
                    { 3, "546" },
                    { 4, "457547" },
                    { 5, "547547" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "1233" },
                    { 3, "1243" },
                    { 4, "1253" },
                    { 5, "1263" }
                });
        }
    }
}
