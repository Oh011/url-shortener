using Project.Application.Features.Authentication.Commands.ConfirmEmail;
using Project.Application.Features.Authentication.Commands.LogIn;
using Project.Application.Features.Authentication.Commands.Register;
using Project.Application.Features.Authentication.Commands.ResendConfirmationEmail;
using Project.Application.Features.Authentication.Dtos;

namespace Project.Application.Features.Authentication.Interfaces
{
    public interface IAuthenticationService
    {


        Task<string> Register(RegisterUserCommand command);

        Task<LogInResponseWithRefreshToken> LogIn(LogInUserCommand command);

        Task<string> ResendConfirmationEmail(ResendConfirmationEmailCommand command);
        Task<string> ConformEmail(ConfirmEmailCommand command);
    }
}
