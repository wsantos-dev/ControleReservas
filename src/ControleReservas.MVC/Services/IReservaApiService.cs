using ControleReservas.Application.DTOs;
using ControleReservas.MVC.Models;
using System;
using System.Threading.Tasks;

namespace ControleReservas.MVC.Services;

public interface IReservaApiService
{
    
    Task<IEnumerable<ReservaViewModel>> ObterReservasAsync();
    Task<ReservaViewModel?> ObterPorIdAsync(Guid id);
    Task CriarAsync(ReservaViewModel vm);
    Task<ReservaViewModel> EditarAsync(Guid id, ReservaViewModel dto);
    Task CancelarAsync(Guid id);
}
