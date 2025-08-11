using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleReservas.Domain.Exceptions
{
    public class SalaComReservaExistenteException : Exception
    {
        public SalaComReservaExistenteException(int quantidade)
            : base($"Não é possível deletar a sala, pois existe(m) {quantidade} reserva(s) associada(s) a ela.")
        {
        }
    }
}
