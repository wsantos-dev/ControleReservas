using ControleReservas.Application.DTOs;
using ControleReservas.Domain;


namespace ControleReservas.Application.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> ObterUsuariosAsync();
    Task<UsuarioDto?> ObterPorIdAsync(Guid id);
    Task<UsuarioDto> CriarAsync(UsuarioDto dto);
    Task AtualizarAsync(UsuarioDto dto);
    Task RemoverAsync(Guid id);
}
