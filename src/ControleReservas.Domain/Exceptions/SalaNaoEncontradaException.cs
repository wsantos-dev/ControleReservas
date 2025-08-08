using System;

namespace ControleReservas.Domain.Exceptions;

public class SalaNaoEncontradaException : Exception
{
    public SalaNaoEncontradaException() : base("Sala não encontrada.")
    {
    }
}
