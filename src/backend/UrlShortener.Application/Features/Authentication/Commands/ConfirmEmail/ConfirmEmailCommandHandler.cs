using MediatR;
using UrlShortener.Application.Features.Authentication.Interfaces;

namespace UrlShortener.Application.Features.Authentication.Commands.ConfirmEmail
{
    internal class ConfirmEmailCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ConfirmEmailCommand, string>
    {
        public async Task<string> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {



            return await authenticationService.ConformEmail(request);
        }
    }
}
