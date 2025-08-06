using System;

namespace ControleReservas.Domain;

public class Usuario
{
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }

    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
