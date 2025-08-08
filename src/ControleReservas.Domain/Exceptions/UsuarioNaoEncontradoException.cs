using System;

namespace ControleReservas.Domain.Exceptions;

public class UsuarioNaoEncontradoException : Exception
{
    public UsuarioNaoEncontradoException() : base("Usuário não encontrado.")
    {
    }
}
