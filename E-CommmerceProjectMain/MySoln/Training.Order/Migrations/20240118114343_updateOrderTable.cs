using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.Order.Migrations
{
    public partial class updateOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RegisterEntity_RegisterId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RegisterId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "RegisterEntityRegisterId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RegisterEntityRegisterId",
                table: "Orders",
                column: "RegisterEntityRegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RegisterEntity_RegisterEntityRegisterId",
                table: "Orders",
                column: "RegisterEntityRegisterId",
                principalTable: "RegisterEntity",
                principalColumn: "RegisterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RegisterEntity_RegisterEntityRegisterId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RegisterEntityRegisterId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RegisterEntityRegisterId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RegisterId",
                table: "Orders",
                column: "RegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RegisterEntity_RegisterId",
                table: "Orders",
                column: "RegisterId",
                principalTable: "RegisterEntity",
                principalColumn: "RegisterId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
