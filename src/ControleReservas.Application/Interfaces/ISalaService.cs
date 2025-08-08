using ControleReservas.Application.DTOs;

namespace ControleReservas.Application.Interfaces;

public interface ISalaService
{
    Task<IEnumerable<SalaDto>> ObterSalasAsync();
    Task<SalaDto?> ObterPorIdAsync(Guid id);
    Task<SalaDto> CriarAsync(SalaDto dto);
    Task AtualizarAsync(SalaDto dto);
    Task RemoverAsync(Guid id);


    
}
