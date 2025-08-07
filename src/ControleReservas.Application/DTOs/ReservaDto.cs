using ControleReservas.Domain.Enum;

namespace ControleReservas.Application.DTOs;

public class ReservaDto
{
    public Guid Id { get; set; }
    public Guid SalaId { get; set; }
    public Guid UsuarioId { get; set; }
    public DateTime DataHoraInicio { get; set; }
    public DateTime DataHoraFim { get; set; }
    public ReservaStatus Status { get; set; }
    public DateTime? DataCancelamento { get; set; }


}
