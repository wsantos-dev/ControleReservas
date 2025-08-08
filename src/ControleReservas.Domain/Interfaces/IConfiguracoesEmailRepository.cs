using System;

namespace ControleReservas.Domain.Interfaces;

public interface IConfiguracoesEmailRepository
{
    Task<string?> ObterValorPorChaveAsync(string chave);

    Task<string?> ObterEmailPorChaveAsync(string chave);
}
