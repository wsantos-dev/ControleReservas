using System;
using ControleReservas.MVC.Models;

namespace ControleReservas.MVC.Services;

public interface IUsuarioApiService 
{
    Task<IEnumerable<UsuarioViewModel>> ObterUsuariosAsync();
    Task<UsuarioViewModel?> ObterPorIdAsync(Guid id);
    Task CriarAsync(UsuarioViewModel vm);
    Task AtualizarAsync(UsuarioViewModel vm);
    Task RemoverAsync(Guid id);
}
