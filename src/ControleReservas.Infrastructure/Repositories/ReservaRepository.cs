using ControleReservas.Domain;
using ControleReservas.Domain.Enum;
using ControleReservas.Domain.Interfaces;
using ControleReservas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace ControleReservas.Infrastructure.Repositories;

public class ReservaRepository : Repository<Reserva>, IReservaRepository
{
    public ReservaRepository(ControleReservasDbContext context) : base(context) {}

    public async Task<bool> ExisteConflitoReserva(Guid salaId, DateTime dataHoraInicio, DateTime dataHoraFim)
        => await _context.Reservas
            .AnyAsync(r => r.SalaId == salaId
                        && dataHoraInicio <=  r.DataHoraFim
                        && dataHoraFim >= r.DataHoraFim
                        && r.Status == Domain.Enum.ReservaStatus.Confirmada);

    public async Task<IEnumerable<Reserva>> GetReservasPorSalaEPeriodoAsync(Guid salaId, DateTime dataHoraInicio, DateTime dataHoraFim)
    {
        return await _context.Reservas
        .Where(r => r.SalaId == salaId
                    && r.Status == ReservaStatus.Confirmada
                    && ((dataHoraInicio >= r.DataHoraInicio && dataHoraInicio < r.DataHoraFim) ||
                        (dataHoraFim > r.DataHoraInicio && dataHoraFim <= r.DataHoraFim) ||
                        (dataHoraInicio <= r.DataHoraInicio && dataHoraFim >= r.DataHoraFim)))
        .ToListAsync();
    }
}
