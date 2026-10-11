using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NorthWind.Entities.Interfaces;
using NorthWind.Sales.Backend.SmtpGateways.Options;

namespace NorthWind.Sales.Backend.SmtpGateways.Services;

internal sealed class MailService(
    IOptions<SmtpOptions> options,
    ILogger<MailService> logger) : IMailService
{
    public async Task SendMailToAdministrator(string subject, string body)
    {
        try
        {
            var settings = options.Value;
            if (string.IsNullOrWhiteSpace(settings.SmtpHost)
                || string.IsNullOrWhiteSpace(settings.SmtpUserName)
                || string.IsNullOrWhiteSpace(settings.SmtpPassword)
                || string.IsNullOrWhiteSpace(settings.SenderEmail)
                || string.IsNullOrWhiteSpace(settings.AdministratorEmail))
            {
                throw new InvalidOperationException("SMTP is not configured for special-order notifications.");
            }

            using var client = new SmtpClient(settings.SmtpHost, settings.SmtpHostPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(settings.SmtpUserName, settings.SmtpPassword)
            };
            using var message = new MailMessage(settings.SenderEmail, settings.AdministratorEmail, subject, body);
            await client.SendMailAsync(message);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Could not send the special-order notification email.");
            throw;
        }
    }
}
