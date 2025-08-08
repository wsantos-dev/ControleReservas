using System;

namespace ControleReservas.Domain.Interfaces;

public interface IUnitOfWork
{
    ISalaRepository Salas { get; }
    IUsuarioRepository Usuarios { get; }
    IReservaRepository Reservas { get; }
    IConfiguracoesEmailRepository ConfiguracoesEmail { get; }

    Task CommitAsync();
}
