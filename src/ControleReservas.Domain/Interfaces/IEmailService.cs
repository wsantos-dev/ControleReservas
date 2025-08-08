using System;

namespace ControleReservas.Domain.Interfaces;

public interface IEmailService
{
    Task EnviarEmailAsync(string destinatario, string assunto, string mensagemHtml);

}
