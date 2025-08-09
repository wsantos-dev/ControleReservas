using System;

namespace ControleReservas.MVC.Models;

public class ReservaViewModel
{
    public Guid Id { get; set; }
    public Guid SalaId { get; set; }
    public string SalaNome { get; set; } = string.Empty;
    public Guid UsuarioId { get; set; }
    public string UsuarioNome { get; set; } = string.Empty;
    public DateTime DataHoraInicio { get; set; }
    public DateTime DataHoraFim { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? DataCancelamento { get; set; }
}
