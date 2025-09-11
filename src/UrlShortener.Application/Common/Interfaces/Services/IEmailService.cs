using Shared.Dtos;

namespace Project.Application.Common.Interfaces.Services
{
    public interface IEmailService
    {



        public Task SendEmailAsync(EmailMessage message);
    }
}
