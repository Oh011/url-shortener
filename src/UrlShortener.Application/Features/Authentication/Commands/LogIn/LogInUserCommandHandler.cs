using MediatR;
using Project.Application.Features.Authentication.Dtos;
using Project.Application.Features.Authentication.Interfaces;

namespace Project.Application.Features.Authentication.Commands.LogIn
{
    public class LogInUserCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<LogInUserCommand, LogInResponseWithRefreshToken>
    {



        public async Task<LogInResponseWithRefreshToken> Handle(LogInUserCommand request, CancellationToken cancellationToken)
        {


            return await authenticationService.LogIn(request);
        }
    }
}
