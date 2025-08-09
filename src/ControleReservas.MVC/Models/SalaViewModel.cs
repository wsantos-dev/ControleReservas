using System;

namespace ControleReservas.MVC.Models;

public class SalaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Capacidade { get; set; }
}
