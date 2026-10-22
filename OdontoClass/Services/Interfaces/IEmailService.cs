namespace OdontoClass.Services.Interfaces;

public interface IEmailService
{
    Task SendAsync(
        string destinatario,
        string assunto,
        string mensagemHtml);
}
