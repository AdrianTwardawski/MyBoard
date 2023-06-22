using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBoards2.Migrations
{
    public partial class WorkItemStateSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "WorkItemStates",
                columns: new[] { "Id", "Value" },
                values: new object[] { 1, "To Do" });

            migrationBuilder.InsertData(
                table: "WorkItemStates",
                columns: new[] { "Id", "Value" },
                values: new object[] { 2, "Doing" });

            migrationBuilder.InsertData(
                table: "WorkItemStates",
                columns: new[] { "Id", "Value" },
                values: new object[] { 3, "Done" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkItemStates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkItemStates",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WorkItemStates",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Tags");
        }
    }
}
