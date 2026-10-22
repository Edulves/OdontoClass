using OdontoClass.Services.Interfaces;
using Resend;

namespace OdontoClass.Services;

public class EmailService : IEmailService
{
    private readonly IResend _resend;
    private readonly IConfiguration _configuration;

    public EmailService(IResend resend, IConfiguration configuration)
    {
        _resend = resend;
        _configuration = configuration;
    }   


    public async Task SendAsync(
        string destinatario,
        string assunto,
        string mensagemHtml)
    {
        var from = _configuration["Resend:FromEmail"]
        ?? throw new InvalidOperationException("O remetente do Resend não foi configurado.");

        var email = new EmailMessage
        {
            From = from,
            Subject = assunto,
            HtmlBody = mensagemHtml
        };

        email.To.Add(destinatario);

        await _resend.EmailSendAsync(email);
    }
}
