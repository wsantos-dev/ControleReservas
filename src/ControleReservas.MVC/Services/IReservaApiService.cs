using System;
using ControleReservas.MVC.Models;

namespace ControleReservas.MVC.Services;

public interface IReservaApiService
{
    
    Task<IEnumerable<ReservaViewModel>> ObterReservasAsync();
    Task<ReservaViewModel?> ObterPorIdAsync(Guid id);
    Task CriarAsync(ReservaViewModel vm);
    Task CancelarAsync(Guid id);
}
