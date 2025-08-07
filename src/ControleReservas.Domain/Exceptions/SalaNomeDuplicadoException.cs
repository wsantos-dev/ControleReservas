using System;

namespace ControleReservas.Domain.Exceptions;

public class SalaNomeDuplicadoException : Exception
{

    public SalaNomeDuplicadoException() : base("Já existe uma sala com esse nome.")
    {
        
    }
}
