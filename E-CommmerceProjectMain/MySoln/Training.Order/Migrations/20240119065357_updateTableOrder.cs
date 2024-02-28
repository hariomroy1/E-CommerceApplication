using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.Order.Migrations
{
    public partial class updateTableOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_ProductEntityProductId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RegisterEntity_RegisterEntityRegisterId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "RegisterEntity");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProductEntityProductId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RegisterEntityRegisterId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductEntityProductId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RegisterEntityRegisterId",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductEntityProductId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegisterEntityRegisterId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Specification = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "RegisterEntity",
                columns: table => new
                {
                    RegisterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberSince = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<long>(type: "bigint", nullable: false),
                    isAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterEntity", x => x.RegisterId);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RegisterId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Carts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_RegisterEntity_RegisterId",
                        column: x => x.RegisterId,
                        principalTable: "RegisterEntity",
                        principalColumn: "RegisterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductEntityProductId",
                table: "Orders",
                column: "ProductEntityProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RegisterEntityRegisterId",
                table: "Orders",
                column: "RegisterEntityRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductId",
                table: "Carts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_RegisterId",
                table: "Carts",
                column: "RegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_ProductEntityProductId",
                table: "Orders",
                column: "ProductEntityProductId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RegisterEntity_RegisterEntityRegisterId",
                table: "Orders",
                column: "RegisterEntityRegisterId",
                principalTable: "RegisterEntity",
                principalColumn: "RegisterId");
        }
    }
}
