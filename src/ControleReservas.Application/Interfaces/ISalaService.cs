using ControleReservas.Application.DTOs;

namespace ControleReservas.Application.Interfaces;

public interface ISalaService
{
    Task<IEnumerable<SalaDto>> ObterSalasAsync();
    Task<SalaDto?> ObterPorIdAsync(Guid id);
    Task CriarAsync(SalaDto dto);
    
}
