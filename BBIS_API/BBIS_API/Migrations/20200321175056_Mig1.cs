using Microsoft.EntityFrameworkCore.Migrations;

namespace BBIS_API.Migrations
{
    public partial class Mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductID",
                table: "SellItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductObjProductId",
                table: "SellItems",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Flavour",
                table: "ProductItems",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<long>(
                name: "ProductID",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductObjProductId",
                table: "OrderItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellItems_ProductObjProductId",
                table: "SellItems",
                column: "ProductObjProductId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SellItems_ProductItems_ProductObjProductId",
                table: "SellItems",
                column: "ProductObjProductId",
                principalTable: "ProductItems",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductItems_ProductObjProductId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SellItems_ProductItems_ProductObjProductId",
                table: "SellItems");

            migrationBuilder.DropIndex(
                name: "IX_SellItems_ProductObjProductId",
                table: "SellItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductObjProductId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "SellItems");

            migrationBuilder.DropColumn(
                name: "ProductObjProductId",
                table: "SellItems");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductObjProductId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<string>(
                name: "Flavour",
                table: "ProductItems",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 60);
        }
    }
}
