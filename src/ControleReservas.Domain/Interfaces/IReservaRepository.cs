using System;

namespace ControleReservas.Domain.Interfaces;

public interface IReservaRepository : IRespository<Reserva>
{
    Task<bool> ExisteConflitoReserva(Guid salaId, DateTime dataHora);
}
