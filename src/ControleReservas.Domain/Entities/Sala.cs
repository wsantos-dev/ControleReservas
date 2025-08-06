using System;

namespace ControleReservas.Domain;

public class Sala
{
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public int Capacidade { get; set; }

    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
