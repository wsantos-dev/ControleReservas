namespace ControleReservas.Domain.Exceptions
{
    public class CancelamentoExpiradoException : Exception
    {
        public CancelamentoExpiradoException() 
            : base("Cancelamento expirado: O prazo para cancelamento da reserva já foi ultrapassado.")
        {
        }
    }
}
