using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BBIS_API.Migrations
{
    public partial class Validation3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehousePrice = table.Column<double>(nullable: false),
                    StockAmount = table.Column<int>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderID);
                });

            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    ProductId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(maxLength: 50, nullable: false),
                    Flavour = table.Column<string>(maxLength: 300, nullable: false),
                    Alcoholic = table.Column<bool>(nullable: false),
                    ContainerType = table.Column<string>(maxLength: 10, nullable: false),
                    Returnable = table.Column<bool>(nullable: false),
                    StockAmount = table.Column<int>(nullable: false),
                    SellPrice = table.Column<double>(nullable: false),
                    Discount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.ProductId);
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
                    SellDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellItems", x => x.SellID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductItems");

            migrationBuilder.DropTable(
                name: "SellItems");
        }
    }
}
