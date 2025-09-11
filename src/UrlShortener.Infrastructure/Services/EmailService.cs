
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Project.Application.Common.Interfaces.Services;
using Shared.Dtos;
using Shared.Options;

namespace Project.Infrastructure.Services
{
    public class EmailService(IOptions<SmtpOptions> options, ILogger<IEmailService> _logger) : IEmailService
    {
        public async Task SendEmailAsync(EmailMessage message)
        {

            try
            {

                var values = options.Value;

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(values.DisplayName, values.From));
                email.To.Add(MailboxAddress.Parse(message.To));
                email.Subject = message.Subject;



                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Body };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(values.Host, values.Port, SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(values.UserName, values.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

            }

            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send confirmation email.");
            }
        }
    }
}
