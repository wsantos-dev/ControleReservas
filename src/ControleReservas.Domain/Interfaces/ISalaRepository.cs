using System;

namespace ControleReservas.Domain.Interfaces;

public interface ISalaRepository : IRespository<Sala>
{
    Task<bool> SalaExisteAsync(string nome);
}
