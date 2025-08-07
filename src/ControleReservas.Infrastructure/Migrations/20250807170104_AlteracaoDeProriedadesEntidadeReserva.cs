using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleReservas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoDeProriedadesEntidadeReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataHoraReserva",
                table: "Reservas",
                newName: "DataHoraInicio");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCancelamento",
                table: "Reservas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataHoraFim",
                table: "Reservas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCancelamento",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "DataHoraFim",
                table: "Reservas");

            migrationBuilder.RenameColumn(
                name: "DataHoraInicio",
                table: "Reservas",
                newName: "DataHoraReserva");
        }
    }
}
