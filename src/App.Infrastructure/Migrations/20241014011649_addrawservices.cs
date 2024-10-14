using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addrawservices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RawMaterialId",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_RawMaterialId",
                table: "Products",
                column: "RawMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Products_RawMaterialId",
                table: "Products",
                column: "RawMaterialId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Products_RawMaterialId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_RawMaterialId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RawMaterialId",
                table: "Products");
        }
    }
}
