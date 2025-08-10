using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleReservas.Domain.Exceptions
{
    public class ReservaCanceladaNaoPodeSerEditadaException : Exception
    { 
        public ReservaCanceladaNaoPodeSerEditadaException() 
            : base("Reservas canceladas não podem ser editadas.") { }
    }
}
