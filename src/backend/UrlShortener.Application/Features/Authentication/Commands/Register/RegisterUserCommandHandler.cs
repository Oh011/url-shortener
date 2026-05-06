using MediatR;
using UrlShortener.Application.Features.Authentication.Interfaces;

namespace UrlShortener.Application.Features.Authentication.Commands.Register
{
    internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {

        private readonly IAuthenticationService authenticationService;


        public RegisterUserCommandHandler(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {


            return await authenticationService.Register(request);
        }
    }
}
