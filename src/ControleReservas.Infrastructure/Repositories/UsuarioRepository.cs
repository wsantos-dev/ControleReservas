using System;
using ControleReservas.Domain;
using ControleReservas.Domain.Interfaces;
using ControleReservas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ControleReservas.Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(ControleReservasDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
        => await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
 
}
