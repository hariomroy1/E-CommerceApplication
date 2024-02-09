using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.Cart.Migrations
{
    public partial class updateCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_ProductEntity_ProductId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_RegisterEntity_RegisterId",
                table: "Carts");

            migrationBuilder.DropTable(
                name: "OrderEntity");

            migrationBuilder.DropTable(
                name: "ProductEntity");

            migrationBuilder.DropTable(
                name: "RegisterEntity");

            migrationBuilder.DropIndex(
                name: "IX_Carts_ProductId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_RegisterId",
                table: "Carts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductEntity",
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
                    table.PrimaryKey("PK_ProductEntity", x => x.ProductId);
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
                name: "OrderEntity",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisterId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuantityOfItems = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderEntity", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_OrderEntity_RegisterEntity_RegisterId",
                        column: x => x.RegisterId,
                        principalTable: "RegisterEntity",
                        principalColumn: "RegisterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductId",
                table: "Carts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_RegisterId",
                table: "Carts",
                column: "RegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderEntity_RegisterId",
                table: "OrderEntity",
                column: "RegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_ProductEntity_ProductId",
                table: "Carts",
                column: "ProductId",
                principalTable: "ProductEntity",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_RegisterEntity_RegisterId",
                table: "Carts",
                column: "RegisterId",
                principalTable: "RegisterEntity",
                principalColumn: "RegisterId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
