using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BBIS_API.Migrations
{
    public partial class Mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    ProductId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(maxLength: 50, nullable: false),
                    Flavour = table.Column<string>(maxLength: 60, nullable: false),
                    Alcoholic = table.Column<bool>(nullable: false),
                    ContainerType = table.Column<string>(maxLength: 10, nullable: false),
                    Returnable = table.Column<bool>(nullable: false),
                    StockAmount = table.Column<int>(nullable: false),
                    SellPrice = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehousePrice = table.Column<double>(nullable: false),
                    StockAmount = table.Column<int>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    ProductID = table.Column<long>(nullable: false),
                    ProductObjProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_OrderItems_ProductItems_ProductObjProductId",
                        column: x => x.ProductObjProductId,
                        principalTable: "ProductItems",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellItems",
                columns: table => new
                {
                    SellID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellAmount = table.Column<int>(nullable: false),
                    SellPriceTotal = table.Column<double>(nullable: false),
                    DiscountApplied = table.Column<bool>(nullable: false),
                    SellDate = table.Column<DateTime>(nullable: false),
                    ProductID = table.Column<long>(nullable: false),
                    ProductObjProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellItems", x => x.SellID);
                    table.ForeignKey(
                        name: "FK_SellItems_ProductItems_ProductObjProductId",
                        column: x => x.ProductObjProductId,
                        principalTable: "ProductItems",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductObjProductId",
                table: "OrderItems",
                column: "ProductObjProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SellItems_ProductObjProductId",
                table: "SellItems",
                column: "ProductObjProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "SellItems");

            migrationBuilder.DropTable(
                name: "ProductItems");
        }
    }
}
