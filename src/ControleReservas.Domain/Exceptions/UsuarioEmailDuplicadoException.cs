using System;

namespace ControleReservas.Domain.Exceptions;

public class UsuarioEmailDuplicadoException : Exception
{
    public UsuarioEmailDuplicadoException() : base ("Já existe um usuário com esse e-mail.")
    {
    }
}
