using System;
using ControleReservas.Domain.Interfaces;
using ControleReservas.Infrastructure.Persistence;

namespace ControleReservas.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ControleReservasDbContext _context;

    public ISalaRepository Salas { get; }
    public IUsuarioRepository Usuarios { get; }
    public IReservaRepository Reservas { get; }

    public UnitOfWork(ControleReservasDbContext context)
    {
        _context = context;
        Salas = new SalaRepository(_context);
        Usuarios = new UsuarioRepository(_context);
        Reservas = new ReservaRepository(_context);

    }


    public async Task CommitAsync()
        => await _context.SaveChangesAsync();
}
