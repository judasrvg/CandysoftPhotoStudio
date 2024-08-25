using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg_initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigValue",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValueType = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigValue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tattoo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TattooName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TattooDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TattooStyleId = table.Column<long>(type: "bigint", nullable: true),
                    TattooBodyPartId = table.Column<long>(type: "bigint", nullable: true),
                    TattooGenreId = table.Column<long>(type: "bigint", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tattoo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tattoo_ConfigValue_TattooBodyPartId",
                        column: x => x.TattooBodyPartId,
                        principalTable: "ConfigValue",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tattoo_ConfigValue_TattooGenreId",
                        column: x => x.TattooGenreId,
                        principalTable: "ConfigValue",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tattoo_ConfigValue_TattooStyleId",
                        column: x => x.TattooStyleId,
                        principalTable: "ConfigValue",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TattooId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservation_Tattoo_TattooId",
                        column: x => x.TattooId,
                        principalTable: "Tattoo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_TattooId",
                table: "Reservation",
                column: "TattooId");

            migrationBuilder.CreateIndex(
                name: "IX_Tattoo_TattooBodyPartId",
                table: "Tattoo",
                column: "TattooBodyPartId");

            migrationBuilder.CreateIndex(
                name: "IX_Tattoo_TattooGenreId",
                table: "Tattoo",
                column: "TattooGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Tattoo_TattooStyleId",
                table: "Tattoo",
                column: "TattooStyleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Tattoo");

            migrationBuilder.DropTable(
                name: "ConfigValue");
        }
    }
}
