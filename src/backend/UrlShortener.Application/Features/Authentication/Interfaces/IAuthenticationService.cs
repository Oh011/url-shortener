using UrlShortener.Application.Features.Authentication.Commands.ConfirmEmail;
using UrlShortener.Application.Features.Authentication.Commands.LogIn;
using UrlShortener.Application.Features.Authentication.Commands.Register;
using UrlShortener.Application.Features.Authentication.Commands.ResendConfirmationEmail;
using UrlShortener.Application.Features.Authentication.Dtos;

namespace UrlShortener.Application.Features.Authentication.Interfaces
{
    public interface IAuthenticationService
    {


        Task<string> Register(RegisterUserCommand command);

        Task<LogInResponseWithRefreshToken> LogIn(LogInUserCommand command);

        Task<string> ResendConfirmationEmail(ResendConfirmationEmailCommand command);
        Task<string> ConformEmail(ConfirmEmailCommand command);
    }
}
