using ControleReservas.Domain.Enum;

namespace ControleReservas.Application.DTOs;

public class ReservaDto
{
    public Guid Id { get; set; }
    public Guid SalaId { get; set; }
    public Guid UsuarioId { get; set; }
    public DateTime DataHoraReserva { get; set; }
    public ReservaStatus Status { get; set; }

}
