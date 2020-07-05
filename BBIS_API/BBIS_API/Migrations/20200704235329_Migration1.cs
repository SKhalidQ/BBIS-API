using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BBIS_API.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    ProductID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(maxLength: 50, nullable: false),
                    Flavour = table.Column<string>(maxLength: 60, nullable: false),
                    Alcoholic = table.Column<bool>(nullable: false),
                    ContainerType = table.Column<string>(maxLength: 20, nullable: false),
                    Returnable = table.Column<bool>(nullable: false),
                    StockAmount = table.Column<int>(nullable: false),
                    SellPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    UserID = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 30, nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityOrdered = table.Column<int>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    ProductID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_OrderItems_ProductItems_ProductID",
                        column: x => x.ProductID,
                        principalTable: "ProductItems",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellItems",
                columns: table => new
                {
                    SellID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContainerReturned = table.Column<bool>(nullable: false),
                    Payed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellDate = table.Column<DateTime>(nullable: false),
                    ProductID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellItems", x => x.SellID);
                    table.ForeignKey(
                        name: "FK_SellItems_ProductItems_ProductID",
                        column: x => x.ProductID,
                        principalTable: "ProductItems",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductID",
                table: "OrderItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_SellItems_ProductID",
                table: "SellItems",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "SellItems");

            migrationBuilder.DropTable(
                name: "UserItems");

            migrationBuilder.DropTable(
                name: "ProductItems");
        }
    }
}
