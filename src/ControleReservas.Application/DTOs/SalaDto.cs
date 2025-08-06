namespace ControleReservas.Application.DTOs;

public class SalaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public int Capacidade { get; set; }
}
