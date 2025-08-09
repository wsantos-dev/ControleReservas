using System;

namespace ControleReservas.MVC.Models;

public class UsuarioViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}