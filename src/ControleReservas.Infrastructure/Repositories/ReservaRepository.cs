using System;
using ControleReservas.Domain;
using ControleReservas.Domain.Interfaces;
using ControleReservas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ControleReservas.Infrastructure.Repositories;

public class ReservaRepository : Repository<Reserva>, IReservaRepository
{
    public ReservaRepository(ControleReservasDbContext context) : base(context) {}

    public async Task<bool> ExisteConflitoReserva(Guid salaId, DateTime dataHora)
        => await _context.Reservas
            .AnyAsync(r => r.SalaId == salaId
                        && r.DataHoraReserva == dataHora
                        && r.Status == Domain.Enum.ReservaStatus.Confirmada);
}
