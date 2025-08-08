using System;
using ControleReservas.Domain.Entities;
using ControleReservas.Domain.Interfaces;
using ControleReservas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ControleReservas.Infrastructure.Repositories;

public class ConfiguracoesEmailRepository : Repository<ConfiguracoesEmail>, IConfiguracoesEmailRepository
{
    public ConfiguracoesEmailRepository(ControleReservasDbContext context) : base(context)
    {
    }

    public async Task<string?> ObterEmailPorChaveAsync(string chave)
    {
       return await _context.ConfiguracoesEmails
                .Where(c => c.Chave == chave)
                .Select(c => c.Email)
                .FirstOrDefaultAsync();
    }

    public async Task<string?> ObterValorPorChaveAsync(string chave)
    {
        return await _context.ConfiguracoesEmails
                .Where(c => c.Chave == chave)
                .Select(c => c.Valor)
                .FirstOrDefaultAsync();
    }
}
