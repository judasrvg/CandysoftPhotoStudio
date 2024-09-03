using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixInventaryLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Products_RawMaterialId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_RawMaterialId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ConsumptionRate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastUsedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RawMaterialId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RawMaterial_StockQuantity",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchaseCost",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseCost",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TotalQuantity",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "ConsumptionRate",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUsedDate",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RawMaterialId",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RawMaterial_StockQuantity",
                table: "Products",
                type: "int",
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
    }
}
