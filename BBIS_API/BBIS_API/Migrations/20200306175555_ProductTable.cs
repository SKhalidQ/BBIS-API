using Microsoft.EntityFrameworkCore.Migrations;

namespace BBIS_API.Migrations
{
    public partial class ProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    ProductId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(nullable: true),
                    Flavour = table.Column<string>(nullable: true),
                    Alcoholic = table.Column<bool>(nullable: false),
                    ContainerType = table.Column<string>(nullable: true),
                    Returnable = table.Column<bool>(nullable: false),
                    StockAmount = table.Column<int>(nullable: false),
                    SellPrice = table.Column<double>(nullable: false),
                    Discount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.ProductId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductItems");
        }
    }
}
