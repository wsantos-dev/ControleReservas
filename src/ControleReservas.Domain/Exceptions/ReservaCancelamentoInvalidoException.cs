using System;

namespace ControleReservas.Domain.Exceptions;

public class ReservaCancelamentoInvalidoException : Exception
{
    public ReservaCancelamentoInvalidoException() 
        : base("Cancelamento inválido: Faltam menos de 24 horas para o início da reserva.")
    {
    }
}
