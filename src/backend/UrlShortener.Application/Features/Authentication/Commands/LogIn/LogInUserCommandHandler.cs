using MediatR;
using UrlShortener.Application.Features.Authentication.Dtos;
using UrlShortener.Application.Features.Authentication.Interfaces;

namespace UrlShortener.Application.Features.Authentication.Commands.LogIn
{
    public class LogInUserCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<LogInUserCommand, LogInResponseWithRefreshToken>
    {



        public async Task<LogInResponseWithRefreshToken> Handle(LogInUserCommand request, CancellationToken cancellationToken)
        {


            return await authenticationService.LogIn(request);
        }
    }
}
