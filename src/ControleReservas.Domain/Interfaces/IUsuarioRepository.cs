using System;

namespace ControleReservas.Domain.Interfaces;

public interface IUsuarioRepository : IRespository<Usuario>
{
    Task<Usuario?> ObterPorEmailAsync(string email);
    
}
