using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.User.Migrations
{
    public partial class msg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e9b4926-b963-4a9b-8922-a46b89a26454");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47c210f4-9824-4c51-9e65-b6b92500c4d9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "07c6a60b-d780-4757-a95e-3a471df28dd0", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "254d63ea-ea00-4516-a413-0cb8b8c5e6cf", "1", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07c6a60b-d780-4757-a95e-3a471df28dd0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "254d63ea-ea00-4516-a413-0cb8b8c5e6cf");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3e9b4926-b963-4a9b-8922-a46b89a26454", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "47c210f4-9824-4c51-9e65-b6b92500c4d9", "1", "Admin", "Admin" });
        }
    }
}
