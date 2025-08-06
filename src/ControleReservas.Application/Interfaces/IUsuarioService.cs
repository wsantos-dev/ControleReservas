using ControleReservas.Application.DTOs;


namespace ControleReservas.Application.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> ObterUsuariosAsync();
    Task<UsuarioDto?> ObterPorIdAsync(Guid id);
    Task CriarAsync(UsuarioDto dto);
}
