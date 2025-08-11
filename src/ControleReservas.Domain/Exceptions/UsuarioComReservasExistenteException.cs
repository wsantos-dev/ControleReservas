using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleReservas.Domain.Exceptions
{
    public class UsuarioComReservasExistenteException : Exception
    {
        public UsuarioComReservasExistenteException(int quantidade)
            : base($"Não é possível deletar o usuário, pois ele possui {quantidade} reserva(s) associada(s).")
        {
        }
    }
}
