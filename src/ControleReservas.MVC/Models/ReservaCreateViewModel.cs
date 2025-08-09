using System;

namespace ControleReservas.MVC.Models;

using System.ComponentModel.DataAnnotations;

public class ReservaCreateViewModel
{
    [Required]
    public Guid SalaId { get; set; }

    [Required]
    public Guid UsuarioId { get; set; }

    [Required]
    [Display(Name = "Data de Início")]
    public DateTime DataHoraInicio { get; set; }

    [Required]
    [Display(Name = "Data Final")]
    public DateTime DataHoraFim { get; set; }

    [Display(Name = "Data de Cancelamento")]
    public DateTime? DataCancelamento { get; set; }

}
