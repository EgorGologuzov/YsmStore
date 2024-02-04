using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YsmStore.API.Migrations
{
    public partial class Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_Category",
                table: "Products",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Title_Option1_Option2",
                table: "Products",
                columns: new[] { "Title", "Option1", "Option2" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Category",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Title_Option1_Option2",
                table: "Products");
        }
    }
}
