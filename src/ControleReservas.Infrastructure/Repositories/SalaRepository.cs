using System;
using ControleReservas.Domain;
using ControleReservas.Domain.Interfaces;
using ControleReservas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ControleReservas.Infrastructure.Repositories;

public class SalaRepository : Repository<Sala>, ISalaRepository
{
    public SalaRepository(ControleReservasDbContext context) : base(context) { }

    public async Task<bool> SalaExisteAsync(string nome)
        => await _context.Salas.AnyAsync(s => s.Nome == nome);
}
