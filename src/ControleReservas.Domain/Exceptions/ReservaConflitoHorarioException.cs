using System;

namespace ControleReservas.Domain.Exceptions;

public class ReservaConflitoHorarioException : Exception
{
    public ReservaConflitoHorarioException() : base("Já existe uma reserva confirmada para esse horário.")
    {
    }
}
