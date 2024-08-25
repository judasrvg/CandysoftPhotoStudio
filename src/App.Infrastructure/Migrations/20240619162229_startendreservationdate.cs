using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class startendreservationdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReservationDate",
                table: "Reservation",
                newName: "ReservationDateStart");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDateEnd",
                table: "Reservation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationDateEnd",
                table: "Reservation");

            migrationBuilder.RenameColumn(
                name: "ReservationDateStart",
                table: "Reservation",
                newName: "ReservationDate");
        }
    }
}
