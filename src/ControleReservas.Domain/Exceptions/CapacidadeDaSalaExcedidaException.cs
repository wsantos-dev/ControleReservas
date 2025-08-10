using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleReservas.Domain.Exceptions
{
    public class CapacidadeDaSalaExcedidaException : Exception
    {
        public CapacidadeDaSalaExcedidaException(string mensagem) 
            : base(mensagem) { }
    }
}
