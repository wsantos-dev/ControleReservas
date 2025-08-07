using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleReservas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PopularBancoDados : Migration
    {
        /// <inheritdoc />

        // Chaves primárias pré-definidas
        Guid usuario1Id = Guid.NewGuid();
        Guid usuario2Id = Guid.NewGuid();
        Guid sala1Id = Guid.NewGuid();
        Guid sala2Id = Guid.NewGuid();
        Guid reserva1Id = Guid.NewGuid();
        Guid reserva2Id = Guid.NewGuid();
        Guid reserva3Id = Guid.NewGuid();

        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Nome", "Email" },
                values: new object[,]
                {
                    { usuario1Id, "João Silva", "joao.silva@email.com" },
                    { usuario2Id, "Maria Oliveira", "maria.oliveira@email.com" }
                });

            // Inserindo Salas
            migrationBuilder.InsertData(
                table: "Salas",
                columns: new[] { "Id", "Nome", "Capacidade" },
                values: new object[,]
                {
                    { sala1Id, "Sala de Reunião Alpha", 10 },
                    { sala2Id, "Sala de Reunião Beta", 5 }
                });

            // Inserindo Reservas
            migrationBuilder.InsertData(
                table: "Reservas",
                columns: new[] { "Id", "SalaId", "UsuarioId", "DataHoraInicio", "DataHoraFim", "Status" },
                values: new object[,]
                {

                    {
                        reserva1Id,
                        sala1Id,
                        usuario1Id,
                        new DateTime(2025, 8, 10, 10, 0, 0),
                        new DateTime(2025, 8, 10, 11, 0, 0),
                        1
                    },


                    {
                        reserva2Id,
                        sala2Id,
                        usuario2Id,
                        new DateTime(2025, 8, 10, 14, 0, 0),
                        new DateTime(2025, 8, 10, 15, 30, 0),
                        1
                    },
                });

            migrationBuilder.InsertData(
                table: "Reservas",
                columns: new[] { "Id", "SalaId", "UsuarioId", "DataHoraInicio", "DataHoraFim", "Status", "DataCancelamento" },
                values: new object[]
                {
                    reserva3Id,
                    sala1Id,
                    usuario2Id,
                    new DateTime(2025, 8, 11, 9, 0, 0),
                    new DateTime(2025, 8, 11, 10, 0, 0),
                    2,
                    new DateTime(2025, 8, 9, 9, 0, 0)
                });
        }



        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reservas",
                keyColumn: "Id",
                keyValues: new object[] {
                    reserva1Id,
                    reserva2Id,
                    reserva3Id
                });

            migrationBuilder.DeleteData(
                table: "Salas",
                keyColumn: "Id",
                keyValues: new object[] {
                    sala1Id,
                    sala2Id
                });

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValues: new object[] {
                    usuario1Id,
                    usuario2Id
                });
        }
    }
}
