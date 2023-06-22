using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBoards2.Migrations
{
    public partial class TagDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Category", "Value" },
                values: new object[,]
                {
                    { 1, null, "Web" },
                    { 2, null, "UI" },
                    { 3, null, "Desktop" },
                    { 4, null, "API" },
                    { 5, null, "Service" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
