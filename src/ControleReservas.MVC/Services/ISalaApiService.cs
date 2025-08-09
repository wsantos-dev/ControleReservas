using System;
using ControleReservas.MVC.Models;

namespace ControleReservas.MVC.Services;

public interface ISalaApiService
{
    Task<IEnumerable<SalaViewModel>> ObterSalasAsync();
    Task<SalaViewModel?> ObterPorIdAsync(Guid id);
    Task CriarAsync(SalaViewModel vm);
    Task AtualizarAsync(SalaViewModel vm);
    Task RemoverAsync(Guid id);
}
