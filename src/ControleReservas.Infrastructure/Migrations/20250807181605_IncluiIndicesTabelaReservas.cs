using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleReservas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IncluiIndicesTabelaReservas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservas_SalaId",
                table: "Reservas");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_SalaId_DataHoraInicio_DataHoraFim",
                table: "Reservas",
                columns: new[] { "SalaId", "DataHoraInicio", "DataHoraFim" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservas_SalaId_DataHoraInicio_DataHoraFim",
                table: "Reservas");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_SalaId",
                table: "Reservas",
                column: "SalaId");
        }
    }
}
