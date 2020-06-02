using Microsoft.EntityFrameworkCore.Migrations;

namespace BBIS_API.Migrations
{
    public partial class Mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductItems_ProductObjProductId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductObjProductId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductObjProductId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "OrderItems",
                newName: "ProductId");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "OrderItems",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductItems_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "ProductItems",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductItems_ProductId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderItems",
                newName: "ProductID");

            migrationBuilder.AlterColumn<long>(
                name: "ProductID",
                table: "OrderItems",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductObjProductId",
                table: "OrderItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductObjProductId",
                table: "OrderItems",
                column: "ProductObjProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductItems_ProductObjProductId",
                table: "OrderItems",
                column: "ProductObjProductId",
                principalTable: "ProductItems",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
