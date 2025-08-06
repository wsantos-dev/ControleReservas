using System;

namespace ControleReservas.Domain;

public class Reserva
{
    public Guid Id;

    public Guid SalaId { get; set; }

    public Sala Sala { get; set; } = null!;

    public Guid UsuarioId { get; set; }

    public Usuario Usuario { get; set; } = null!;

    public DateTime DataHoraReserva { get; set; }

    public ReservaStatus Status { get; set; }
}
