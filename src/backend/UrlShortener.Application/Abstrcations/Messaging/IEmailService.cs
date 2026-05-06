using Shared.Dtos;

namespace UrlShortener.Application.Abstrcations.Messaging
{
    public interface IEmailService
    {



        public Task SendEmailAsync(EmailMessage message);
    }
}
