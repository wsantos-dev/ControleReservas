using ControleReservas.Application.DTOs;

namespace ControleReservas.Application.Interfaces;

public interface IReservaService
{
    Task<IEnumerable<ReservaDto>> ObterReservasAsync();
    Task<ReservaDto?> ObterPorIdAsync(Guid id);
    Task CriarAsync(ReservaCreateDto dto);
    Task CancelarAsync(Guid id);

}
