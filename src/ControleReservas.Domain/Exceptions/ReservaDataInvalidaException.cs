using System;

namespace ControleReservas.Domain.Exceptions;

public class ReservaDataInvalidaException : Exception
{
    public ReservaDataInvalidaException() : base("A data e hora de término não podem ser menores ou iguais à data e hora de início.") { }
}