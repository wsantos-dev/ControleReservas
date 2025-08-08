using System;
using ControleReservas.Domain.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ControleReservas.Application.Services;

public class EmailService : IEmailService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmailService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task EnviarEmailAsync(string destinatario, string assunto, string mensagemHtml)
    {
        var apiKey = await _unitOfWork.ConfiguracoesEmail.ObterValorPorChaveAsync("ControleReservaAPI");
        var email = await _unitOfWork.ConfiguracoesEmail.ObterEmailPorChaveAsync("ControleReservaAPI");

        if (string.IsNullOrEmpty(apiKey))
            throw new Exception("Chave da API do SendGrid não encontrada no banco.");

        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(email, "Sistema de Reservas");
        var to = new EmailAddress(destinatario);
        var msg = MailHelper.CreateSingleEmail(from, to, assunto, "", mensagemHtml);

        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Falha ao enviar e-mail. Código: {response.StatusCode}");

    }
}
