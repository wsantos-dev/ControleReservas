using System;

namespace ControleReservas.Domain.Exceptions;

public class ReservaCancelamentoInvalidoException : Exception
{
    public ReservaCancelamentoInvalidoException() : base("Cancelamentos só são permitidos com no mínimo 24 horas de antecedência.")
    {
    }
}
