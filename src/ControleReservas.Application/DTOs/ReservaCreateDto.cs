using System;

namespace ControleReservas.Application.DTOs;

public class ReservaCreateDto
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public DateTime DateHoraReserva { get; set; }
}
