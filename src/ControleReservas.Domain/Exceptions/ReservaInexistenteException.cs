using System;

namespace ControleReservas.Domain.Exceptions;

public class ReservaInexistenteException : Exception
{

    public ReservaInexistenteException() : base("Reserva não encontrada.")
    {
    }
}
