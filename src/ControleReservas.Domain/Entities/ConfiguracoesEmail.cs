using System;

namespace ControleReservas.Domain.Entities;


public class ConfiguracoesEmail
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Chave { get; set; } = null!;
    public string Valor { get; set; } = null!;


}