using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.User.Migrations
{
    public partial class removeadmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdmin",
                table: "Registers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3e9b4926-b963-4a9b-8922-a46b89a26454", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "47c210f4-9824-4c51-9e65-b6b92500c4d9", "1", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e9b4926-b963-4a9b-8922-a46b89a26454");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47c210f4-9824-4c51-9e65-b6b92500c4d9");

            migrationBuilder.AddColumn<bool>(
                name: "isAdmin",
                table: "Registers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
