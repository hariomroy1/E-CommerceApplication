using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.User.Migrations
{
    public partial class addtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registers",
                columns: table => new
                {
                    RegisterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<long>(type: "bigint", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberSince = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registers", x => x.RegisterId);
                });

            migrationBuilder.InsertData(
                table: "Registers",
                columns: new[] { "RegisterId", "Email", "MemberSince", "Name", "Password", "Phone", "isAdmin" },
                values: new object[] { 6, "adminFirst@gmail.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin2", "admin@123456", 2345678905L, true });

            migrationBuilder.InsertData(
                table: "Registers",
                columns: new[] { "RegisterId", "Email", "MemberSince", "Name", "Password", "Phone", "isAdmin" },
                values: new object[] { 7, "hariomroy@gmail.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Anshu", "admin@123457", 1234567896L, true });

            migrationBuilder.InsertData(
                table: "Registers",
                columns: new[] { "RegisterId", "Email", "MemberSince", "Name", "Password", "Phone", "isAdmin" },
                values: new object[] { 8, "admin34@gmail.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PriyamAdmin", "admin@123455", 12345678960L, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registers");
        }
    }
}
